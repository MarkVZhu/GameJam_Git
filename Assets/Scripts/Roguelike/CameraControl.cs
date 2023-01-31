using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //���ű�����Ϊ**����ģʽ**��ʹ�����ű��ܹ�����ķ��ʴ˴��ı���
    public static CameraControl instance;

    public float speed;

    public Transform target;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //������������趨���ٶ��ƶ���targetλ��
        if (target != null)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), speed * Time.deltaTime);
    }

    public void ChangeTarget(Transform newTarget)//����target(��Room�ű��е���)
    {
        target = newTarget;
    }
}

