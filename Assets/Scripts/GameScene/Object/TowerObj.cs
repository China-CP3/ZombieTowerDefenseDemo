using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObj : MonoBehaviour
{
    public Transform head;
    public Transform gunPoint;
    private float roundSpeed = 50f;
    private TowerInfo info;
    private MonsterObj targetObj;
    private float nowTime;//攻击间隔时间
    private Vector3 monsterPos;
    private List<MonsterObj> targetObjList;

    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }
    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(info.type==1)
        {
            //不能攻击的情况  无目标  目标已死  超出攻击距离
            if(targetObj==null|| targetObj.isDead||Vector3.Distance(transform.position,targetObj.transform.position) > info.atkRange)
            {
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position,info.atkRange);
            }
            if (targetObj == null)
                return;
            monsterPos=targetObj.transform.position;
            monsterPos.y=head.position.y;//避免炮塔看向怪物的脚底 让双方y值一样
            head.rotation = Quaternion.Slerp(head.rotation,Quaternion.LookRotation(monsterPos-head.transform.position),roundSpeed*Time.deltaTime);
            if(Vector3.Angle(head.forward, monsterPos - head.position)<20&&Time.time-nowTime>=info.offsetTime)//炮塔的forward和僵尸减去炮塔这条向量的夹角    攻击间隔时间
            {
                nowTime = Time.time;
                targetObj.Wound(info.atk);
                GameDataMgr.Instance.PlaySound("Music/Tower");
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff),gunPoint.position,gunPoint.rotation);
                Destroy(effObj,0.3f);
            }
        }
        else
        {
            targetObjList = GameLevelMgr.Instance.FindMonstersAll(this.transform.position,info.atkRange);
            if(targetObjList.Count>0&&Time.time-nowTime>=info.offsetTime)
            {
                nowTime = Time.time;
                GameDataMgr.Instance.PlaySound("Music/Gun");
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), this.transform.position, this.transform.rotation);
                Destroy(effObj, 0.3f);
                for (int i = 0; i < targetObjList.Count; i++)
                {
                    
                    targetObjList[i].Wound(info.atk);
                }
            }
        }
    }
}
