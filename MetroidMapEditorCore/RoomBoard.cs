using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using MetroidMapEditorCore;
namespace MetroidMapEditorCore
{
    public class RoomBoard : MonoBehaviour
    {
        public static RoomBoard mainRoomBoard;
        public GameObject roomPrefab; // 房间预制件
        public Transform roomsContainer; // 房间容器，用于存放所有房间
        public Button _ZoomUpButton;
        public Button _ZoomDownButton;
        public float zoomMin = 0.5f, zoomMax = 3f; // 缩放上下限
        public float zoomSpeed = 0.1f; // 缩放速度
        [SerializeField] float _ZoomRateCurrent;
        public float dragSpeed = 10f;
        private List<RoomBase> rooms = new List<RoomBase>(); // 当前所有房间的列表
        private void Start()
        {
            if (!mainRoomBoard)
                mainRoomBoard = this;
            //if(!)
            InitializeRooms();
        }

        void Update()
        {
            //HandleZoom();
            HandlePanelDrag();
        }

        void InitializeRooms()
        {
            if (roomsContainer)
            {
                foreach (RoomBase room in roomsContainer.GetComponentsInChildren<RoomBase>())
                {
                    rooms.Add(room);
                    room.ReportRoomInitialize("初始化计入房间版总控");
                }
            }
        }

        // 创建新房间
        public void CreateRoom(Vector2 position)
        {
            GameObject newRoomObj = Instantiate(roomPrefab, position, Quaternion.identity, roomsContainer);
            RoomBase newRoom = newRoomObj.GetComponent<RoomBase>();
            rooms.Add(newRoom);
            newRoom.SetColor(Random.ColorHSV()); // 随机颜色示例
        }

        // 删除房间
        public void DeleteRoom(RoomBase room)
        {
            if (room != null)
            {
                rooms.Remove(room);
                Destroy(room.gameObject);
            }
        }


        //按钮调用 勿删
        public void ZoomUp()
        {
            HandleZoom(zoomSpeed);
        }
        public void ZoomDown()
        {
            HandleZoom(-zoomSpeed);
        }
        void HandleZoom(float zoomNum)
        {
            if (zoomNum != 0)
            {
                Debug.Log("fangda");

                _ZoomRateCurrent += zoomNum;
                _ZoomRateCurrent = Mathf.Clamp(_ZoomRateCurrent, zoomMin, zoomMax);
                roomsContainer.localScale = new Vector3(_ZoomRateCurrent, _ZoomRateCurrent, 1f);
            }
        }


        public void ClickRoom(RoomBase room)
        {
            //    Debug.LogError("选中房间" + room.gameObject.name);
            foreach (RoomBase r in rooms)
            {
                r.onDragPrepare(false);
            }
            room.transform.SetSiblingIndex(roomsContainer.childCount - 1);
            room.onDragPrepare(true);
            RoomInspector.current.callRoomInspector(room);
        }

        // 面板拖拽
        void HandlePanelDrag()
        {
            if (Input.GetMouseButton(1)) // 右键拖动
            {
                Vector3 delta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0) * dragSpeed;
                roomsContainer.position += delta;
            }
        }
    }


}
