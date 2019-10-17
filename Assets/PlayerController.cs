using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer sr;
    Animator a;
    Rigidbody2D rb;
    bool jump;
    bool walk;
    int jumpCount = 0;
    public float velocity = 0.1f;
    public float jumpForce = 5000f;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        walk = jump = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pos.x -= velocity;
                sr.flipX = true;
            }
            else
            {
                pos.x += velocity;
                sr.flipX = false;
            }
            transform.position = pos;
            if (!jump)
            {
                a.SetBool("Walk", true);
                walk = true;
            }
        }
        else
        {
            a.SetBool("Walk", false);
            walk = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if(jumpCount < 2)
            {
                if (walk)
                {
                    walk = false;
                    a.SetBool("Walk", false);
                }

                jumpCount++;
                a.SetBool("Jump", true);
                rb.AddForce(transform.up * jumpForce);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        a.SetBool("Jumping", false);
        jumpCount = 0;
    }
}
