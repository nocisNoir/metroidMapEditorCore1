using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidMapEditorCore
{
    public class DoorInspector : MonoBehaviour
    {
        public static DoorInspector current;
        public DoorBase _NowSelectDoor;
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
            //¹¦ÄÜ°´Å¥
        }
    }

}
