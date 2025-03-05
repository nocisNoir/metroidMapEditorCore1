using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPattle : PattleBase<Color>
{
    public int randomColorSeed;
   // public List<Color> colors;
    // Start is called before the first frame update
    void Start()
    {
        initColorData();
        DefaultGetButtons();
        if (defaultHide)
            CallThisPattle(false);

    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public Color[] GenerateMacaronColors(int length, int seed = 0)
    {
        // 初始化随机数种子
        Random.InitState(seed);

        Color[] colors = new Color[length];

        for (int i = 0; i < length; i++)
        {
            // 随机生成色相（0 到 1）
            float hue = Random.Range(0f, 1f);
            hue = (i + hue) / length;

            // 固定饱和度和明度
            float saturation = Random.Range(0.2f, 0.6f); // 低饱和度
            float value = Random.Range(0.6f, 1f);       // 高明度

            // 将 HSV 转换为 RGB
            colors[i] = Color.HSVToRGB(hue, saturation, value);
        }

        return colors;
    }

    void initColorData()
    {
        data = new List<Color>();
        Color[] colors = GenerateMacaronColors(_PattleButtons.Count, randomColorSeed);
        foreach (Color co in colors)
        {
            data.Add(co);
        }
    }

    public override void ButtonInitialize(Button b)
    {
        if (b.transform.GetSiblingIndex() < data.Count)
        {
            b.GetComponent<Image>().color = data[b.transform.GetSiblingIndex()];
            Debug.Log("按钮" + b.transform.GetSiblingIndex() + "颜色" + data[b.transform.GetSiblingIndex()]);
        }
        else
        {
            Debug.LogWarning("按钮" + b.transform.GetSiblingIndex() + "超出数据范围，无法设定颜色");
        }
        base.ButtonInitialize(b);
    }
    public override Color GetButtonObj(int i)
    {
        return base.GetButtonObj(i);
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
        if (nowClickID < data.Count)
        {
            //这里输出一个颜色
            Debug.LogError("按了" + nowClickID + "按钮" + $"<color={ColorToHex(data[nowClickID])}>颜色{data[nowClickID]}</color>");
        }
    }

}
