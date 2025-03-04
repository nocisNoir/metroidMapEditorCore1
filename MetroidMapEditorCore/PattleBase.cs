using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PattleBase<T> : MonoBehaviour
{
    public List<Button> _PattleButtons;
    public int nowClickID;
    public List<T> data;
    private void Start()
    {
        DefaultGetButtons();
    }

   public void DefaultGetButtons()
    {
        foreach(Button bt in GetComponentsInChildren<Button>())
        {
            _PattleButtons.Add(bt);
            ButtonInitialize(bt);
        }
    }
    public virtual void  ButtonInitialize(Button b)
    {
        b.onClick.AddListener(() => setPattleButtonID_byClick(b));
    }
    public virtual void setPattleButtonID_byClick(Button button)
    {
        nowClickID = getPattleButtonId(button);
        Debug.LogError("ʱ��" + Time.fixedTime + "���˰�ť" + nowClickID);
    }
    public int getPattleButtonId(Button button)
    {

        return button.transform.GetSiblingIndex();
    }
    public virtual T getButtonObj<T>(int i)
    {
        
        return default;
    }

}

public class PattleData<T>
{
   public List<IData<T>> Data;
    public T getDataById(int id)
    {
        if (id < Data.Count)
        {
            return Data[id].GetData();
        }
        else
        {
            Debug.LogError("δ�ҵ�����" + typeof(T));
            return default(T);
        }
    }
}

public interface IData<T> // ���ͽӿڣ�T �����ݵ�����
{
#pragma warning disable CS0693 // ���Ͳ������ⲿ�����е����Ͳ���ͬ��
    public T GetData(); // ��ȡ���ݵķ���
#pragma warning restore CS0693 // ���Ͳ������ⲿ�����е����Ͳ���ͬ��
    void Display();
}

public class ColorData : IData<Color>
{
    public Color ColorValue;
    public ColorData(Color color)
    {
        ColorValue = color;
    }
    public Color GetData()
    {
        return ColorValue;
    }

    public void Display()
    {
        Debug.Log($"Color: {ColorValue}");
    }
}



