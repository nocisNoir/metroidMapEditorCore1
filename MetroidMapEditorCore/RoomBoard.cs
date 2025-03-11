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
        public Button _AddNewRoomButton;
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
            mouseScrollZoom();
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

        void mouseScrollZoom()
        {
            // 获取鼠标滚轮输入
            Vector2 scrollDelta = Input.mouseScrollDelta;

            if (scrollDelta.y > 0)
            {
                ZoomUp();
                //Debug.Log("鼠标滚轮向上滚动");
            }
            else if (scrollDelta.y < 0)
            {
                ZoomDown();//                Debug.Log("鼠标滚轮向下滚动");
            }
        }

        // 创建新房间
        public void CreateRoom()
        {
            Vector2 position = Vector2.zero;
            // GameObject newRoomObj = 
            RoomBase newRoom = Instantiate(SampleUIObjs.main.sampleRoom);//, position, Quaternion.identity, roomsContainer);// newRoomObj.GetComponent<RoomBase>();
            rooms.Add(newRoom);

            newRoom.isSample = false;
            newRoom.gameObject.SetActive(true);

            newRoom.transform.SetParent(roomsContainer);
            newRoom.transform.localPosition = position;
            newRoom.transform.localScale = Vector3.one;
            newRoom.ReportRoomInitialize("初始化计入房间版总控");

            //newRoom.SetColor(Random.ColorHSV()); // 随机颜色示例
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
            HandleZoom(zoomSpeed,Input.mousePosition);
        }
        public void ZoomDown()
        {
            HandleZoom(-zoomSpeed, Input.mousePosition);// new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight) * 0.5f);// Input.mousePosition);
        }
        void HandleZoom(float zoomNum,Vector2 zoomCenter)
        {
            if (zoomNum != 0)
            {
                Debug.Log($"缩放中{zoomCenter}");

                // 计算缩放前的世界坐标中心点
                Vector3 worldCenterBeforeZoom = Camera.main.ScreenToWorldPoint((zoomCenter-new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)*0.5f)*_ZoomRateCurrent);

                // 更新缩放比例
                _ZoomRateCurrent += zoomNum;
                _ZoomRateCurrent = Mathf.Clamp(_ZoomRateCurrent, zoomMin, zoomMax);
                roomsContainer.localScale = new Vector3(_ZoomRateCurrent, _ZoomRateCurrent, 1f);

                // 计算缩放后的世界坐标中心点
                Vector3 worldCenterAfterZoom = Camera.main.ScreenToWorldPoint((zoomCenter - new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight) * 0.5f) *_ZoomRateCurrent);

                // 调整位置以保持缩放中心点不变
                Vector3 offset = worldCenterBeforeZoom - worldCenterAfterZoom;
                roomsContainer.position += offset;
            }
        }


        public void ClickRoom(RoomBase room)
        {
            //    Debug.LogError("选中房间" + room.gameObject.name);
            foreach (RoomBase r in rooms)
            {
                r.dragController.onDragPrepare(false);
            }
            room.transform.SetSiblingIndex(roomsContainer.childCount - 1);
            room.dragController.onDragPrepare(true);
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
