using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class PlayerControlV : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float jump;
    [SerializeField] [Range(-2, 2)] private float rayLenghtWall;
    [Range(-2, 2)] public float rayHeightGround;

    public float speed;
    public float jumpForce;
    public float checkHeight;
    public float checkWallDistance;

    private Rigidbody2D rb;
    private Animator anim;
    private float LastDirection;
    private float originRayLenghtWall;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        LastDirection = 0;
    }

    private void Start()
    {
        originRayLenghtWall = rayLenghtWall;
    }

    void Update()
    {
        CheckWall();
        GetInput();
        ModifyAnimation();
    }

    void FixedUpdate()
    {
        if ((((LastDirection > 0 && horizontal < 0) || (LastDirection < 0 && horizontal > 0)) && CheckWall()) 
            || GetComponent<RotatePlayer>().GetRotating()
            || !GetComponentInChildren<RootExtract>().canMove)
        {
            horizontal = 0;
        }
        
        if(GetComponent<RotatePlayer>().moveAfterReverseRoate && horizontal != 0)
        {
            transform.position -= new Vector3(2.255f, 0, 0);
            GetComponent<RotatePlayer>().moveAfterReverseRoate = false;
        }

        if(horizontal != 0 && !GetComponent<RotatePlayer>().moveAfterReverseRoate)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            transform.localScale = new Vector3(transform.localScale.x, (horizontal / Mathf.Abs(horizontal) * -5), transform.localScale.z);
            if (LastDirection <= 0 && horizontal > 0)
            {
                transform.position += new Vector3(2.255f, 0, 0);
                rayHeightGround = -rayHeightGround;
                rayLenghtWall = -originRayLenghtWall - checkWallDistance;
            }
            else if (LastDirection > 0 && horizontal < 0)
            {
                transform.position -= new Vector3(2.255f, 0, 0);
                rayHeightGround = -rayHeightGround;
                rayLenghtWall = originRayLenghtWall;
            }
            LastDirection = horizontal;
        }


        if (CheckGround() && jump != 0)
        {
            rb.AddRelativeForce(new Vector2(jumpForce, 0) * -1, ForceMode2D.Impulse);
            Debug.Log("Jump");
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(rayHeightGround, 0, 0), transform.right, checkHeight, LayerMask.GetMask("Ground"));

        Ray2D ray = new Ray2D(transform.position + new Vector3(rayHeightGround, 0, 0), transform.right);
        Debug.DrawRay(ray.origin, ray.direction * (checkHeight), Color.red);

        //Debug.Log(hit.collider != null);
        if (hit.collider != null)
            return true;

        return false;
    }

    private bool CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(rayLenghtWall, 0, 0), transform.up, checkWallDistance, LayerMask.GetMask("Ground"));

        Ray2D ray = new Ray2D(transform.position + new Vector3(rayLenghtWall, 0, 0), transform.up);
        Debug.DrawRay(ray.origin, ray.direction * checkWallDistance, Color.green);

        //Debug.Log(hit.collider != null);
        if (hit.collider != null)
            return true;

        return false;
    }
}
