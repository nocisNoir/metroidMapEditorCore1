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
            //  Debug.LogWarning("允许拖动房间" + name);
            // 记录点击位置
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
            mainDragEvent.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            mainDragEvent.triggers.Add(entry);
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
            transform.localPosition = gridVector(transform.localPosition, _DragGridOffset);//new Vector3((int)(transform.position.x * 0.5f) * 2, (int)(transform.position.y * 0.5f) * 2, transform.position.z);
        }
    }

}
