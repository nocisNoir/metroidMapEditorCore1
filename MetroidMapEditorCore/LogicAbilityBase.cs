using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class LogicAbilityBase : MonoBehaviour
    {
        [Header("能力属性")]
        [SerializeField]protected int _ID;
        [SerializeField]protected string _AbilityName;

        [Header("能力获取&使用数据")]
        [SerializeField]protected bool _isGain;//是否已获得此能力
        [SerializeField]protected bool _isUseAbilityState;//针对按键切换状态的能力，记录是否在能力状态
    [Header("阶段性能力属性")]
    [SerializeField] protected int _maxAbilityStateNum;
    [SerializeField] protected int _nowAbilityState;


        //获取信息
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
            Debug.LogWarning("尽量别给能力赋值新id。。。");
        }


        public void AddAbilityGainPoint()
        {
            Debug.LogError("添加能力获取位置点");
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
