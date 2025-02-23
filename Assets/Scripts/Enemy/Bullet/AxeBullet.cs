using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBullet : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    [SerializeField] private float moveSpeed;
    private float destroyTimer = 10;//×Ô»ÙÊ±¼ä
    public float ATK;
    private GameObject player;
    private float movingDir;
    private float rotationAmount;
    public float rotateSpeed;

    [SerializeField] private float flipTimer;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        movingDir = player.transform.position.x > transform.position.x ? 1 : -1;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(moveSpeed * movingDir, 0);
    }

    private void Update()
    {
        rotationAmount = (rotateSpeed * Time.deltaTime) % 360;
        destroyTimer -= Time.deltaTime;
        if (destroyTimer < 0)
        {
            Destroy(gameObject);
        }

        flipTimer -= Time.deltaTime;
        if(flipTimer<0)
        {
            Flip();
        }

        transform.Rotate(transform.rotation.x, transform.rotation.y, rotationAmount);
    }

    private void Flip()
    {
        rigidbody2D.velocity = new Vector2(-moveSpeed * movingDir, 0);
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
