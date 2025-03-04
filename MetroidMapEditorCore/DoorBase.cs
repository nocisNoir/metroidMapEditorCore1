using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class DoorBase : MonoBehaviour
    {
        //这是门，绑定了路线、房间、能力
        [Header("自身属性")]
        public Sprite _Icon;
        public RectTransform doorTransform;
        public Button doorButton;
        //public Transform _OffsetToRoom;
        public BaseDirection _DirecionToRoom;
        public int _IdToRoom;
        

        [Header("游戏性属性")]
        public DoorHideType _HideType;
        public bool _AllowEnterWhenHide;//隐藏时是否允许进门，对于永久隐藏的门一般为true

        public OneSideDoorType _SideType;//单向门种类

        [Header("收集品解锁开门属性")]//需要集齐x个i收集品，才允许开门
        public int _CollecionItemId_Requre;
        public int _CollectionItemNum_Requre;

        [Header("游戏运行时属性")]
        [SerializeField] public bool _IsOpenAllow;//是否集齐能力允许打开
        [SerializeField] public bool _IsUsed_Enter;//是否已经进过此门
        [SerializeField] public bool _IsUsed_Out;//是否已经通过此门出房间


        [Header("绑定的能力id")]
        [SerializeField] public int[] _AbilitysId;
        //能力检定最好加个 both & or 选项，确定需要任意能力或是
        [Header("绑定的房间&序号")]
        [SerializeField] public int _RoomId;
        [Header("绑定的路线")]
        [SerializeField] public int _WayId;
        [Header("房间位置相关")]
        [SerializeField] RoomBase _AttachRoom;
        [SerializeField] EdgeIndexPair _AttachRoomEdgeIndex;
        [Header("能力相关")]
        [SerializeField] Ability[] _AbilitysAll;//需要所有
        [SerializeField] Ability[] _AbilitysAny;//需要任意一个
        [Header("开门收集品相关")]
        int zhanwei;


        private void Awake()
        {
            doorTransform = GetComponent<RectTransform>();

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
            if (!doorButton)
            {
                if (!GetComponent<Button>())
                    gameObject.AddComponent<Button>();
            }
            doorButton = GetComponent<Button>();
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
                doorTransform.localPosition = _AttachRoom.GetNearestPointOnRoomEdge(doorTransform , out _AttachRoomEdgeIndex);
            }
        }



        public void callDoorInspector(DoorBase door)
        {

        }
    }

    public enum DoorHideType//隐藏属性
    {
        NoHide, AlwaysHide, OnceHide, HideWhileNotOpen, Others
        ///不隐藏，永远隐藏，先隐藏进门后取消隐藏，开门条件未集齐时隐藏
    }
    public enum OneSideDoorType//单向门属性
    {
        TwoSide, OutOnly, EnterOnly, EnterOnce
        ///正常双边门，仅可出房间的门，仅可进房间的门，进了之后转双向的门
    }

    public enum DoorCollectionRequreType//进门收集品判定属性
    {
        NumCheck, NumCheckSpendMulti, NumCheckSpendOnce
        ///数量够即可，数量够且每次出门需要花费，数量够且第一次需要花费
    }
}
