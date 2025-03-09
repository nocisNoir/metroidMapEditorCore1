using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritePattle : PattleBase<Sprite>
{
    // Start is called before the first frame update
    void Start()
    {
        DefaultGetButtons();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ButtonInitialize(Button b)
    {
        if (b.transform.GetSiblingIndex() < data.Count)
        {
            b.image.sprite = data[b.transform.GetSiblingIndex()];
        }
        else
        {
            b.image.enabled=(false);
        }
        base.ButtonInitialize(b);
    }
}

