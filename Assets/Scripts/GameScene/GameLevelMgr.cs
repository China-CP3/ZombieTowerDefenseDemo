using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr
{
    //打开游戏场景时 动态创建玩家
    //判断游戏是否结束
    //初始化主塔的血量
    private static GameLevelMgr instance=new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    public PlayerObejct playerObejct;
    private List<MonsterPoint> monsterPoints=new List<MonsterPoint>();
    private int nowWaveNum = 0;//当前还有多少波怪物
    private int maxWaveNum=0;//总共多少波怪物
    //public int nowMonsterSavedNum=0;//当前场景中活着的怪物数量
    private List<MonsterObj> monstersList = new List<MonsterObj>();//场景中的怪物们
    private GameLevelMgr()
    {

    }
    /// <summary>
    /// 初始化角色信息
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(SceneInfo info)
    {
        UiMgr.Instance.ShowPanel<GamePanel>();
        RoleInfo roleInfo = GameDataMgr.Instance.nowRoleChoose;//拿到角色信息
        Transform heroBornPos = GameObject.Find("HeroBornPos").transform;
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),heroBornPos.position,heroBornPos.rotation);
        playerObejct= heroObj.GetComponent<PlayerObejct>();//拿到角色身上的playerobj脚本 然后初始化
        playerObejct.InitPlayerInfo(roleInfo.atk, info.money);//从角色信息拿到攻击力，从场景信息拿到当前场景的初始金币
        
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        //初始化主塔的血量
        MainTowerObj.Instance.UpdateHp(info.towerHp);
    }
    //判断游戏是否结束 
    //通过判断场景中是否全部怪物创建完毕 并且场景中已创建的怪物全部死亡


    public void AddMonsterPonit(MonsterPoint Point)
    {
        
        monsterPoints.Add(Point);
    }
    /// <summary>
    /// 更新一共有多少波怪
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMaxWaveNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        UiMgr.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum,maxWaveNum);
    }
    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        UiMgr.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }
    /// <summary>
    /// 判断所有出怪点的剩下波数和当前还需创建怪物个数是否为0
    /// </summary>
    /// <returns></returns>
    public bool CheckOverAll()
    {
        for (int i = 0; i < monsterPoints.Count; i++)
        {
            if(!monsterPoints[i].CheckOver())
            {
                return false;
            }
        }
        if(monstersList.Count>0)
        {
            return false;
        }
        return true;
    }
    ///// <summary>
    ///// 怪物出生点每创建1个怪物  场景中活着的怪物数量就+1
    ///// </summary>
    ///// <param name="num"></param>
    //public void ChangeMonsterSavedNum(int num)
    //{
    //    nowMonsterSavedNum += num;
    //}
    public void AddMonsterToMonsterList(MonsterObj obj)
    {
        monstersList.Add(obj);
    }
    public void RemoveMonsterFromMonsterList(MonsterObj obj)
    {
        monstersList.Remove(obj);
    }
    
    public MonsterObj FindMonster(Vector3 pos,int range)
    {
        for (int i = 0; i < monstersList.Count; i++)
        {
            if (monstersList[i] .isDead==false&& Vector3.Distance(pos, monstersList[i].transform.position) <= range)
            {
                return monstersList[i];
            }
        }
        return null;
    }
    public List<MonsterObj> FindMonstersAll(Vector3 pos, int range)
    {
        List<MonsterObj> list=new List<MonsterObj>();
        for (int i = 0; i < monstersList.Count; i++)
        {
            if(monstersList[i].isDead==false&&Vector3.Distance(pos,monstersList[i].transform.position)<=range)
            {
                list.Add(monstersList[i]);
            }
        }
        return list;
    }
    /// <summary>
    /// 清空当前关卡记录的数据  避免影响下次进入该关卡
    /// </summary>
    public void ClearInfo()
    {
        monsterPoints.Clear();
        monstersList.Clear();
        nowWaveNum =0;
        playerObejct = null;
    }
}
