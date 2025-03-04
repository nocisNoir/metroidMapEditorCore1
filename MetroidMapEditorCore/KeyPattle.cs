using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KeyPattle : PattleBase<KeyCode>
{
    public GameObject defaultKeyBoardText;
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

        base.ButtonInitialize(b);
        if (b.transform.childCount==0)//.GetComponentInChildren<TextMeshProUGUI>())
        {
            GameObject textObj = Instantiate(defaultKeyBoardText, b.transform);
        }
        TextMeshProUGUI text = b.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

        text.text = "" + ((int)data[b.transform.GetSiblingIndex()]);
        if((int)data[b.transform.GetSiblingIndex()]-(int)KeyCode.A>=0&& (int)data[b.transform.GetSiblingIndex()] - (int)KeyCode.A <= 26)
        {
            text.text = "" + (char)(((int)data[b.transform.GetSiblingIndex()] - (int)KeyCode.A) + 'A');
        }
    }
}
