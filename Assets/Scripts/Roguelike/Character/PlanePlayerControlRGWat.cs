using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePlayerControlRGWat : MonoBehaviour
{
    Rigidbody2D rig;
    Animator anim;

    Vector2 movement;//控制人物转向的变量

    public float speed;
    public float rollAcceleration;

    [SerializeField] private GameObject mapCanvas;
    [SerializeField] private GameObject AttackRange;
    private bool isAttack;

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

        if (Input.GetButtonDown("Fire1") )
        {
            int num = (int)Random.Range(0, 3);
            switch (num)
            {
                case 0:
                    anim.SetTrigger("attack1");
                    break;
                case 1:
                    anim.SetTrigger("attack2");
                    break;
                case 2:
                    anim.SetTrigger("attack3");
                    break;
            }
        }
    }

    void SwitchAnim()
    {
        anim.SetFloat("speed", movement.magnitude);
        //AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(1);
        //isAttack = stateInfo.IsName("Player_Attack1") || stateInfo.IsName("Player_Attack2") || stateInfo.IsName("Player_Attack3");

        if (movement.magnitude > 0 && Input.GetButtonDown("Roll") && !isAttack)
        {
            anim.SetTrigger("roll");
            StartCoroutine(RollAccelerate());
        }
    }

    private IEnumerator  RollAccelerate()
    {
        speed *= rollAcceleration;
        yield return  new WaitForSeconds(0.1f);
        speed /= rollAcceleration;
    }

    private void StartAttack()
    {
        isAttack = true;
        AttackRange.SetActive(true);
        speed /= 2;
    }

    private void EndAttack()
    {
        isAttack = false;
        AttackRange.SetActive(false);
        speed *= 2;
    }
}
