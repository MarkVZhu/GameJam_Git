using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //将脚本设置为**单例模式**，使其他脚本能够方便的访问此处的变量
    public static CameraControl instance;

    public float speed;

    public Transform target;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //将摄像机根据设定的速度移动到target位置
        if (target != null)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), speed * Time.deltaTime);
    }

    public void ChangeTarget(Transform newTarget)//更新target(在Room脚本中调用)
    {
        target = newTarget;
    }
}

