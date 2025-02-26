using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Littlebat : MonoBehaviour
{
    public float destroyTime;
    public float verticalPeriod;
    public float upDownWidth;
    private float aF;
    private float phase;
    public float horizonSpeed;
    private Vector2 moveDirection;
    private GameObject Player;
    private Rigidbody2D rb;
    public Animator animator;
    private bool faceRight =true;

    [SerializeField] private float ATK;



    private void Start()
    {
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        float horizontalDistance = Player.transform.position.x - transform.position.x;
        moveDirection.x = horizontalDistance * horizonSpeed * Time.deltaTime;
        aF = 2 * Mathf.PI / verticalPeriod;
        phase = 0f;
        animator=GetComponent<Animator>();
    }
    private void Update()
    {
        
        move();
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0) 
        {
            Destroy(gameObject);
        }

        Die();
        Flip();
    }
    private void move()
    {
        phase += aF * Time.deltaTime;
        if(phase>2*Mathf.PI)
        {
            phase -= 2 * Mathf.PI;
        }
         
        float upDownPos = Mathf.Sin(phase) * upDownWidth*0.01f;
        transform.position = new Vector3(transform.position.x+moveDirection.x, transform.position.y + upDownPos, transform.position.z);
      

    }

    private void Flip()
    {
        if (faceRight == true && moveDirection.x < 0)
        {
            transform.Rotate(0, 180, 0);
            faceRight = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().GetDamage(ATK);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Limit"))
        {
            Destroy(gameObject);
        }
    }
    private void Die()
    {
        if (gameObject.GetComponent<EnemyHealth>().health <= 0)
            animator.Play("BatDie");
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
   
