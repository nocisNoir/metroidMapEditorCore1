using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class AbilityBase : LogicAbilityBase
    {
        [SerializeField] AbilityTriggerType _TriggerType;//��������
        [SerializeField] KeyCode[] _ActiveKey;
        [SerializeField] Button[] _ActiveButtons;
        [SerializeField] bool _IsBasicMoveAbility;//�Ƿ�Ϊ�������Ҽ��Ļ����ƶ�����

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
            Debug.Log("����" + _ID + "����" + _AbilityName + "����������" + _TriggerType + "���󶨰���" + keys);
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
                switch (_TriggerType)//���ڵ�������������Զ����true��״̬����ס����Ҫ���
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

