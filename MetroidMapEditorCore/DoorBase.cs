using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class DoorBase : LogicDoorBase
    {
        //这是门，绑定了路线、房间、能力
        [Header("自身属性")]
        public Sprite _Icon;
        public RectTransform doorTransform;
        public Button doorButton;
        //public Transform _OffsetToRoom;
        public BaseDirection _DirecionToRoom;
        public int _IdToRoom;
        public UIObjDragController dragController;

        [Header("游戏性属性")]
        public DoorHideType _HideType;
        public bool _AllowEnterWhenHide;//隐藏时是否允许进门，对于永久隐藏的门一般为true

        [Header("绑定的房间&序号")]
        [SerializeField] public int _RoomId;
        [Header("绑定的路线")]
        [SerializeField] public int _WayId;
        [Header("房间位置相关")]
        [SerializeField] RoomBase _AttachRoom;
        [SerializeField] EdgeIndexPair _AttachRoomEdgeIndex;
        public bool InitByAttachRoom;
        [Header("开门收集品相关")]
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
                    Debug.LogError("未找到此门对应的房间？？？");
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

    public enum DoorHideType//隐藏属性
    {
        NoHide, AlwaysHide, OnceHide, HideWhileNotOpen, Others
        ///不隐藏，永远隐藏，先隐藏进门后取消隐藏，开门条件未集齐时隐藏
    }

}
