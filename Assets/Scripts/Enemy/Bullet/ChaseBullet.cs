using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBullet : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    [SerializeField] private float moveSpeed;
    public float angle;
    private float destroyTimer = 10;//×Ô»ÙÊ±¼ä
    public float ATK;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Player").transform.position, moveSpeed * Time.deltaTime);
        if (destroyTimer < 0)
        {
            Destroy(gameObject);
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
