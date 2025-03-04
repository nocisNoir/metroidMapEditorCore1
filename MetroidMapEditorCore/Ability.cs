using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class Ability : MonoBehaviour
    {
        [Header("能力属性")]
        [SerializeField] int _ID;
        [SerializeField] string _AbilityName;
        [SerializeField] AbilityTriggerType _TriggerType;//触发类型
        [SerializeField] KeyCode[] _ActiveKey;
        [SerializeField] Button[] _ActiveButtons;
        [SerializeField] bool _IsBasicMoveAbility;//是否为上下左右键的基础移动能力

        [Header("能力获取&使用数据")]
        [SerializeField] bool _isGain;//是否已获得此能力
        [SerializeField] bool _isUseAbilityState;//针对按键切换状态的能力，记录是否在能力状态

        //获取信息
        public int getID()
        {
            return _ID;

        }
        public string getName()
        {
            return _AbilityName;
        }
        public AbilityTriggerType GetTriggerType()
        {
            return _TriggerType;
        }
        public KeyCode[] getKeysBlend()
        {
            return _ActiveKey;
        }

        public void logAbilityInfo()
        {
            string keys = "";
            foreach (KeyCode k in _ActiveKey)
            {
                keys += k.ToString() + " ";
            }
            Debug.Log("能力" + _ID + "名称" + _AbilityName + "，触发类型" + _TriggerType + "，绑定按键" + keys);
        }
        public void getAbility_PlayerMotion()
        {
            _isGain = true;
        }

        public void setName(string name)
        {
            _AbilityName = name;
        }
        public void setID(int id)
        {
            _ID = id;
            Debug.LogWarning("尽量别给能力赋值新id。。。");
        }
        public void setType(AbilityTriggerType att)
        {
            _TriggerType = att;
        }
        public void blendActiveKey(KeyCode[] keys)
        {
            _ActiveKey = keys;
        }

        public void AddAbilityGainPoint()
        {
            Debug.LogError("添加能力获取位置点");
        }

        public void SwitchState_PlayerMotion()
        {
            bool getSwitchKey = false;
            foreach (KeyCode k in _ActiveKey)
            {
                if (Input.GetKey(k))
                    getSwitchKey = true;
            }
            if (getSwitchKey)
            {
                _isUseAbilityState = !_isUseAbilityState;
            }
        }

        void effectByStateAbility()
        {

        }

        void effectByAbilityClick()
        {

        }
        public bool AbilityCheck_PlayerMotion()
        {
            if (!_isGain)
                return false;

            switch (_TriggerType)//对于单击、被动，永远返回true，状态、按住则需要检测
            {
                case AbilityTriggerType.CLICK:
                    return true;
                case AbilityTriggerType.HOLD:
                    foreach (KeyCode k in _ActiveKey)
                    {
                        if (Input.GetKey(k))
                            return true;
                    }
                    break;
                case AbilityTriggerType.SWITCHSTATE:
                    if (_isUseAbilityState)
                        return true;
                    break;
                case AbilityTriggerType.BEIDONG:
                    return true;
                default:
                    break;
            }
            return false;
        }

    }
    public enum AbilityTriggerType//能力按键触发类型
    {
        CLICK, HOLD, SWITCHSTATE, BEIDONG
    }
    public enum AbilityType
    {

    }
    public enum BaseDirection
    {
        UP, DOWN, LEFT, RIGHT, UpRight, UpLeft, DownRight, DownLeft, FRONT, BACK
    }
}
