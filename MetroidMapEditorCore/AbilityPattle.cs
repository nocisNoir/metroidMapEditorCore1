using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidMapEditorCore
{
    public class AbilityPattle : PattleBase<AbilityBase>
    {
        public AbilityInspector mainAbilityInspector;
        public static AbilityPattle main;
        
        //绑定能力的pattle
        void Start()
        {
            if (!main)
                main = this;
            DefaultGetButtons();
            if (defaultHide)
                CallThisPattle(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void addData()
        {
            AbilityBase ability = Instantiate(SampleUIObjs.main.sampleAbility);
            data.Add(ability);
            base.addData();
        }
        public override void ButtonInitialize(Button b)
        {
            base.ButtonInitialize(b);
        }
        public void refreshIcons()
        {
            for (int i= 0; i < data.Count; i++)
            {
                if (data[i]._IconSpr && data[i]._IconColor != default)
                {
                    _PattleButtons[i].image.sprite = data[i]._IconSpr;
                    _PattleButtons[i].image.color = data[i]._IconColor;

                }

            }
        }
    }

}
