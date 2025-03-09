using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace MetroidMapEditorCore
{
    public class DoorInspector : MonoBehaviour
    {
        
        public static DoorInspector current;
        public DoorBase _NowSelectDoor;
        public DoorBase _NowSelectWayDoor_Temp;
        public Button _CloseButton;
        public Button _DestoryDoorButton;
        public Button _SelectWayDoorButton;

        public TextMeshProUGUI _IdText;
        public TextMeshProUGUI _EdgeText;
        private void Awake()
        {
            if (!current)
                current = this;
            gameObject.SetActive(false);
        }
        // Start is called before the first frame update
        void Start()
        {
            if (RoomInspector.current)
            {
                if(current==this)
                    RoomInspector.current.doorInspector = this;
                
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void callDoorInspector(DoorBase door)
        {
           // refreshRoomOutline();
            gameObject.SetActive(true);
            _NowSelectDoor= door;
            _IdText.text = door.getEip().Id.ToString();
            _EdgeText.text = door.getEip().Edge.ToString();
//            _RoomNameInputField.fontAsset = aimFont;
//            _RoomNameInputField.text = "";
//            RoomNameArea.text = room.name;
//            RoomNameArea.font = aimFont;
//            _RoomSizeX.text = room._RoomSize.x.ToString();
//            _RoomSizeY.text = room._RoomSize.y.ToString();
//            _RoomGridOffset.text = room._RoomGridOffset.ToString();
//            refreshRoomOutline(true);
            initButtons();
        }
        public void initButtons()
        {
            //功能按钮
            if (_CloseButton)
                _CloseButton.onClick.AddListener(() => hideDoorInspector());
            if (_DestoryDoorButton)
                _DestoryDoorButton.onClick.AddListener(() => destoryDoor());
            if (_SelectWayDoorButton)
                _SelectWayDoorButton.onClick.AddListener(() => selectWayDoor());
        }
        void selectWayDoor()
        {
            Debug.Log(Time.time + "进入选门状态");
            foreach(DoorBase door in FindObjectsOfType<DoorBase>())
            {
                door.setSelectWayDoorState(true);
            }
            Debug.Log(Time.time + "进入选门状态完成");
            StartCoroutine(GetSelectWayDoorInput());

        }
        IEnumerator GetSelectWayDoorInput()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
            //左键输入
            yield return null;
            yield return new WaitForSeconds(.5f);
            if (_NowSelectWayDoor_Temp)
            {
                //接收到门输入
                createWay(_NowSelectDoor, _NowSelectWayDoor_Temp);
                _NowSelectWayDoor_Temp = null;
            }
            Debug.Log(Time.time + "退出选门状态");
            foreach (DoorBase door in DoorBase.allDoor)//<DoorBase>())
            {
                door.setSelectWayDoorState(false);
            }
            Debug.Log(Time.time + "退出选门状态完成");


        }
        public void destoryDoor()
        {
            if (_NowSelectDoor)
            {
                _NowSelectDoor.DestroyDoor();
                _NowSelectDoor = null;
                Debug.LogWarning("删除选中的门");
            }
        }

        public void createWay(DoorBase startDoor,DoorBase endDoor)
        {

            if (SampleUIObjs.main.sampleWay)
            {
                Debug.Log("创建路线" + startDoor + "至" + endDoor);

                WayBase newWay = Instantiate(SampleUIObjs.main.sampleWay);
                newWay.attachDoors = new DoorBase[2] { startDoor, endDoor };
                newWay.showThisWay();
            }

        }
        public void hideDoorInspector()
        {
            if(_NowSelectDoor)
            {
                _NowSelectDoor.dragController.onDragPrepare(false);
            }
            gameObject.SetActive(false);
        }

        public void getSelectWayDoor(DoorBase door)
        {
            _NowSelectWayDoor_Temp = door;
            Debug.LogError("设置选中的门" + door);
        }
    }

}
