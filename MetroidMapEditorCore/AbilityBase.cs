using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class AbilityBase : LogicAbilityBase
    {
        [SerializeField] AbilityTriggerType _TriggerType;//触发类型
        [SerializeField] KeyCode[] _ActiveKey;
        [SerializeField] Button[] _ActiveButtons;
        [SerializeField] bool _IsBasicMoveAbility;//是否为上下左右键的基础移动能力

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public AbilityTriggerType GetTriggerType()
        {
            return _TriggerType;
        }
        public KeyCode[] getKeysBlend()
        {
            return _ActiveKey;
        }
        public void setAbilityTriggerType(AbilityTriggerType att)
        {
            _TriggerType = att;
        }
        public void blendActiveKey(KeyCode[] keys)
        {
            _ActiveKey = keys;
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
        public bool abilityCheck_PlayerMotion()
        {
            bool gainAbility = AbilityCheck();
            if (gainAbility)
            {
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


            }
            return false;

        }
    }
}

