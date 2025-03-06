using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class DoorBase : LogicDoorBase
    {
        //�����ţ�����·�ߡ����䡢����
        [Header("��������")]
        public Sprite _Icon;
        public RectTransform doorTransform;
        public Button doorButton;
        //public Transform _OffsetToRoom;
        public BaseDirection _DirecionToRoom;
        public int _IdToRoom;
        public UIObjDragController dragController;

        [Header("��Ϸ������")]
        public DoorHideType _HideType;
        public bool _AllowEnterWhenHide;//����ʱ�Ƿ�������ţ������������ص���һ��Ϊtrue

        [Header("�󶨵ķ���&���")]
        [SerializeField] public int _RoomId;
        [Header("�󶨵�·��")]
        [SerializeField] public int _WayId;
        [Header("����λ�����")]
        [SerializeField] RoomBase _AttachRoom;
        [SerializeField] EdgeIndexPair _AttachRoomEdgeIndex;
        public bool InitByAttachRoom;
        [Header("�����ռ�Ʒ���")]
        int zhanwei;

        
        private void Awake()
        {
            doorTransform = GetComponent<RectTransform>();
            if (!doorButton)
            {
                if (!GetComponent<Button>())
                    gameObject.AddComponent<Button>();
            }
            doorButton = GetComponent<Button>();
            if (!dragController)
            {
                if (!GetComponent<UIObjDragController>())
                    gameObject.AddComponent<UIObjDragController>();
                dragController = GetComponent<UIObjDragController>();
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<Image>())
            {
                GetComponent<Image>().sprite = _Icon;
            }
            initDoor();
        }

        // Update is called once per frame
        void Update()
        {

        }
        void initDoor()
        {
            doorButton.enabled = false;
            if (!_AttachRoom)
            {
                if (GetComponentInParent<RoomBase>())
                {
                    _AttachRoom = GetComponentInParent<RoomBase>();
                }
                else
                {
                    Debug.LogError("δ�ҵ����Ŷ�Ӧ�ķ��䣿����");
                }
            }

            if (_AttachRoom)
            {
                if(!InitByAttachRoom)
                    doorTransform.localPosition = _AttachRoom.GetNearestPointOnRoomEdge(doorTransform , out _AttachRoomEdgeIndex);
                else
                {
                    doorButton.enabled = true;
                }
                if(!_AttachRoom.doors.Contains(this))
                    _AttachRoom.doors.Add(this);
            }
        }

        public void setEip(EdgeIndexPair eip)
        {
            _AttachRoomEdgeIndex = eip;
        }
        public bool checkEdgeIndexPair(EdgeIndexPair eip)
        {
            return (eip.Edge == _AttachRoomEdgeIndex.Edge) && (eip.Id == _AttachRoomEdgeIndex.Id) ;
        }

        public void EnableDoorButton(bool enable = false)
        {
            doorButton.enabled = enable;
        }

        public void callDoorInspector()
        {
            DoorInspector.current.callDoorInspector(this);
        }
    }

    public enum DoorHideType//��������
    {
        NoHide, AlwaysHide, OnceHide, HideWhileNotOpen, Others
        ///�����أ���Զ���أ������ؽ��ź�ȡ�����أ���������δ����ʱ����
    }

}
