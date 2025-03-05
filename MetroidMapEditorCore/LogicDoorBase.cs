using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicDoorBase : MonoBehaviour
{

    [Header("��Ϸ����ʱ����")]
    [SerializeField] public bool _IsOpenAllow;//�Ƿ������������
    [SerializeField] public bool _IsUsed_Enter;//�Ƿ��Ѿ���������
    [SerializeField] public bool _IsUsed_Out;//�Ƿ��Ѿ�ͨ�����ų�����
    [Header("�������")]
    [SerializeField] LogicAbilityBase[] _AbilitysAll;//��Ҫ����
    [SerializeField] LogicAbilityBase[] _AbilitysAny;//��Ҫ����һ��
    public OneSideDoorType _SideType;//����������

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum OneSideDoorType//����������
{
    TwoSide, OutOnly, EnterOnly, EnterOnce
    ///����˫���ţ����ɳ�������ţ����ɽ�������ţ�����֮��ת˫�����
}

public enum DoorCollectionRequreType//�����ռ�Ʒ�ж�����
{
    NumCheck, NumCheckSpendMulti, NumCheckSpendOnce
    ///���������ɣ���������ÿ�γ�����Ҫ���ѣ��������ҵ�һ����Ҫ����
}
