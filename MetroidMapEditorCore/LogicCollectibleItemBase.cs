using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LogicCollectibleItemBase : MonoBehaviour
{
    public static List<LogicCollectibleItemBase> _AllCollectibleItem;
    //收集品
    [Header("收集品属性")]
    [SerializeField] public int _ID;
    [SerializeField] public string _CollectionItemName;
    [SerializeField] public string _CollectionItemInfo;//描述
    [SerializeField] public Sprite _CollectionItemIcon;//icon

    [Header("收集品获取&使用数据")]
    [SerializeField] protected List<bool> _GainInfo;//是否已获得此能力

    [SerializeField] protected bool _CanBeUsed;//是否是能够被消耗掉的收集品
    [SerializeField] protected int _NowHaveNum;//目前持有的数量，即gainInfo中true的数量-已被使用的数量
    [SerializeField] protected int _BeUsedNum;//已被使用的数量
    [SerializeField] protected int _MaxNum;//总数

    // Start is called before the first frame update
    void Start()
    {
        if (_AllCollectibleItem==null)
        {
            _AllCollectibleItem = new List<LogicCollectibleItemBase>();
        }
        if (!_AllCollectibleItem.Contains(this))
            _AllCollectibleItem.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void initCollectionItem()
    {
        if (_GainInfo == null)
        {
            _GainInfo = new List<bool>();
            // 使用 Enumerable.Repeat 生成指定数量的 true
            IEnumerable<bool> trueValues = Enumerable.Repeat(false, _MaxNum);

            // 将生成的 true 值添加到列表中
            _GainInfo.AddRange(trueValues);

        }
    }

    public void GainCollectionItem(int id)
    {
        if(id>_GainInfo.Count)
        {
            Debug.LogError($"收集品{_CollectionItemName}超出范围");
            return;
        }    
        if (!_GainInfo[id])
        {
            _GainInfo[id] = true;
            _NowHaveNum++;
            Debug.Log($"获取收集品{_CollectionItemName}的第{id}个");
        }
        else
        {
            Debug.LogWarning($"收集品{_CollectionItemName}的第{id}个已获取");
        }
    }
    public bool UseCollectionItem(int num = 1)
    {
        if (num > _NowHaveNum)
        {
            Debug.LogWarning($"收集品{_CollectionItemName}需求超出持有数量，无法使用");
            return false;
        }
        else
        {
            _NowHaveNum -= num;
            _BeUsedNum += num;
            return true;
        }
    }
    public bool CheckCollectionItemNum(int num)
    {
        if (num > _NowHaveNum)
        {
            Debug.LogWarning($"收集品{_CollectionItemName}需求超出持有数量，检测失败");
            return false;
        }
        else
            return true;
    }

}
