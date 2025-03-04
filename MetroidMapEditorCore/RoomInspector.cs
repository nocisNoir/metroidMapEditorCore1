using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class RoomInspector : MonoBehaviour
    {
        public RoomBase nowSelectRoom;
        public string _RoomName_Input;
        public Color _RoomColor_Input;
        public TMP_FontAsset aimFont;
        public static RoomInspector current;
        [Header("自带组件")]
        public TMP_InputField _RoomNameInputField;
        public Button[] colorButtons;
        public TextMeshProUGUI RoomNameArea;
        public Color[] colors;

        //一个按钮用来调整房间大小
        public TMP_InputField _RoomSizeX;
        public TMP_InputField _RoomSizeY;
       

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
            initializeColorButton();
            initializeSize();

        }

        // Update is called once per frame
        void Update()
        {

        }

        void initializeSize()
        {
            Debug.Log(Camera.main.pixelWidth + "," + Camera.main.pixelHeight);
            RectTransform rt = GetComponent<RectTransform>();
            int width = Mathf.Max((int)(Camera.main.pixelWidth / 3.5f), 120);
            rt.offsetMin = new Vector2(-width, 0);
        }

        void initializeColorButton()
        {
            for (int i = 0; i < colorButtons.Length; i++)
            {
                if (i < colors.Length)
                {
                    colorButtons[i].GetComponent<Image>().color = colors[i];
                    //  colorButtons[i].onClick.AddListener(setRoomColor(i));
                    //colorButtons[i].onClick.AddListener((data)=> { setRoomColor(Color)data; });
                }
                else
                {
                    Destroy(colorButtons[i].gameObject);
                }
            }
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
            refreshRoomOutline(true);
        }
        public void setRoomSize()
        {
            int x, y;
            if(int.TryParse( _RoomSizeX.text,out x)&&int.TryParse(_RoomSizeY.text,out y))
            {
                if (nowSelectRoom)
                {
                    nowSelectRoom.setRoomSize(x, y);
                    Debug.Log($"设定房间{nowSelectRoom._RoomName}的尺寸为({x},{y})");
                }
            }
        }

        void refreshRoomOutline(bool outlineState=false)
        {
            if (nowSelectRoom)
            {
                nowSelectRoom.outLineImg.enabled = outlineState;
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
            gameObject.SetActive(false);
        }

        public void setRoomColor(int colID)
        {
            if (colID >= colors.Length)
                colID = colors.Length - 1;
            setRoomColor(colors[colID]);
            Debug.Log("房间" + nowSelectRoom + "改色号" + colors[colID] + "id是" + colID);
        }
        public void setRoomColor(Color col)
        {
            // Debug.Log(gameObject.name);
            nowSelectRoom.SetColor(col);
        }
    }

}
