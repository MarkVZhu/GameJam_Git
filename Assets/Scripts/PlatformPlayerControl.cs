using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayerControl : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float jump;

    public float speed;
    public float jumpForce;
    public float checkLenght;

    private Rigidbody2D rb;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        ModifyAnimation();
    }

    void FixedUpdate()
    {
        if(horizontal != 0)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            transform.localScale = new Vector3((horizontal / Mathf.Abs(horizontal) * 5), transform.localScale.y, transform.localScale.z);
        }

        if (CheckGround() && jump != 0)
        {
            rb.AddRelativeForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            //anim.SetBool("jump", true);
        }

        anim.SetFloat("Speed", Mathf.Abs(horizontal));   
    }

    private void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");
    }

    private void ModifyAnimation()
    {
        if(rb.velocity.y < 0)
        {
            //anim.SetBool("jump", false);
           // anim.SetBool("fall", true);
        }
        if (CheckGround())
        {
            //anim.SetBool("isGround", true);
            //anim.SetBool("jump", false); //Deleting this line dose not affect execution, but affect logic
            //anim.SetBool("fall", false);
        }
        else
        {
            //anim.SetBool("isGround", false);
        }
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up * -1, checkLenght, LayerMask.GetMask("Ground"));

        Ray2D ray = new Ray2D(transform.position, transform.up);
        Debug.DrawRay(ray.origin, ray.direction * (checkLenght) * -1, Color.red);

        Debug.Log(hit.collider != null);
        if (hit.collider != null)
            return true;

        return false;
    }
}
