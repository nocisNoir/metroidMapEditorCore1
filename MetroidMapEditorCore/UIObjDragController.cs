using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace MetroidMapEditorCore
{
    public class UIObjDragController : MonoBehaviour
    {
        bool nowAllowDrag;
        private Vector2 dragOffset;
        public float _DragSpeed;
        public EventTrigger mainDragEvent;
        public RectTransform _MainDragUIRect;
        public int _DragGridOffset;

        public bool useRoomLegalPos;//使用房间合法点
        public DoorBase _AttachDoor;//上面那个是true时才会生效，用于从门脚本里面获取门位置并记录
        public EdgeIndexPair _DoorLastEip;
        // Start is called before the first frame update
        void Start()
        {
            InitializedEventTrigger();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void onDragPrepare(bool state)
        {
            nowAllowDrag = state;

        }
        public static Vector3 gridVector(Vector3 input, int gridsize = 1)
        {
            return new Vector3((int)(input.x * (1.0f / gridsize)) * gridsize, (int)(input.y * (1.0f / gridsize)) * gridsize, input.z);

        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (!nowAllowDrag)
                return;
            dragOffset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        void InitializedEventTrigger()
        {
            if (!mainDragEvent)
            {
                if (gameObject.GetComponent<EventTrigger>())
                {
                    mainDragEvent = gameObject.GetComponent<EventTrigger>();
                }
                else
                {
                    mainDragEvent = gameObject.AddComponent<EventTrigger>();
                }
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
            if(useRoomLegalPos)
                entry.callback.AddListener((data) =>  recordEip_DragDoor((PointerEventData)data));

            mainDragEvent.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            mainDragEvent.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) => { onEndDrag((PointerEventData)data); });
            if (useRoomLegalPos)
                entry.callback.AddListener((data) => recordEip_DragDoor((PointerEventData)data));
            mainDragEvent.triggers.Add(entry);
            //松开？

        }

        public void recordEip_DragDoor(PointerEventData data)
        {
            _DoorLastEip = _AttachDoor.getEip();
        }

        public void OnDrag(PointerEventData data)
        {
            if (!nowAllowDrag)
                return;
            //        Debug.Log(Time.fixedDeltaTime+ "正在拖动房间" + name);
            // 拖动房间
            Vector2 mousePos = (Input.mousePosition) + (Vector3)dragOffset;
            // transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            transform.position = (Vector3)((Vector2)Camera.main.ScreenToWorldPoint(mousePos)) + Vector3.forward * transform.position.z;
            //这里设定的是房间的情况，使用网格化坐标
            //如果考虑到门，上一步是对的，这一步就会使用房间内合法点坐标。。。
            if (useRoomLegalPos)
            {
                if (!_AttachDoor)
                    Debug.LogError($"拖动门{gameObject.name}时，未找到对应的门？？？");
                transform.localPosition = _AttachDoor.getDoorPosInRoomLegelPos();//标准化位置，且记录eip
                //GetNearestPointOnRoomEdge(transform,);
            }
            else
                transform.localPosition = gridVector(transform.localPosition, _DragGridOffset);//new Vector3((int)(transform.position.x * 0.5f) * 2, (int)(transform.position.y * 0.5f) * 2, transform.position.z);
        }

        public void onEndDrag(PointerEventData data)
        {
            if (useRoomLegalPos&&_AttachDoor)
            {
                _AttachDoor.checkOtherDoorChangePos();
                
                Debug.LogError("执行一次门的互换换位置！！");
            }
        }
    }

}
