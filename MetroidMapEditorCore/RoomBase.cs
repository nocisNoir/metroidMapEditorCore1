using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MetroidMapEditorCore
{
    public class RoomBase : MonoBehaviour
    {
        public Image outLineImg;
        public Image mainRoomImg;
        public Button mainRoomButton;
        public EventTrigger roomEvent;
        public RectTransform mainRoomRect;

        [Header("��Ϸ������")]
        public Vector2Int _RoomSize;
        public float _DragSpeed;
        bool nowAllowDrag;
        public Color _RoomColor;
        private Vector2 dragOffset;
        public string _RoomName;

        public List<DoorBase> doors;

        private void Awake()
        {
            mainRoomRect = GetComponent<RectTransform>();
            mainRoomButton = GetComponent<Button>();
            if (_RoomSize.x > 0 && _RoomSize.y > 0)
            {
                mainRoomRect.sizeDelta = 100 * _RoomSize;
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
            InitializedEventTrigger();
            initImg();
            transform.localPosition = gridVector(transform.localPosition, 50);
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
            if (nowAllowDrag)
            {

            }
        }
        public void onDragPrepare(bool state)
        {
            nowAllowDrag = state;

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
        public void setRoomSize(int x=0,int y=0)
        {
            Vector2Int aimSize = new Vector2Int(x, y);
            if (aimSize != Vector2.zero)
                _RoomSize = aimSize;
            GetComponent<RectTransform>().sizeDelta = 100 * _RoomSize;
        }

        public static Vector3 gridVector(Vector3 input,int gridsize = 1)
        {
            return new Vector3((int)(input.x * (1.0f/gridsize)) * gridsize, (int)(input.y * (1.0f/gridsize)) * gridsize, input.z);

        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (!nowAllowDrag)
                return;
            //  Debug.LogWarning("�����϶�����" + name);
            // ��¼���λ��
            dragOffset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        void InitializedEventTrigger()
        {
            if (!roomEvent)
            {
                if (gameObject.GetComponent<EventTrigger>())
                {
                    roomEvent = gameObject.GetComponent<EventTrigger>();
                }
                else
                {
                    roomEvent = gameObject.AddComponent<EventTrigger>();
                }
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
            roomEvent.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            roomEvent.triggers.Add(entry);
        }

        public void OnDrag(PointerEventData data)
        {
            if (!nowAllowDrag)
                return;
            //        Debug.Log(Time.fixedDeltaTime+ "�����϶�����" + name);
            // �϶�����
            Vector2 mousePos = (Input.mousePosition) + (Vector3)dragOffset;
           // transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            transform.position = (Vector3)((Vector2)Camera.main.ScreenToWorldPoint(mousePos)) + Vector3.forward * transform.position.z;
            transform.localPosition = gridVector(transform.localPosition, 50);//new Vector3((int)(transform.position.x * 0.5f) * 2, (int)(transform.position.y * 0.5f) * 2, transform.position.z);
        }

        public void SetColor(Color color)
        {
            mainRoomImg.color = color;
            Debug.Log("����" + name + "��ɫ��" + color);
        }

        public Vector2 GetNearestPointOnRoomEdge(RectTransform objRect,out EdgeIndexPair edgeIndex)
        {
            int maxPointNum = GetLegalPointNum(mainRoomRect, 50);
            Vector2[] points = new Vector2[maxPointNum];
            for(int i=0; i < maxPointNum; i++)
            {
                points[i] = GetPositionOnEdge(i);
            }
            int AimPointID = FindNearestPointIndex(points, objRect.localPosition);
            edgeIndex = GetLogicRectOffsetByIndex(AimPointID, 50, mainRoomRect);
            return points[AimPointID];
        }

        public Vector2 GetPositionOnEdge(RectTransform.Edge edge, int id, int dOffset = 50, RectTransform rectTransform = null)
        {
            if (!rectTransform)
                rectTransform = mainRoomRect;
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

