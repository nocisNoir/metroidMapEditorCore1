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
        // ��ʼ�����������
        Random.InitState(seed);

        Color[] colors = new Color[length];

        for (int i = 0; i < length; i++)
        {
            // �������ɫ�ࣨ0 �� 1��
            float hue = Random.Range(0f, 1f);
            hue = (i + hue) / length;

            // �̶����ͶȺ�����
            float saturation = Random.Range(0.2f, 0.6f); // �ͱ��Ͷ�
            float value = Random.Range(0.6f, 1f);       // ������

            // �� HSV ת��Ϊ RGB
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
            Debug.Log("��ť" + b.transform.GetSiblingIndex() + "��ɫ" + data[b.transform.GetSiblingIndex()]);
        }
        else
        {
            Debug.LogWarning("��ť" + b.transform.GetSiblingIndex() + "�������ݷ�Χ���޷��趨��ɫ");
        }
        base.ButtonInitialize(b);
    }
    public override Color GetButtonObj(int i)
    {
        return base.GetButtonObj(i);
    }

    public static string ColorToHex(Color color)
    {
        // �� RGB ת��Ϊ 0-255 ������
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);

        // ��ʽ��Ϊʮ�������ַ���
        return string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
    }
    public override void setPattleButtonID_byClick(Button button)
    {
        base.setPattleButtonID_byClick(button);
        if (nowClickID < data.Count)
        {
            //�������һ����ɫ
            Debug.LogError("����" + nowClickID + "��ť" + $"<color={ColorToHex(data[nowClickID])}>��ɫ{data[nowClickID]}</color>");
        }
    }

}
