using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePlayerControl : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float jump;

    public float speed;

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
    }

    void FixedUpdate()
    {
        if (horizontal != 0)
        {
            rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
            transform.localScale = new Vector3((horizontal / Mathf.Abs(horizontal) * 5), transform.localScale.y, transform.localScale.z);
        }
        if (vertical != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed * Time.fixedDeltaTime);
        }

        anim.SetFloat("Speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }

    private void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        jump = Input.GetAxis("Jump");
    }
}
