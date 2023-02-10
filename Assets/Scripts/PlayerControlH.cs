using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerControlH : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float jump;
    [SerializeField] [Range(1, 2)] private float rayHeight;

    public float speed;
    public float jumpForce;
    public float checkHeight;
    public float checkWallDistance;

    private Rigidbody2D rb;
    private Animator anim;
    private float LastDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        LastDirection = 0;
    }

    private void Start()
    {
    }

    void Update()
    {
        CheckWallRight();
        GetInput();
        ModifyAnimation();
    }

    void FixedUpdate()
    {
        if ((LastDirection > 0 && horizontal < 0) && CheckWallLeft() 
            || (LastDirection < 0 && horizontal > 0) && CheckWallRight()
            ||GetComponent<RotatePlayer>().GetRotating() 
            || !GetComponentInChildren<RootExtract>().canMove)
        {
            horizontal = 0;
        }
        
        if(horizontal != 0)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            transform.localScale = new Vector3((horizontal / Mathf.Abs(horizontal) * 5), transform.localScale.y, transform.localScale.z);
            LastDirection = horizontal;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up * -1, checkHeight, LayerMask.GetMask("Ground"));

        Ray2D ray = new Ray2D(transform.position, transform.up);
        Debug.DrawRay(ray.origin, ray.direction * (checkHeight) * -1, Color.red);

        //Debug.Log(hit.collider != null);
        if (hit.collider != null)
            return true;

        return false;
    }

    private bool CheckWallLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, rayHeight, 0), transform.right * -1 , checkWallDistance, LayerMask.GetMask("Ground"));

        Ray2D ray = new Ray2D(transform.position + new Vector3(0, rayHeight, 0), transform.right);
        Debug.DrawRay(ray.origin, ray.direction * checkWallDistance * -1, Color.green);

        //Debug.Log(hit.collider != null);
        if (hit.collider != null)
            return true;

        return false;
    }

    private bool CheckWallRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, rayHeight, 0), transform.right, checkWallDistance, LayerMask.GetMask("Ground"));

        Ray2D ray = new Ray2D(transform.position + new Vector3(0, rayHeight, 0), transform.right);
        Debug.DrawRay(ray.origin, ray.direction * checkWallDistance, Color.green);

        //Debug.Log(hit.collider != null);
        if (hit.collider != null)
            return true;

        return false;
    }
}
