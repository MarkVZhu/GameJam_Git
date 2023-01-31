using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePlayerControlRG : MonoBehaviour
{
    Rigidbody2D rig;
    Animator anim;

    Vector2 movement;//控制人物转向的变量

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

        //切换动画
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        rig.MovePosition(rig.position + movement * speed * Time.fixedDeltaTime);
        //注：再这里rig.position也可以使用transform.position
    }

    void GetInput()
    {
        //获得轴输入作为人物转向控制
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //切换方向
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
