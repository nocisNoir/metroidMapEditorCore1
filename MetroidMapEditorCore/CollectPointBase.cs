using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidMapEditorCore
{
    public class CollectPointBase : MonoBehaviour//收集点位
    {
        public AbilityBase _ability;
        public CollectibleItemBase _item;
        public int _itemIndex;
        public RoomBase _AttachRoom;
        [SerializeField] public bool isCollected;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void getCollected()
        {
            if (!isCollected)
            {
                if (_item)
                {
                    _item.GainCollectionItem(_itemIndex);
                }
                if (_ability)
                {
                    _ability.GainAbility();
                }
                isCollected = true;
            }
        }
    }
}

