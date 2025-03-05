using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PattleBase<T> : MonoBehaviour
{
    public bool defaultHide;
    public Transform _PattleButtonParent;
    public List<Button> _PattleButtons;
    public int nowClickID;
    public List<T> data;
    public Button _CloseButton;
    public event OnClickCallBack onClickCallBackEvent;
    public delegate void OnClickCallBack();

    private void Start()
    {
        DefaultGetButtons();
    }

   public void DefaultGetButtons()
    {
        if (_CloseButton)
            _CloseButton.onClick.AddListener(() => CallThisPattle(false));
        foreach(Button bt in  _PattleButtonParent.GetComponentsInChildren<Button>())
        {
            if(!_PattleButtons.Contains(bt))
                _PattleButtons.Add(bt);
            ButtonInitialize(bt);
        }
    }
    public virtual void  ButtonInitialize(Button b)
    {
        b.onClick.AddListener(() => setPattleButtonID_byClick(b));
        b.onClick.AddListener(() => onClickCallBackEvent?.Invoke());
    }


    public virtual bool GetDataOnClick(out T dataOut)
    {
        dataOut = data[nowClickID];
        return true;
    }
    public virtual void setPattleButtonID_byClick(Button button)
    {
        nowClickID = getPattleButtonId(button);
        //很重要的地方，需要加一个回掉？
        Debug.Log("时间" + Time.fixedTime + "按了按钮" + nowClickID);
    }
    public int getPattleButtonId(Button button)
    {

        return button.transform.GetSiblingIndex();
    }
    public T GetNowClickData()
    {
        return data[nowClickID];
    }
    public virtual T GetButtonObj(int i)
    {
        
        return default;
    }
    public void CallThisPattle(bool ifCall=true)
    {
        gameObject.SetActive(ifCall);
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
            Debug.LogError("未找到数据" + typeof(T));
            return default(T);
        }
    }
}

public interface IData<T> // 泛型接口，T 是数据的类型
{
#pragma warning disable CS0693 // 类型参数与外部类型中的类型参数同名
    public T GetData(); // 获取数据的方法
#pragma warning restore CS0693 // 类型参数与外部类型中的类型参数同名
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



