using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    public int maxWave;
    public int monsterNumOneWave;//每波有多少个怪
    private int nowNum;//当前还需要创建的怪物数量
    public List<int> mosterIds;//多个id  表示随机创建的多种不同的怪物
    private int nowId;//当前波的怪物id
    public float createOneOffsetTime;//单个怪物创建的时间间隔
    public float delayTimeWave;//每波之间的时间间隔
    public float firstDelayTime;//第一波怪物之前的玩家准备时间
    private List<GameObject> monsterObjList;//提前加载好各个怪物预制体
    // Start is called before the first frame update
    void Start()
    {
        GameLevelMgr.Instance.UpdateMaxWaveNum(maxWave);//更新关卡中所有出怪点的最大波数的总和
        GameLevelMgr.Instance.AddMonsterPonit(this);//每个出怪点都把自己添加进 关卡管理器 方便那边拿到出怪点的波数之类的数据
        monsterObjList = new List<GameObject>(GameDataMgr.Instance.monsterInfoList.Count - 1);
        for (int i = 0; i < GameDataMgr.Instance.monsterInfoList.Count; i++)
        {
            monsterObjList.Add(Resources.Load<GameObject>(GameDataMgr.Instance.monsterInfoList[i].res));
            //Resources.Load<GameObject>(GameDataMgr.Instance.monsterInfoList[i].res);
        }
        Invoke("CreateWave", firstDelayTime);
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 创建一波怪物
    /// </summary>
    private void CreateWave()
    {
        nowId=mosterIds[Random.Range(0, mosterIds.Count)];//0,count-1
        nowNum = monsterNumOneWave;
        maxWave--;
        CreateMonster();
        GameLevelMgr.Instance.ChangeNowWaveNum(1);
    }
    private void CreateMonster()
    {
        MonsterInfo info =GameDataMgr.Instance.monsterInfoList[nowId-1];
        //GameObject obj = Instantiate(Resources.Load<GameObject>(info.res),this.transform.position,Quaternion.identity);
        GameObject obj = Instantiate(monsterObjList[nowId - 1], this.transform.position, Quaternion.identity);
        MonsterObj monsterObj = obj.AddComponent<MonsterObj>();
        monsterObj.InitInfo(info);
        nowNum--;
        GameLevelMgr.Instance.AddMonsterToMonsterList(monsterObj);
        if (nowNum==0)//当前波怪物创建完后  如果不是最后一波  就在间隔时间后  创建下一波
        {
            if(maxWave>0)
            {
                Invoke("CreateWave",delayTimeWave);
            }
        }
        else
        {
            Invoke("CreateMonster", createOneOffsetTime);//间隔时间后 创建下个怪物          
        }
    }
    public bool CheckOver()
    {
        return maxWave==0&&nowNum==00;//波数为0和当前场景还需创建的怪物个数为0   说明怪物全部创建完了
    }
}
