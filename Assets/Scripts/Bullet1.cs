using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    [SerializeField] private float moveSpeed;
    public float angle;
    private float destroyTimer=10;//×Ô»ÙÊ±¼ä
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity =Quaternion.Euler(0,0,angle)* new Vector2(moveSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer-= Time.deltaTime;
        if ( destroyTimer < 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")|| collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
