using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MetroidMapEditorCore
{
    public class RoomInspector : MonoBehaviour
    {
        public DoorInspector doorInspector;
        public RoomBase nowSelectRoom;
        public string _RoomName_Input;
        public Color _RoomColor_Input;
        public TMP_FontAsset aimFont;
        public static RoomInspector current;
        [Header("自带组件")]
        public Button _SetColorButton;
        public Button _AddDoorButton;
        public ColorPattle _colorPattle_RoomColor;
        public TMP_InputField _RoomNameInputField;
        public TextMeshProUGUI RoomNameArea;

        //一个按钮用来调整房间大小
        public TMP_InputField _RoomSizeX;
        public TMP_InputField _RoomSizeY;
        public TMP_InputField _RoomGridOffset;
       

        private void Awake()
        {
            if (!current)
            {
                current = this;
                gameObject.SetActive(false);

            }
            else
                Destroy(gameObject);

        }
        // Start is called before the first frame update
        void Start()
        {
            initializeColorPattle();
            initializeSize();

        }

        // Update is called once per frame
        void Update()
        {

        }

        void initButtons()
        {
            if (_AddDoorButton)
            {
                _AddDoorButton.onClick.RemoveAllListeners();
                if (nowSelectRoom)
                    _AddDoorButton.onClick.AddListener(() => nowSelectRoom.addNewDoor());
            }

        }
        void initializeSize()
        {
            Debug.Log(Camera.main.pixelWidth + "," + Camera.main.pixelHeight);
            RectTransform rt = GetComponent<RectTransform>();
            int width = Mathf.Max((int)(Camera.main.pixelWidth / 3.5f), 120);
            rt.offsetMin = new Vector2(-width, 0);
        }

        void initializeColorPattle()
        {
            if (_colorPattle_RoomColor)
            {
                _colorPattle_RoomColor.CallThisPattle();
                _colorPattle_RoomColor.onClickCallBackEvent += setRoomColor;
                _SetColorButton.onClick.AddListener(() => callRoomColorPattle());
            }

        }

        public void callDoorInspector(DoorBase door)
        {
            if (!doorInspector)
                Debug.LogError("未找到门检视板");




        }
        public void hideDoorInspector()
        {

        }

        //加个刷新房间外框
        public void callRoomInspector(RoomBase room)
        {
            refreshRoomOutline();
            gameObject.SetActive(true);
            nowSelectRoom = room;
            _RoomNameInputField.fontAsset = aimFont;
            _RoomNameInputField.text = "";
            RoomNameArea.text = room.name;
            RoomNameArea.font = aimFont;
            _RoomSizeX.text = room._RoomSize.x.ToString();
            _RoomSizeY.text = room._RoomSize.y.ToString();
            _RoomGridOffset.text = room._RoomGridOffset.ToString();
            refreshRoomOutline(true);
            initButtons();
        }
        public void setRoomSize()
        {
            int x, y,z;
            if(int.TryParse( _RoomSizeX.text,out x)&&int.TryParse(_RoomSizeY.text,out y))
            {
                if (int.TryParse(_RoomGridOffset.text, out z))
                {

                }
                else
                    z = 0;
                if (nowSelectRoom)
                {
                    nowSelectRoom.setRoomSize(x, y,z);
                    Debug.Log($"设定房间{nowSelectRoom._RoomName}的尺寸为({x},{y})，一格大小为{z}");
                }
            }
        }

        void refreshRoomOutline(bool outlineState=false)
        {
            if (nowSelectRoom)
            {
                nowSelectRoom.outLineImg.enabled = outlineState;
                nowSelectRoom.doors.ForEach((door) => door.doorButton.enabled = outlineState);
            }
        }
        public void ResetRoomName()
        {
            string newName = _RoomNameInputField.text;
            if (newName == "")
                return;
            nowSelectRoom.name = newName;
            nowSelectRoom.gameObject.name = newName;
            RoomNameArea.text = newName;
        }
        public void HideRoomInspector()
        {
            if(nowSelectRoom)
                refreshRoomOutline(false);
            nowSelectRoom = null;

            gameObject.SetActive(false);
        }

        public void callRoomColorPattle()
        {
            _colorPattle_RoomColor.CallThisPattle(true);
        }
        public void setRoomColor()
        {
            Color col = _colorPattle_RoomColor.GetNowClickData();

            // Debug.Log(gameObject.name);
            _SetColorButton.image.color = col;
            nowSelectRoom.SetColor(col);
        }
    }

}
