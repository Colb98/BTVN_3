using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer sr;
    Animator a;
    Rigidbody2D rb;
    bool jump;
    bool walk;
    int jumpCount = 0;
    int itemCount = 0;
    public float velocity = 0.1f;
    public float jumpForce = 5000f;
    public TextMeshProUGUI hpText;
    public Image hpMask;
    public Image hpBar;
    public GameObject panel;
    public int maxHp = 500;
    public float hp = 400f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        walk = jump = false;
    }

    void UpdateHorizontalPosition(Vector2 pos)
    {
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
    }

    void UpdateJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (jumpCount < 2)
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

    void UpdateHP()
    {
        hpText.SetText((int)hp + "/" + maxHp);
        hpMask.rectTransform.transform.localScale = new Vector3(hp / maxHp, 1, 1);
        if (hp < 100)
        {
            hpBar.color = new Color(1, 0, 0);
        }
        else
        {
            hpBar.color = new Color(1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        UpdateHorizontalPosition(pos);
        UpdateJump();
        UpdateHP();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        a.SetBool("Jumping", false);
        jumpCount = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "heal":
                Heal(0.5f);
                break;
            case "dmg":
                TakeDamage(2f);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "item":
                PickItem(collision.gameObject);
                break;
        }
    }

    void PickItem(GameObject gameObject)
    {
        Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        gameObject.SetActive(false);
        Vector3 pos = panel.transform.position;
        var item = new GameObject();
        item.transform.SetParent(panel.transform, false);
        item.AddComponent<Image>();
        item.GetComponent<Image>().sprite = sprite;
        pos.x += 50 + 90 * itemCount;
        item.transform.position = pos;
        item.transform.localScale = new Vector3(0.6f, 0.6f);
        itemCount++;
    }

    public void Heal(float amount)
    {
        hp += amount;
        if (hp > maxHp)
            hp = maxHp;
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp < 1)
            hp = 1;
    }
}
