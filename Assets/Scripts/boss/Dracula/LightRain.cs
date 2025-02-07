using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRain : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool isFlip=false;
    [SerializeField] private float ATK;
    private SpriteRenderer rbSprite;

    private float destroyTimer = 10;//×Ô»ÙÊ±¼ä

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2 (0,moveSpeed);
        rbSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Flip();

        destroyTimer -= Time.deltaTime;
        if (destroyTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    private void Flip()
    {
        if(rb.velocity.y<0&&isFlip==false)
        {
            rb.velocity = new Vector2 (0,-moveSpeed);
            isFlip = true;
            rbSprite.flipY=false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetDamage(ATK);
            Destroy(gameObject);
        }
    }
}
