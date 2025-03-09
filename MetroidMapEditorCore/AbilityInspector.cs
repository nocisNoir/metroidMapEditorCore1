using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace MetroidMapEditorCore
{
    public class AbilityInspector : MonoBehaviour
    {
        public AbilityPattle abilityPattle;
        public Vector2 _AbilityPattleDefaultPos;
        public static AbilityInspector current;
        public AbilityBase _NowSelectAbility;
        public Button _CloseButton;
        public Button _OpenButton;
        [Header("选icon相关")]
        public Button _IconPattleOpenButton;//选icon盘的button
        public Button _IconPattleCloseButton;
        public GameObject iconPattles;
        public PattleBase<Color> abilityIconColorPattle;
        public PattleBase<Sprite> abilityIconSpritePattle;
        public Image _AbilityIconShow;
        [Header("改字")]
        public TextMeshProUGUI nameText;
        public TMP_InputField _AbilityNameInput;
        string nameTextPrevious;
        public TMP_InputField _AbilityInfoInput;

        // Start is called before the first frame update
        void Start()
        {
            initButtons();
            //initButtons();

        }

        private void Awake()
        {
            if (!current)
                current = this;
        }
        // Update is called once per frame
        void Update()
        {
        }

        void initButtons()
        {
            if (_OpenButton)
            {
                _OpenButton.onClick.AddListener(() => callAbilityInspector(true));
            }
            if(_IconPattleOpenButton)
                _IconPattleOpenButton.onClick.AddListener(() => callIconPattle(true));
            if (_IconPattleCloseButton)
                _IconPattleCloseButton.onClick.AddListener(() => callIconPattle(false));
            if (_CloseButton)
                _CloseButton.onClick.AddListener(() => callAbilityInspector(false));
            if (abilityIconColorPattle)
                abilityIconColorPattle.onClickCallBackEvent += setAbilityIconColor;
            if (abilityIconSpritePattle)
                abilityIconSpritePattle.onClickCallBackEvent += setAbilityIconColor;
            if (abilityPattle)
                abilityPattle.onClickCallBackEvent += selectAbility;
            if (_AbilityNameInput)
                _AbilityNameInput.onEndEdit.AddListener(OnAbilityNameInputChanged);

            //初始关闭
            if (abilityPattle)
                abilityPattle.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        void callIconPattle(bool ifCall=true)
        {
            iconPattles.SetActive(ifCall);
            
        }

         public  void callAbilityInspector(bool ifCall=true)
        {
            if (current)
                current.gameObject.SetActive(ifCall);
            if (abilityPattle)
                abilityPattle.gameObject.SetActive(ifCall);
            if(ifCall)
            {
                if (abilityPattle)
                {
                  //  abilityPattle.gameObject.SetActive(true);
                    abilityPattle.transform.localPosition = _AbilityPattleDefaultPos;
                }
                if (DoorInspector.current)
                    DoorInspector.current.hideDoorInspector();
                if (RoomInspector.current)
                    RoomInspector.current.HideRoomInspector();
            }
            else
            {
            }
          
        }
        public void selectAbility()//AbilityBase ability=null)
        {
            //选中能力
            //if (ability == null)
              AbilityBase  ability = abilityPattle.data[abilityPattle.nowClickID];
            _NowSelectAbility = ability;
            nameText.text = ability._AbilityName;
            _AbilityNameInput.text = ability._AbilityName;
            _AbilityIconShow.sprite = ability._IconSpr;
            _AbilityIconShow.color = ability._IconColor;
            if (iconPattles)
                iconPattles.SetActive(false);
        }
        
        void setAbilityIconColor()
        {
            Sprite spr = abilityIconSpritePattle.GetNowClickData();
            Color co = abilityIconColorPattle.GetNowClickData();
            _AbilityIconShow.sprite = spr;
            _AbilityIconShow.color = co;
            _NowSelectAbility.refreshIcon(co, spr);

            abilityPattle.refreshIcons();
        }

        private void OnAbilityNameInputChanged(string newText)
        {
            // 比较新旧文本内容
            if (newText != nameTextPrevious)
            {
                Debug.Log("输入框内容被修改，旧值: " + nameTextPrevious + "，新值: " + newText);

                // 更新旧值
                nameTextPrevious = newText;
                _NowSelectAbility.setName(nameTextPrevious);
            }
        }

    }

}
