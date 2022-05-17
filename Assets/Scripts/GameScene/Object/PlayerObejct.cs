using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObejct : MonoBehaviour
{
    //初始化玩家属性
    //站立和下蹲的切换
    //攻击动作不同的处理 刀和枪
    //金钱变化
    public Transform gunPoint;//枪的射击点
    public int atk;
    public int money;
    private float roundSpeed = 100f;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("HSpeed",Input.GetAxis("Horizontal"));
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));

        if(Input.GetKeyDown(KeyCode.F))
            animator.SetTrigger("RollTrigger");
        if (Input.GetKeyDown(KeyCode.Mouse0))
            animator.SetTrigger("FireTrigger");

        this.transform.Rotate(Vector3.up,Input.GetAxis("Mouse X")*roundSpeed*Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1,1);
        }else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 0);
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //    Time.timeScale = 0f;
        //    UiMgr.Instance.ShowPanel<GamePanel>().btnSetting.gameObject.SetActive(true);
        //}
    }
    /// <summary>
    /// 初始化玩家属性
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="money"></param>
    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;
        UpdateMoney();
    }/// <summary>
     /// 刀的伤害检测
     /// </summary>
    public void KnifeEvent()
    {
       GameDataMgr.Instance.PlaySound("Music/Knife");
       Collider[] colliders= Physics.OverlapSphere(this.transform.position+this.transform.forward+transform.up,1,1<<LayerMask.NameToLayer("Monster"));
       for(int i=0;i<colliders.Length;i++)
       {
            //让僵尸受伤
          MonsterObj monster= colliders[i].gameObject.GetComponent<MonsterObj>();
          if(monster!=null&&!monster.isDead)
          {
             monster.Wound(atk);
             break;
          }
       }
    }
    public void ShootEvent()
    {
       GameDataMgr.Instance.PlaySound("Music/Gun");
       RaycastHit[] hits = Physics.RaycastAll(new Ray(gunPoint.position, this.transform.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));
       for (int i = 0; i < hits.Length; i++)
       {
            //让僵尸受伤 攻击特效
            MonsterObj monster = hits[i].collider.gameObject.GetComponent<MonsterObj>();
            if (monster != null&&!monster.isDead)
            {
                monster.Wound(atk);
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowRoleChoose.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj,3);
                break;
            }
        }
    }
    public void UpdateMoney()
    {
        UiMgr.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }
    public void AddMoney(int n)
    {
        money += n;
        UpdateMoney();
    }
}
