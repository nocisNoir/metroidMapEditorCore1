using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class DoorBase : LogicDoorBase
    {
        bool nowSelectWayDoor;
        public static List<DoorBase> allDoor=new List<DoorBase>();
        //这是门，绑定了路线、房间、能力
        [Header("自身属性")]
        public Sprite _Icon;
        public RectTransform doorTransform;
        public Button doorButton;
        public Outline doorOutLine;

        public Outline doorAltOutLine;
        //public Transform _OffsetToRoom;
      //  public BaseDirection _DirecionToRoom;
        public int _IdToRoom;
        public UIObjDragController dragController;


        [Header("游戏性属性")]
        public DoorHideType _HideType;
        public bool _AllowEnterWhenHide;//隐藏时是否允许进门，对于永久隐藏的门一般为true

        [Header("绑定的房间&序号")]
        [SerializeField] public int _RoomId;
        [Header("绑定的路线")]
        [SerializeField] public int _WayId;
        [SerializeField] public WayBase _DoorWay;
        [Header("房间位置相关")]
        [SerializeField]public RoomBase _AttachRoom;
        [SerializeField] EdgeIndexPair _AttachRoomEdgeIndex;

//        [Header("道路相关")]

        public bool InitByAttachRoom;
        [Header("开门收集品相关")]
        int zhanwei;

        
        private void Awake()
        {
            if (allDoor == null)
                allDoor = new List<DoorBase>();
            if (allDoor != null)
            {
                if (!allDoor.Contains(this))
                    allDoor.Add(this);
            }
              //  allDoor = new List<DoorBase>();
            doorTransform = GetComponent<RectTransform>();
            if (!doorButton)
            {
                if (!GetComponent<Button>())
                    gameObject.AddComponent<Button>();
            }
            doorButton = GetComponent<Button>();

 //           return;
            if (!dragController)
            {
                if (!GetComponent<UIObjDragController>())
                    gameObject.AddComponent<UIObjDragController>();
                dragController = GetComponent<UIObjDragController>();
                dragController.useRoomLegalPos = true;
                dragController._AttachDoor = this;
            }
            if (!doorOutLine)
            {
                doorOutLine = gameObject.AddComponent<Outline>();
                doorOutLine.effectColor = Color.black;
                doorOutLine.effectDistance =new Vector2(6,3);
                doorOutLine.enabled = false;
            }
            if (!doorAltOutLine)
            {
                doorAltOutLine = gameObject.AddComponent<Outline>();
                doorAltOutLine.effectColor = Color.red;
                doorAltOutLine.effectDistance = new Vector2(8 , 5);
                doorAltOutLine.enabled = false;
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
                if (!InitByAttachRoom)
                {
                    EdgeIndexPair temp;
                    doorTransform.localPosition = _AttachRoom.GetNearestPointOnRoomEdge(doorTransform, out temp);
                    Debug.Log($"房间{name}正在调整位置！！！{temp.logInfo()}");
                    
                    setEip(temp);
                }
                else
                {
                    doorButton.enabled = true;
                }
                if(!_AttachRoom.doors.Contains(this))
                    _AttachRoom.doors.Add(this);
                if (dragController)
                    dragController._DragGridOffset = _AttachRoom._RoomGridOffset;

                if (doorButton)
                {
                    doorButton.onClick.AddListener(() => callDoorInspector());
                    doorButton.onClick.AddListener(() => _AttachRoom.refreshRoomDoorsDragState(this));

                }
            }
        }
        public EdgeIndexPair getEip()
        {
            return _AttachRoomEdgeIndex;
        }
        public void checkOtherDoorChangePos()
        {
            //检测到同位置有一个其它门，将其它门塞到第一个合法点上面去。。。
            foreach(DoorBase door in _AttachRoom.doors)
            {
                if (door != this)
                {
                    if (door._AttachRoomEdgeIndex == this._AttachRoomEdgeIndex)
                    {
                       // _AttachRoom.setDoorToNewLegalPos(door);
                        _AttachRoom.setDoorToInputPos(door, dragController._DoorLastEip);
                        break;
                    }
                }
            }
        }

        public void DestroyDoor()
        {
            //需要有一个删除房间的功能？
            if (_AttachRoom)
            {
                if (_AttachRoom.doors.Contains(this))
                    _AttachRoom.doors.Remove(this);
            }
            allDoor.Remove(this);
            Destroy(gameObject);
        }
        public void RefreshDoorPos()
        {
            doorTransform.localPosition = _AttachRoom.GetPositionOnEdge(_AttachRoomEdgeIndex);
        }
        public void setEip(EdgeIndexPair eip)
        {
            _AttachRoomEdgeIndex = eip;
            refreshEdgeLastEip();
        }
        public void setEip(int newId)//,RectTransform.Edge newEdge = )
        {
            setEip(_AttachRoomEdgeIndex.Edge, newId);
        }
        public void setEip(RectTransform.Edge newEdge,int newId = -1)
        {
            Debug.Log("原eip" + _AttachRoomEdgeIndex.logInfo() + "改为" + newEdge + "index" + newId);
            if (newId != -1)
            {
                _AttachRoomEdgeIndex.Id = newId;
            }
            _AttachRoomEdgeIndex.Edge = newEdge;
            refreshEdgeLastEip();
        }
        void refreshEdgeLastEip()
        {
            if(_AttachRoomEdgeIndex.Id== _AttachRoom.GetPointNumOnEdge(_AttachRoomEdgeIndex.Edge) )
            {
                _AttachRoomEdgeIndex.Edge = RoomBase.GetNextEdge(_AttachRoomEdgeIndex.Edge);
                _AttachRoomEdgeIndex.Id = 0;// _AttachRoom.GetPointNumOnEdge(_AttachRoomEdgeIndex.Edge)+1;//GetPointsNumOnEdge(_AttachRoomEdgeIndex.Edge);
            }
        }
        public bool checkEdgeIndexPair(EdgeIndexPair eip)
        {
            return (eip.Edge == _AttachRoomEdgeIndex.Edge) && (eip.Id == _AttachRoomEdgeIndex.Id) ;
        }
        public bool checkEdgeIndexPair(RectTransform.Edge edge,int id)
        {
            return (_AttachRoomEdgeIndex.Edge == edge) && (_AttachRoomEdgeIndex.Id == id);
        }
//        public bool checkEdgeIndexPair(EdgeIndexPair eip,)
        public Vector3 getDoorPosInRoomLegelPos()
        {
            if (!_AttachRoom)
                Debug.LogError("无房间！！！");
            else
            {
                return _AttachRoom.GetNearestPointOnRoomEdge(doorTransform,out _AttachRoomEdgeIndex);
            }


            return Vector3.zero;
        }
        public void EnableDoorButton(bool enable = false)
        {
            doorButton.enabled = enable;
        }

        public void refreshDoorOutLine(bool enable = false)
        {
            doorOutLine.enabled = enable;
        }
        public void refreshDoorAltOutLine(bool enable = false)
        {
            doorAltOutLine.enabled = enable;
        }

        public void callDoorInspector()
        {
            if (nowSelectWayDoor)
            {
                Debug.Log("选门状态下按下门！！！");
                if (DoorInspector.current)
                    DoorInspector.current.getSelectWayDoor(this);
            }
            else
                DoorInspector.current.callDoorInspector(this);
        }
        public void setSelectWayDoorState(bool ifset)
        {
            nowSelectWayDoor = ifset;
            if (ifset)
            {
                Debug.Log("开启选门状态？");
                doorButton.enabled = true;
            }
            else
            {
                if (RoomInspector.current.nowSelectRoom != _AttachRoom)
                    doorButton.enabled = false;
            }

        }

    }

    public enum DoorHideType//隐藏属性
    {
        NoHide, AlwaysHide, OnceHide, HideWhileNotOpen, Others
        ///不隐藏，永远隐藏，先隐藏进门后取消隐藏，开门条件未集齐时隐藏
    }

}
