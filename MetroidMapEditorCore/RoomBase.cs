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
        [Header("��Ϸ������")]
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
            // mainRoomImg.rectTransform.        // ���ø�����Ϊ��ǰ�ű����ڵ� GameObject
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
            // ��ȡ RectTransform
            RectTransform rectTransform = mainRoomImg.rectTransform;
            rectTransform.localScale = Vector3.one;

            // ���� RectTransform ��ê��Ϊȫ������
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;

            // ���� RectTransform ��ƫ�������������Ҹ����� 10 ����λ
            rectTransform.offsetMin = new Vector2(10, 10); // ���½�ƫ�� (left, bottom)
            rectTransform.offsetMax = new Vector2(-10, -10); // ���Ͻ�ƫ�� (right, top)
        }
        public void ReportRoomInitialize(string info = "")
        {
            //�����button�Ӹ�����¼�
            Debug.Log(Time.time + "����" + _RoomName + "��ʼ��������" + info+"������壿"+RoomBoard.mainRoomBoard+"��ť��"+mainRoomButton);
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
                //����һ����
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
                Debug.LogError("�޿�λ������");
            }

        }

        //���ڸ������������������λ��



        public void SetColor(Color color)
        {
            mainRoomImg.color = color;
            Debug.Log("����" + name + "��ɫ��" + color);
        }

        /// <summary>
        /// ����ȫ���ǻ�ȡ�����ϵ�λ��صĺ���
        /// </summary>

        int getFirstEmptyPointID(out bool first0)
        {
            first0 = true;
            //�����������ÿ���Ϸ�ê�㣬�ҵ���һ����λʱ������
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
            Debug.LogError("δ�ҵ���λ������");
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
            Debug.LogWarning($"rect��{rectTransform}�����룿{dOffset}");
            id = id % GetLegalPointNum(rectTransform, dOffset);
            EdgeIndexPair eip = GetLogicRectOffsetByIndex(id, dOffset, rectTransform, edge);
            // ��ȡ RectTransform �Ŀ��
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            // ����Ŀ��λ�õ�ƫ����
            float offset = eip.Id * dOffset;

            Vector2 position = Vector2.zero;
            switch (eip.Edge)
            {
                case RectTransform.Edge.Top:
                    position = new Vector2(-width / 2 + offset, height / 2); // �ϱߴ�����
                    break;
                case RectTransform.Edge.Right:
                    position = new Vector2(width / 2, height / 2 - offset); // �ұߴ��ϵ���
                    break;
                case RectTransform.Edge.Bottom:
                    position = new Vector2(width / 2 - offset, -height / 2); // �±ߴ��ҵ���
                    break;
                case RectTransform.Edge.Left:
                    position = new Vector2(-width / 2, -height / 2 + offset); // ��ߴ��µ���
                    break;
            }
            // ���ֲ�����ת��Ϊ��������
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
                    return RectTransform.Edge.Top; // Ĭ�Ϸ����ϱ�
            }
        }

        public Vector2 GetPositionOnEdge(int id, int doffset = 50, RectTransform rectTransform = null)
        {
            return GetPositionOnEdge(RectTransform.Edge.Top, id, doffset, rectTransform);
        }
        // ���� RectTransform �����������кϷ���ĸ���֮��
        public static int GetLegalPointNum(RectTransform rect, int poffset)
        {
            // ��ȡ RectTransform �Ŀ��
            float width = rect.rect.width;
            float height = rect.rect.height;

            // ����ÿ���ߵĺϷ�����
            int topPoints = GetPointsNumOnEdge(width, poffset);    // �ϱ�
            int rightPoints = GetPointsNumOnEdge(height, poffset); // �ұ�
            int bottomPoints = GetPointsNumOnEdge(width, poffset); // �±�
            int leftPoints = GetPointsNumOnEdge(height,poffset);  // ���

           // Debug.LogWarning($"4�ߵ�����{topPoints}��{rightPoints}���߳�{width},{height}");
            // ���������ߵĺϷ�����֮��
            return topPoints + rightPoints + bottomPoints + leftPoints;
        }
        public static int GetPointsNumOnEdge(float edgeLength, int pOffset)
        {
            // ÿ����ļ���� 50 ��λ
            // float interval = 50;

            // �Ϸ����� = �߳��� / �������������
            int points = Mathf.FloorToInt(edgeLength / pOffset);
            return points;
        }

        // ���ؾ��� pos ����ĵ�����
        public static int FindNearestPointIndex(Vector2[] points, Vector2 pos)
        {
            if (points == null || points.Length == 0)
            {
                Debug.LogError("points ����Ϊ�ջ�δ��ʼ��");
                return -1; // ���� -1 ��ʾ��Ч
            }

            int nearestIndex = 0; // ���������
            float minDistance = Vector2.Distance(points[0], pos); // ��С����

            // ���� points ����
            for (int i = 1; i < points.Length; i++)
            {
                float distance = Vector2.Distance(points[i], pos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestIndex = i;
                }
            }
            Debug.LogWarning($"{pos}����ĵ���{points[nearestIndex]}");
            return nearestIndex;
        }

        //����һ��id���������id��Ӧ�ıߺʹ˱��ϵ�id
        public static EdgeIndexPair GetLogicRectOffsetByIndex(int id,int dOffset,RectTransform rect,RectTransform.Edge edge=RectTransform.Edge.Top)
        {
            id = id % GetLegalPointNum(rect, dOffset);
            // ����Ŀ��λ�õ�ƫ����
            float width = rect.rect.width;
            float height = rect.rect.height;
            // ����ߵĳ���
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
                offset = id * dOffset; // ��һ������id
                edge = GetNextEdge(edge); // ˳ʱ����ת����һ����

                // ���¼����±ߵĳ���
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

