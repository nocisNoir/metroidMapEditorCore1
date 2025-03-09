using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidMapEditorCore
{
    public class RoomBase : MonoBehaviour
    {
        public bool isSample;
        public Image outLineImg;
        public Image mainRoomImg;
        public Button mainRoomButton;
        public RectTransform _MainRoomRect;
        public UIObjDragController dragController;
        [Header("游戏性属性")]
        public Vector2Int _RoomSize; public int _RoomGridOffset;

        public Color _RoomColor;

        public string _RoomName;

        public List<DoorBase> doors;
        

        private void Awake()
        {
            if (isSample)
                return;
            if (_RoomGridOffset == 0)
            {
                _RoomGridOffset = 50;
            }
            _MainRoomRect = GetComponent<RectTransform>();
            mainRoomButton = GetComponent<Button>();
            if (_RoomSize.x > 0 && _RoomSize.y > 0&&_RoomGridOffset>0)
            {
                _MainRoomRect.sizeDelta = _RoomGridOffset * _RoomSize;
            }
            if (!dragController)
            {
                if(!GetComponent<UIObjDragController>())
                    gameObject.AddComponent<UIObjDragController>();
                dragController = GetComponent<UIObjDragController>();
                dragController._DragGridOffset = _RoomGridOffset;
            }
            
        }
        private void Start()
        {
            if (isSample)
                return;
            if (_RoomName == "")
            {
                _RoomName = gameObject.name;
            }
            if (!outLineImg)
                outLineImg = GetComponent<Image>();
          //  InitializedEventTrigger();
            initImg();
            transform.localPosition = UIObjDragController.gridVector(transform.localPosition, _RoomGridOffset);
        }

        void initImg()
        {
            if (!mainRoomImg)
            {
                mainRoomImg = new GameObject("MainRoomImg").AddComponent<Image>();
            }
           // mainRoomImg.transform.parent = transform;
            // mainRoomImg.rectTransform.        // 设置父对象为当前脚本所在的 GameObject
            mainRoomImg.transform.SetParent(transform, false);
            mainRoomImg.transform.localPosition = Vector2.zero;
            mainRoomImg.transform.localScale = Vector3.one;
            mainRoomImg.transform.SetAsFirstSibling();
            refreshMainRoomImgSize();
            mainRoomImg.color = outLineImg.color;
            outLineImg.color = Color.black;
            outLineImg.enabled = false;
        }
        private void Update()
        {

        }

        void refreshMainRoomImgSize()
        {
            // 获取 RectTransform
            RectTransform rectTransform = mainRoomImg.rectTransform;
            rectTransform.localScale = Vector3.one;

            // 设置 RectTransform 的锚点为全屏拉伸
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;

            // 设置 RectTransform 的偏移量，上下左右各减少 10 个单位
            rectTransform.offsetMin = new Vector2(10, 10); // 左下角偏移 (left, bottom)
            rectTransform.offsetMax = new Vector2(-10, -10); // 右上角偏移 (right, top)
        }
        public void ReportRoomInitialize(string info = "")
        {
            //这里给button加个点击事件
            Debug.Log(Time.time + "房间" + _RoomName + "初始化。。。" + info+"房间面板？"+RoomBoard.mainRoomBoard+"按钮？"+mainRoomButton);
            mainRoomButton.onClick.AddListener(() => RoomBoard.mainRoomBoard.ClickRoom(this));
        }
        public void setRoomSize(int x=0,int y=0,int gridOffset=0)
        {
            Vector2Int aimSize = new Vector2Int(x, y);
            if (aimSize != Vector2.zero)
            {
                _RoomSize = aimSize;
            }
            if (gridOffset != 0)
            {
                _RoomGridOffset = gridOffset;
            }
            GetComponent<RectTransform>().sizeDelta = gridOffset * _RoomSize;
            if (aimSize != Vector2.zero || gridOffset != 0)
            {
                RefreshDoorPointWhileResetSize();
            }
        }

        public void addNewDoor()
        {
            //bool allow0;
            int newDoorPos = getFirstEmptyPointID();
            if (newDoorPos >= 0)// || allow0)
            {
                //新增一个门
                DoorBase door = Instantiate(SampleUIObjs.main.sampleDoor, transform);
                door.InitByAttachRoom = true;
                //door.doorTransform.localPosition = GetNearestPointOnRoomEdge(door.doorTransform, out eip);

                EdgeIndexPair eip = GetLogicRectOffsetByIndex(newDoorPos, _RoomGridOffset, _MainRoomRect);
                door.doorTransform.localPosition = GetPositionOnEdge(newDoorPos, _RoomGridOffset);
                door._AttachRoom = this;
                door.setEip(eip);//新建门？
                doors.Add(door); door.gameObject.SetActive(true);

            }
            else
            {
                Debug.LogError("无空位不创建");
            }

        }


        public void setDoorToNewLegalPos(DoorBase door)
        {
            if (!doors.Contains(door))
                Debug.LogError("未找到房间");
            // bool allow0;
            int newDoorPos = getFirstEmptyPointID();//out allow0);
            if (newDoorPos >= 0)// || allow0)
            {
                EdgeIndexPair eip = GetLogicRectOffsetByIndex(newDoorPos, _RoomGridOffset, _MainRoomRect);
                door.doorTransform.localPosition = GetPositionOnEdge(newDoorPos, _RoomGridOffset);
                door.setEip(eip);
            }
            else
            {
                Debug.LogError("无空位不移动");
            }
        }

        public void setDoorToInputPos(DoorBase door,EdgeIndexPair eip)
        {
            if (!doors.Contains(door))
                Debug.LogError("未找到房间");
            if (checkGetDoorsInEipPos(eip, out _))
                Debug.LogError("位置" + eip.logInfo() + "已有门");
            //把房间塞到eip位置上面去
            door.doorTransform.localPosition = GetPositionOnEdge(eip, _RoomGridOffset);
            door.setEip(eip);

        }
        //用于给房间设置网格上面的位置
        public void refreshRoomDoorsDragState(DoorBase door)
        {

            if (!doors.Contains(door))
            {
                Debug.LogError($"输入非此房间的门{door.gameObject.name}");
            }
            
            doors.ForEach((d) => d.dragController.onDragPrepare(false));
            door.dragController.onDragPrepare(true);
        }

        public void SetColor(Color color)
        {
            mainRoomImg.color = color;
            _RoomColor = color;
            Debug.Log("房间" + name + "改色号" + color);
        }


        public void RefreshDoorPointWhileResetSize(bool ifDelete = true)
        {
            RectTransform.Edge nowEdge = RectTransform.Edge.Left;
            for(int edgeId = 0; edgeId < 4; edgeId++)
            {
                nowEdge = GetNextEdge(nowEdge);

                //从顶边开始
                List<DoorBase> nowEdgeDoor = new List<DoorBase>();
                foreach(DoorBase d in doors)
                {
                    if (d.getEip().Edge == nowEdge)
                        nowEdgeDoor.Add(d);
                }
                nowEdgeDoor.Sort((door1, door2) => door1.getEip().Id.CompareTo(door2.getEip().Id));

                float length = (nowEdge == RectTransform.Edge.Left || nowEdge == RectTransform.Edge.Right)
                    ? _MainRoomRect.rect.height
                    : _MainRoomRect.rect.width;
                int legalPointNum = GetPointsNumOnEdge(length, _RoomGridOffset);
                //先把多余的去掉
              //  Debug.LogError($"边{nowEdge}可用点数{legalPointNum}");
                List<DoorBase> overflowDoors = new List<DoorBase>();
                if (nowEdgeDoor.Count > legalPointNum)
                {
                    overflowDoors = nowEdgeDoor.GetRange(legalPointNum, nowEdgeDoor.Count - legalPointNum);
                    nowEdgeDoor.RemoveRange(legalPointNum, nowEdgeDoor.Count - legalPointNum);
                }

                //现在是长度合适的list了
                legalPointNum--;
                int lastCount = nowEdgeDoor.Count - 1;
                while (lastCount>0&&legalPointNum < nowEdgeDoor[lastCount].getEip().Id)
                {
                    nowEdgeDoor[lastCount].setEip(legalPointNum);
                    
                    legalPointNum--;
                    lastCount--;
                }
                //削弱完了该给这些门全部刷新一边？

                foreach(DoorBase door in nowEdgeDoor)
                {
                    door.RefreshDoorPos();
                    door.gameObject.SetActive(true);
                }
                foreach(DoorBase door in overflowDoors)
                {
                    door.gameObject.SetActive(false);
                }

            }
        }

        
        /// <summary>
        /// 以下全都是获取房间上点位相关的函数
        /// </summary>
        bool checkGetDoorsInEipPos(EdgeIndexPair nowEip, out DoorBase[] doorsOnPos)
        {
            List<DoorBase> doorBases = new List<DoorBase>();
            bool getDoor = false;

            foreach (DoorBase door in doors)
            {
                if (door.checkEdgeIndexPair(nowEip))
                {
                    getDoor = true;
                    doorBases.Add(door);
                }
            }
            doorsOnPos = doorBases.ToArray();
            return getDoor;

        }
        int getFirstEmptyPointID()//out bool first0)
        {
           // first0 = true;
            //遍历这个房间每个合法锚点，找到第一个空位时，返回
            for(int i = 0; i < GetLegalPointNum(_MainRoomRect, _RoomGridOffset); i++)
            {
                //bool isEmpty=true;
                EdgeIndexPair nowEip = GetLogicRectOffsetByIndex(i,_RoomGridOffset,_MainRoomRect);
               if(!checkGetDoorsInEipPos(nowEip, out _))
                {
                  //  first0 = (i == 0);
                    return i;
                    //检测此逻辑位置无门
                }
            }
            Debug.LogError("未找到空位！！！");
            return -1;
        }

        public Vector2 GetNearestPointOnRoomEdge(RectTransform objRect,out EdgeIndexPair edgeIndex)
        {
            int maxPointNum = GetLegalPointNum(_MainRoomRect, _RoomGridOffset);
            Vector2[] points = new Vector2[maxPointNum];
            for(int i=0; i < maxPointNum; i++)
            {
                points[i] = GetPositionOnEdge(i);
            }
            int AimPointID = FindNearestPointIndex(points, objRect.localPosition);
            edgeIndex = GetLogicRectOffsetByIndex(AimPointID, _RoomGridOffset, _MainRoomRect);
           Debug.LogWarning($"最近点{points[AimPointID]}序号{AimPointID},逻辑位置{edgeIndex.logInfo()}" );
            return points[AimPointID];
        }

        public Vector2 GetPositionOnEdge(EdgeIndexPair edgeIndexPair,int dOffset = -1, RectTransform rectTransform = null)
        {
            if (!rectTransform)
                rectTransform = _MainRoomRect;
            if (dOffset == -1)
                dOffset = _RoomGridOffset;
            Debug.LogWarning($"rect：{rectTransform}，距离？{dOffset}");
            int id = edgeIndexPair.Id;
            RectTransform.Edge edge = edgeIndexPair.Edge;
            id = id % GetLegalPointNum(rectTransform, dOffset);
            EdgeIndexPair eip = GetLogicRectOffsetByIndex(id, dOffset, rectTransform, edge);
            // 获取 RectTransform 的宽高
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            // 计算目标位置的偏移量
            float offset = eip.Id * dOffset;

            Vector2 position = Vector2.zero;
            switch (eip.Edge)
            {
                case RectTransform.Edge.Top:
                    position = new Vector2(-width / 2 + offset, height / 2); // 上边从左到右
                    break;
                case RectTransform.Edge.Right:
                    position = new Vector2(width / 2, height / 2 - offset); // 右边从上到下
                    break;
                case RectTransform.Edge.Bottom:
                    position = new Vector2(width / 2 - offset, -height / 2); // 下边从右到左
                    break;
                case RectTransform.Edge.Left:
                    position = new Vector2(-width / 2, -height / 2 + offset); // 左边从下到上
                    break;
            }
            // 将局部坐标转换为世界坐标
            return position; //rectTransform.TransformPoint(position);
        }

        public static RectTransform.Edge GetNextEdge(RectTransform.Edge currentEdge)
        {
            switch (currentEdge)
            {
                case RectTransform.Edge.Top:
                    return RectTransform.Edge.Right;
                case RectTransform.Edge.Right:
                    return RectTransform.Edge.Bottom;
                case RectTransform.Edge.Bottom:
                    return RectTransform.Edge.Left;
                case RectTransform.Edge.Left:
                    return RectTransform.Edge.Top;
                default:
                    return RectTransform.Edge.Top; // 默认返回上边
            }
        }

        public static RectTransform.Edge GetPreviousEdge(RectTransform.Edge currentEdge)
        {
            switch (currentEdge)
            {
                case RectTransform.Edge.Top:
                    return RectTransform.Edge.Left;
                case RectTransform.Edge.Right:
                    return RectTransform.Edge.Top;
                case RectTransform.Edge.Bottom:
                    return RectTransform.Edge.Right;
                case RectTransform.Edge.Left:
                    return RectTransform.Edge.Bottom;
                default:
                    return RectTransform.Edge.Top; // 默认返回上边
            }
        }
        public Vector2 GetPositionOnEdge(int id, int doffset = 50, RectTransform rectTransform = null)
        {
            return GetPositionOnEdge(new EdgeIndexPair(id,RectTransform.Edge.Top), doffset, rectTransform);
        }
        // 计算 RectTransform 四条边上所有合法点的个数之和
        public static int GetLegalPointNum(RectTransform rect, int poffset=-1)
        {
            
            // 获取 RectTransform 的宽高
            float width = rect.rect.width;
            float height = rect.rect.height;

            // 计算每条边的合法点数
            int topPoints = GetPointsNumOnEdge(width, poffset);    // 上边
            int rightPoints = GetPointsNumOnEdge(height, poffset); // 右边
            int bottomPoints = GetPointsNumOnEdge(width, poffset); // 下边
            int leftPoints = GetPointsNumOnEdge(height,poffset);  // 左边

           // Debug.LogWarning($"4边点数量{topPoints}，{rightPoints}，边长{width},{height}");
            // 返回四条边的合法点数之和
            return topPoints + rightPoints + bottomPoints + leftPoints;
        }
         static int GetPointsNumOnEdge(float edgeLength, int pOffset)
        {
            // 合法点数 = 边长度 / 间隔的整数部分
            int points = Mathf.FloorToInt(edgeLength / pOffset);
            return points;
        }
         static int GetPointNumOnEdge(RectTransform.Edge edge,RectTransform rect,int pOffset)
        {
            float length = (edge == RectTransform.Edge.Left || edge == RectTransform.Edge.Right)
                ? rect.rect.height
                : rect.rect.width;
            int legalPointNum = GetPointsNumOnEdge(length, pOffset);// + 1;
            return legalPointNum;
        }
        public int GetPointNumOnEdge(RectTransform.Edge edge)
        {
            return GetPointNumOnEdge(edge, _MainRoomRect, _RoomGridOffset);
        }

        // 返回距离 pos 最近的点的序号
        public static int FindNearestPointIndex(Vector2[] points, Vector2 pos)
        {
            if (points == null || points.Length == 0)
            {
                Debug.LogError("points 数组为空或未初始化");
                return -1; // 返回 -1 表示无效
            }

            int nearestIndex = 0; // 最近点的序号
            float minDistance = Vector2.Distance(points[0], pos); // 最小距离

            // 遍历 points 数组
            for (int i = 1; i < points.Length; i++)
            {
                float distance = Vector2.Distance(points[i], pos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestIndex = i;
                }
            }
            Debug.LogWarning($"{pos}最近的点是{points[nearestIndex]}");
            return nearestIndex;
        }

        //输入一个id，返回这个id对应的边和此边上的id
        public static EdgeIndexPair GetLogicRectOffsetByIndex(int id,int dOffset,RectTransform rect,RectTransform.Edge edge=RectTransform.Edge.Top)
        {
         //   int i = 0;
           // int[] temp = new int[5] {-1,-1,-1,-1,-1 };
           // temp[0] = id;
            id = id % GetLegalPointNum(rect, dOffset);
            //temp[1] = id;
            // 计算目标位置的偏移量
            float width = rect.rect.width;
            float height = rect.rect.height;
            // 计算边的长度
            float edgeLength = width;
            switch (edge)
            {
                case RectTransform.Edge.Top:
                case RectTransform.Edge.Bottom:
                    edgeLength = width;
                    break;
                case RectTransform.Edge.Left:
                case RectTransform.Edge.Right:
                    edgeLength = height;
                    break;
            }
            float offset = id * dOffset;

            while (offset >= edgeLength)
            {
               // if (i < 3)
                 //   i++;
                id -= GetPointsNumOnEdge(edgeLength, dOffset);
               // temp[i + 1] = id;
                offset = id * dOffset; // 下一个边算id
                edge = GetNextEdge(edge); // 顺时针旋转到下一个边

                // 重新计算新边的长度
                switch (edge)
                {
                    case RectTransform.Edge.Top:
                    case RectTransform.Edge.Bottom:
                        edgeLength = width;
                        break;
                    case RectTransform.Edge.Left:
                    case RectTransform.Edge.Right:
                        edgeLength = height;
                        break;
                }
            }

          //  Debug.Log($"计算顺序{temp[0]},{temp[1]},{temp[2]},{temp[3]},{temp[4]},id：{id}，边：{edge}");
            return new EdgeIndexPair(id, edge); 
        }

    }
        [System.Serializable]
    public struct EdgeIndexPair
    {

        public EdgeIndexPair(int id, RectTransform.Edge edge) : this()
        {
            Id = id;
            Edge = edge;
        }

        public static bool operator ==(EdgeIndexPair a, EdgeIndexPair b)
        {
            return a.Edge == b.Edge && a.Id == b.Id;
        }

        // 重写 != 运算符
        public static bool operator !=(EdgeIndexPair a, EdgeIndexPair b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (obj is EdgeIndexPair other)
            {
                return this == other;
            }
            return false;
        }
        public string logInfo()
        {
            return $"边{Edge.ToString()}序号{Id}";
        }
        // 重写 GetHashCode 方法
        public override int GetHashCode()
        {
            // 使用 _edge 和 _index 的哈希值组合
            return Edge.GetHashCode() ^ Id.GetHashCode();
        }

        [SerializeField] public int Id;//{ get; }
        [SerializeField] public RectTransform.Edge Edge;// { get; }
    }
}

