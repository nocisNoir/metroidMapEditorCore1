using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicDoorBase : MonoBehaviour
{

    [Header("游戏运行时属性")]
    [SerializeField] public bool _IsOpenAllow;//是否集齐能力允许打开
    [SerializeField] public bool _IsUsed_Enter;//是否已经进过此门
    [SerializeField] public bool _IsUsed_Out;//是否已经通过此门出房间
    [Header("能力相关")]
    [SerializeField] LogicAbilityBase[] _AbilitysAll;//需要所有
    [SerializeField] LogicAbilityBase[] _AbilitysAny;//需要任意一个
    public OneSideDoorType _SideType;//单向门种类

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum OneSideDoorType//单向门属性
{
    TwoSide, OutOnly, EnterOnly, EnterOnce
    ///正常双边门，仅可出房间的门，仅可进房间的门，进了之后转双向的门
}

public enum DoorCollectionRequreType//进门收集品判定属性
{
    NumCheck, NumCheckSpendMulti, NumCheckSpendOnce
    ///数量够即可，数量够且每次出门需要花费，数量够且第一次需要花费
}
