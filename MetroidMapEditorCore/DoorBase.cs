using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class DoorBase : MonoBehaviour
    {
        //�����ţ�����·�ߡ����䡢����
        [Header("��������")]
        public Sprite _Icon;
        public RectTransform doorTransform;
        public Button doorButton;
        //public Transform _OffsetToRoom;
        public BaseDirection _DirecionToRoom;
        public int _IdToRoom;
        

        [Header("��Ϸ������")]
        public DoorHideType _HideType;
        public bool _AllowEnterWhenHide;//����ʱ�Ƿ�������ţ������������ص���һ��Ϊtrue

        public OneSideDoorType _SideType;//����������

        [Header("�ռ�Ʒ������������")]//��Ҫ����x��i�ռ�Ʒ����������
        public int _CollecionItemId_Requre;
        public int _CollectionItemNum_Requre;

        [Header("��Ϸ����ʱ����")]
        [SerializeField] public bool _IsOpenAllow;//�Ƿ������������
        [SerializeField] public bool _IsUsed_Enter;//�Ƿ��Ѿ���������
        [SerializeField] public bool _IsUsed_Out;//�Ƿ��Ѿ�ͨ�����ų�����


        [Header("�󶨵�����id")]
        [SerializeField] public int[] _AbilitysId;
        //�����춨��üӸ� both & or ѡ�ȷ����Ҫ������������
        [Header("�󶨵ķ���&���")]
        [SerializeField] public int _RoomId;
        [Header("�󶨵�·��")]
        [SerializeField] public int _WayId;
        [Header("����λ�����")]
        [SerializeField] RoomBase _AttachRoom;
        [SerializeField] EdgeIndexPair _AttachRoomEdgeIndex;
        [Header("�������")]
        [SerializeField] Ability[] _AbilitysAll;//��Ҫ����
        [SerializeField] Ability[] _AbilitysAny;//��Ҫ����һ��
        [Header("�����ռ�Ʒ���")]
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
                    Debug.LogError("δ�ҵ����Ŷ�Ӧ�ķ��䣿����");
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

    public enum DoorHideType//��������
    {
        NoHide, AlwaysHide, OnceHide, HideWhileNotOpen, Others
        ///�����أ���Զ���أ������ؽ��ź�ȡ�����أ���������δ����ʱ����
    }
    public enum OneSideDoorType//����������
    {
        TwoSide, OutOnly, EnterOnly, EnterOnce
        ///����˫���ţ����ɳ�������ţ����ɽ�������ţ�����֮��ת˫�����
    }

    public enum DoorCollectionRequreType//�����ռ�Ʒ�ж�����
    {
        NumCheck, NumCheckSpendMulti, NumCheckSpendOnce
        ///���������ɣ���������ÿ�γ�����Ҫ���ѣ��������ҵ�һ����Ҫ����
    }
}
