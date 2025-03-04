using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MetroidMapEditorCore
{
    public class Ability : MonoBehaviour
    {
        [Header("��������")]
        [SerializeField] int _ID;
        [SerializeField] string _AbilityName;
        [SerializeField] AbilityTriggerType _TriggerType;//��������
        [SerializeField] KeyCode[] _ActiveKey;
        [SerializeField] Button[] _ActiveButtons;
        [SerializeField] bool _IsBasicMoveAbility;//�Ƿ�Ϊ�������Ҽ��Ļ����ƶ�����

        [Header("������ȡ&ʹ������")]
        [SerializeField] bool _isGain;//�Ƿ��ѻ�ô�����
        [SerializeField] bool _isUseAbilityState;//��԰����л�״̬����������¼�Ƿ�������״̬

        //��ȡ��Ϣ
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
            Debug.Log("����" + _ID + "����" + _AbilityName + "����������" + _TriggerType + "���󶨰���" + keys);
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
            Debug.LogWarning("�������������ֵ��id������");
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
            Debug.LogError("���������ȡλ�õ�");
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
            return false;
        }

    }
    public enum AbilityTriggerType//����������������
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
