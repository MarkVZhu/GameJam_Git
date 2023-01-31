using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //TODO: 初始化这些变量
    private float nowTime;
    private float startTime;
    Rigidbody2D rb;
    Animator anim;
    Vector2 movement;
    GameObject player;
    float attackNowTime, attackStartTime;
    float birth_x, birth_y;
    float activeDistance, detectDistance, attackDistance;

    void Update()
    {
        nowTime = Time.time;
        if (nowTime - startTime >= 1)   //每隔一秒改变一次移动方向
        {
            if (Mathf.Sqrt(Mathf.Pow(rb.transform.position.x - birth_x, 2) + Mathf.Pow(rb.transform.position.y - birth_y, 2)) <= activeDistance)
            {
                movement.x = Random.Range(-1f, 1f);
                movement.y = Random.Range(-1f, 1f);
                startTime = nowTime;
            }
            else	//怪物超出活动范围则向出生点移动
            {
                movement.x = birth_x - rb.transform.position.x;
                movement.y = birth_y - rb.transform.position.y;
                startTime = nowTime;
            }

        }

        //保证人物朝向始终面对水平移动的方向
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //玩家在探测范围内时，追踪玩家
        if (Mathf.Sqrt(Mathf.Pow(rb.transform.position.x - player.transform.position.x, 2) + Mathf.Pow(rb.transform.position.y - player.transform.position.y, 2)) <= detectDistance)
        {
            movement.x = player.transform.position.x - rb.transform.position.x;
            movement.y = player.transform.position.y - rb.transform.position.y;
        }
        //玩家在攻击范围内时，攻击玩家
        if (Mathf.Sqrt(Mathf.Pow(rb.transform.position.x - player.transform.position.x, 2) + Mathf.Pow(rb.transform.position.y - player.transform.position.y, 2)) <= attackDistance)
        {
            attackNowTime = Time.time;
            if (attackNowTime - attackStartTime > 1)
            {
                anim.SetBool("isAttacking", true);

                attackStartTime = attackNowTime;
            }
        }
    }
}
