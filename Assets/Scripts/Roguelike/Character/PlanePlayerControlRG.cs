using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePlayerControlRG : MonoBehaviour
{
    Rigidbody2D rig;
    Animator anim;

    Vector2 movement;//��������ת��ı���

    public float speed;

    [SerializeField] private GameObject mapCanvas;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        GetInput();

        //�л�����
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        rig.MovePosition(rig.position + movement * speed * Time.fixedDeltaTime);
        //ע��������rig.positionҲ����ʹ��transform.position
    }

    void GetInput()
    {
        //�����������Ϊ����ת�����
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //�л�����
        if (movement.x != 0)
        {
            transform.localScale = new Vector3(movement.x, 1, 1);
        }

        if (Input.GetButtonDown("Map") && mapCanvas)
        {
            mapCanvas.SetActive(!mapCanvas.activeInHierarchy);
        }
    }

    void SwitchAnim()
    {
        anim.SetFloat("speed", movement.magnitude);
    }
}
