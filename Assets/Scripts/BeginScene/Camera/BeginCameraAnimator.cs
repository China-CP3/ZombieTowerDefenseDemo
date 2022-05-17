using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeginCameraAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityAction overAction;//用于记录动画播放完后 执行某个事
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnLeft(UnityAction unityAction)
    {
        animator.SetTrigger("Left");
        overAction=unityAction;
    }
    public void TurnRight(UnityAction unityAction)
    {
        animator.SetTrigger("Right");
        overAction = unityAction;
    }
    public void PlayAnimaOver()
    {
        overAction?.Invoke();
        overAction = null;
    }
}
