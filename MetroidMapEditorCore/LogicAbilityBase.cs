using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class LogicAbilityBase : MonoBehaviour
    {
        [Header("��������")]
        [SerializeField]protected int _ID;
        [SerializeField]protected string _AbilityName;

        [Header("������ȡ&ʹ������")]
        [SerializeField]protected bool _isGain;//�Ƿ��ѻ�ô�����
        [SerializeField]protected bool _isUseAbilityState;//��԰����л�״̬����������¼�Ƿ�������״̬
    [Header("�׶�����������")]
    [SerializeField] protected int _maxAbilityStateNum;
    [SerializeField] protected int _nowAbilityState;


        //��ȡ��Ϣ
        public int getID()
        {
            return _ID;

        }
        public string getName()
        {
            return _AbilityName;
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


        public void AddAbilityGainPoint()
        {
            Debug.LogError("���������ȡλ�õ�");
        }

        public void SwitchState(int aimState)
        {



               // _isUseAbilityState = !_isUseAbilityState;
            
        }

        void effectByStateAbility()
        {

        }

        void effectByAbilityClick()
        {

        }
        public virtual bool AbilityCheck()
        {
            if (!_isGain)
                return false;
            return true;
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
