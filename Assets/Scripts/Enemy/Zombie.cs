using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private bool faceRight;
    public float ATK;
    private Animator animator;


    private Rigidbody2D rigidbody2D;

    public Transform playerPosition;

    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float directionX = playerPosition.position.x - transform.position.x;      
            float moveDirection = directionX > 0 ? 1 : -1;
            rigidbody2D.velocity = new Vector2(moveDirection * moveSpeed, rigidbody2D.velocity.y);
        Flip();

    }

    private void Flip()
    {
        if (rigidbody2D.velocity.x > 0 && faceRight == false)
        {
            transform.Rotate(0, 180, 0);
            faceRight = true;
        }
        if (rigidbody2D.velocity.x < 0 && faceRight == true)
        {
            transform.Rotate(0, 180, 0);
            faceRight = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetDamage(ATK);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosition = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {       
            playerPosition = null;
        }
    }
}


