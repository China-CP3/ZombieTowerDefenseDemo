using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;//摄像机看向的目标
    public Vector3 offsetPos;//摄像机偏移位置
    public float bodyHeight;//看向位置的y偏移
    //摄像机的移动和旋转速度
    public float moveSpeed;
    public float rotationSpeed;
    private Vector3 targetPos;
    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null)
        {
            return;
        }
        targetPos = target.position+target.forward*offsetPos.z;//Vector3.forward 是世界坐标系  target.forward才是角色自己的坐标系
        targetPos += Vector3.up * offsetPos.y;
        targetPos += target.right * offsetPos.x;
        this.transform.position=Vector3.Lerp(this.transform.position, targetPos, moveSpeed*Time.deltaTime);//先快后慢  a+(b-a)*t
        //Vector3.Slerp(this.transform.position, targetPos, moveSpeed*Time.deltaTime); 匀速

        targetRotation = Quaternion.LookRotation((target.position+Vector3.up*bodyHeight)-this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation,rotationSpeed*Time.deltaTime);
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
