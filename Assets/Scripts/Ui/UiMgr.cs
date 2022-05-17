using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMgr
{
    private static UiMgr instance=new UiMgr();
    public static UiMgr Instance => instance;
    //存储显示中的ui面板 每次打开某个面板 就加入这个字典
    //关闭面板时 直接获取字典中的某个面板进行关闭
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private GameObject canvas;//作为面板的父对象
    private UiMgr()
    {
        canvas = GameObject.Instantiate(Resources.Load<GameObject>("Ui/Canvas"));
        GameObject.DontDestroyOnLoad(canvas);//切换场景时不移除
    }
    public T ShowPanel<T>() where T:BasePanel
    {
        //保证T的名字和面板预制体的名字一样 这样就可以通过T的名字得到对应的预制体
        string panelName = typeof(T).Name;
        if(panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;//父类转子类 加了约束basepanel  那么一定能够转换  因为都是basepanel或者其子类
        }
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("Ui/"+panelName));
        panelObj.transform.SetParent(canvas.transform, false);//新打开的ui面板设置为canvas的子物体 false表示世界坐标的保持
        T panel= panelObj.GetComponent<T>();//拿到面板预制体身上的脚本 等会关闭面板的时候 可以通过这个脚本删除依附的gameobject 就可以关闭面板了
        panelDic.Add(panelName,panel);//把已经打开的面板添加到字典里  每次打开前先判断字典里有没有 是否已打开
        panel.ShowMe();//面板打开时的逻辑  在面板基类里   uimgr只负责打开关闭面板 
        return panel;
    }
    public void HidePanel<T>(bool isFade=true) where T:BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)//是否需要淡入淡出 默认需要
            {
                panelDic[panelName].HideMe(() =>
                {
                    GameObject.Destroy(panelDic[panelName].gameObject);//脚本依附的对象 也就是面板
                    panelDic.Remove(panelName);
                });
            }
            else
            {  
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }
           
        }
    }
    public T GetPanel<T>() where T:BasePanel
    {
        string panelName= typeof(T).Name;
        if(panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }
}
