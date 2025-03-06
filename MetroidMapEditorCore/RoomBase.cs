using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidMapEditorCore
{
    public class RoomBase : MonoBehaviour
    {
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
            }
            
        }
        private void Start()
        {
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
            mainRoomImg.transform.parent = transform;
            // mainRoomImg.rectTransform.        // 设置父对象为当前脚本所在的 GameObject
            mainRoomImg.transform.SetParent(transform, false);
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
                _RoomSize = aimSize;
            if (gridOffset != 0)
                _RoomGridOffset = gridOffset;
            GetComponent<RectTransform>().sizeDelta = gridOffset * _RoomSize;
        }

        public void addNewDoor()
        {
            bool allow0;
            int newDoorPos = getFirstEmptyPointID(out allow0);
            if (newDoorPos != 0 || allow0)
            {
                //新增一个门
                DoorBase door = Instantiate(SampleUIObjs.main.sampleDoor, transform);
                door.InitByAttachRoom = true;
                //door.doorTransform.localPosition = GetNearestPointOnRoomEdge(door.doorTransform, out eip);

                EdgeIndexPair eip = GetLogicRectOffsetByIndex(newDoorPos, _RoomGridOffset, _MainRoomRect);
                door.doorTransform.localPosition = GetPositionOnEdge(newDoorPos, _RoomGridOffset);
                door.setEip(eip);
                doors.Add(door); door.gameObject.SetActive(true);

            }
            else
            {
                Debug.LogError("无空位不创建");
            }

        }

        //用于给房间设置网格上面的位置



        public void SetColor(Color color)
        {
            mainRoomImg.color = color;
            Debug.Log("房间" + name + "改色号" + color);
        }

        /// <summary>
        /// 以下全都是获取房间上点位相关的函数
        /// </summary>

        int getFirstEmptyPointID(out bool first0)
        {
            first0 = true;
            //遍历这个房间每个合法锚点，找到第一个空位时，返回
            for(int i = 0; i < GetLegalPointNum(_MainRoomRect, _RoomGridOffset); i++)
            {
                bool isEmpty=true;
                EdgeIndexPair nowEip = GetLogicRectOffsetByIndex(i,_RoomGridOffset,_MainRoomRect);
                foreach (DoorBase door in doors)
                {
                    if (door.checkEdgeIndexPair(nowEip))
                    {
                        isEmpty = false;
                        break;
                    }  
                }
                if (i > 0)
                    first0 = false;
                if (isEmpty)
                    return i;
            }
            Debug.LogError("未找到空位！！！");
            return 0;
        }

        public Vector2 GetNearestPointOnRoomEdge(RectTransform objRect,out EdgeIndexPair edgeIndex)
        {
            int maxPointNum = GetLegalPointNum(_MainRoomRect, 50);
            Vector2[] points = new Vector2[maxPointNum];
            for(int i=0; i < maxPointNum; i++)
            {
                points[i] = GetPositionOnEdge(i);
            }
            int AimPointID = FindNearestPointIndex(points, objRect.localPosition);
            edgeIndex = GetLogicRectOffsetByIndex(AimPointID, 50, _MainRoomRect);
            return points[AimPointID];
        }

        public Vector2 GetPositionOnEdge(RectTransform.Edge edge, int id, int dOffset = 50, RectTransform rectTransform = null)
        {
            if (!rectTransform)
                rectTransform = _MainRoomRect;
            Debug.LogWarning($"rect：{rectTransform}，距离？{dOffset}");
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

        public Vector2 GetPositionOnEdge(int id, int doffset = 50, RectTransform rectTransform = null)
        {
            return GetPositionOnEdge(RectTransform.Edge.Top, id, doffset, rectTransform);
        }
        // 计算 RectTransform 四条边上所有合法点的个数之和
        public static int GetLegalPointNum(RectTransform rect, int poffset)
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
        public static int GetPointsNumOnEdge(float edgeLength, int pOffset)
        {
            // 每个点的间隔是 50 单位
            // float interval = 50;

            // 合法点数 = 边长度 / 间隔的整数部分
            int points = Mathf.FloorToInt(edgeLength / pOffset);
            return points;
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
            id = id % GetLegalPointNum(rect, dOffset);
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

            while (offset > edgeLength)
            {
                id -= GetPointsNumOnEdge(edgeLength, dOffset);
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

            return new EdgeIndexPair(id, edge); 
        }

    }
        [System.Serializable]
    public struct EdgeIndexPair
    {
       public RectTransform.Edge _edge;
       public int _index;

        public EdgeIndexPair(int id, RectTransform.Edge edge) : this()
        {
            Id = id;
            Edge = edge;
        }

        public int Id { get; }
        public RectTransform.Edge Edge { get; }
    }
}

