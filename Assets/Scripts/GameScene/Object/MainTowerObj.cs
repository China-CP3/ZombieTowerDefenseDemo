using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObj : MonoBehaviour
{
    //受到伤害 更新血量
    //被僵尸获取到位置
    //
    // Start is called before the first frame update
    private int hp;
    private int maxHp;
    private bool isDead;
    private static MainTowerObj instance;
    public static MainTowerObj Instance => instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void UpdateHp(int hp,int maxHp)
    //{
    //    this.hp = hp;
    //    this.maxHp = maxHp;
    //    UiMgr.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp);
    //}
    public void UpdateHp(int hp)
    {
        this.hp = hp;
        UiMgr.Instance.GetPanel<GamePanel>().UpdateTowerHp(this.hp);
    }
    public void Wound(int value)
    {
        if(isDead)
        {
            return;
        }
        hp-=value;
        if(hp<=0)
        {
            hp = 0;
            isDead = true;
            GameOverPanel gameOverPanel=  UiMgr.Instance.ShowPanel<GameOverPanel>();

            gameOverPanel.InitInfo(5, false);//游戏失败后奖励金币
        }
        //UpdateHp(hp,maxHp);
        UpdateHp(hp);
    }
    /// <summary>
    /// 过场景的时候删除自己
    /// </summary>
    private void OnDestroy()
    {
        instance = null;
    }
}
