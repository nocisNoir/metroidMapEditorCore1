using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPattle : PattleBase<Color>
{
    public List<Color> colors;
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
        if (b.transform.GetSiblingIndex() < colors.Count)
        {
            b.GetComponent<Image>().color = colors[b.transform.GetSiblingIndex()];
            Debug.LogWarning("按钮" + b.transform.GetSiblingIndex() + "颜色" + colors[b.transform.GetSiblingIndex()]);
        }
        else
        {
            Debug.LogWarning("按钮" + b.transform.GetSiblingIndex() + "超出数据范围，无法设定颜色");
        }
        base.ButtonInitialize(b);
    }
    public override T getButtonObj<T>(int i)
    {
        if (typeof(T) == typeof(Color)&&i<colors.Count)
        {
            return (T)(object)colors[i];
        }
        return base.getButtonObj<T>(i);
    }

    public static string ColorToHex(Color color)
    {
        // 将 RGB 转换为 0-255 的整数
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);

        // 格式化为十六进制字符串
        return string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
    }
    public override void setPattleButtonID_byClick(Button button)
    {
        base.setPattleButtonID_byClick(button);
        if (nowClickID < colors.Count)
        {
            Debug.LogError("按了" + nowClickID + "按钮" + $"<color={ColorToHex(colors[nowClickID])}>颜色{colors[nowClickID]}</color>");
        }
    }

}
