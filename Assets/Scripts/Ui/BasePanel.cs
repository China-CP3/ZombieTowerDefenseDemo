using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour//抽象类不能被new  只能继承该类来使用
{
    private CanvasGroup canvasGroup;//控制面板及其子面板的整体透明度
    private float alphaSpeed = 10f;//淡入淡出的速度
    public bool isShow=false;//判断当前是淡入还是淡出
    private UnityAction hideCallBack=null;
    //awake比start先执行 为了避免在start里找不到canvasgroup public会在其他地方被点出来 没有必要 所以用protected  
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup==null)
        {
            canvasGroup=this.gameObject.AddComponent<CanvasGroup>();//假设忘了给该对象添加组件canvasgroup 这里就来加 上面那句得到的是null  因为没添加该组件
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()//虚函数允许子类重写 也可以不重写
    {
        Init();
    }
    /// <summary>
    /// 用来让子类初始化 注册控件事件 抽象函数子类必须实现
    /// </summary>
    public abstract void Init();
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallBack = callBack;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isShow && canvasGroup.alpha < 1)//当淡入时, 如果alpha透明度<1 就一直累加 加到1位置
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        else if (!isShow && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
        ////当处于显示状态时 如果透明度 不为1  就会不停的加到1 加到1 过后 就停止变化了
        ////淡入
        //if (isShow && canvasGroup.alpha != 1)
        //{
        //    canvasGroup.alpha += alphaSpeed * Time.deltaTime;
        //    if (canvasGroup.alpha >= 1)
        //        canvasGroup.alpha = 1;
        //}
        ////淡出
        //else if (!isShow && canvasGroup.alpha != 0)
        //{
        //    canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
        //    if (canvasGroup.alpha <= 0)
        //    {
        //        canvasGroup.alpha = 0;
        //        //让面板 透明度淡出完成后 再去执行的一些逻辑
        //        hideCallBack?.Invoke();
        //    }

        //}
    }
 
}
