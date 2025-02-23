using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeKnight : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public Transform player; // 玩家对象的Transform组件
    public float cooldown;//发射子弹间隔时间
    private float cooldownTimer;//记录冷却时间
    public GameObject bullet;
    private Animator animator;
    [SerializeField] private float stopDistanceX;
    [SerializeField] private float moveSpeed;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cooldownTimer = 1;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0 && player != null)
        {
            animator.Play("Attack");
        }

        if(rigidbody2D.velocity.x > 0)
        {
            gameObject.transform.rotation = new(0, 0, 0, 0);

        }
        if(rigidbody2D.velocity.x < 0)
        {
            gameObject.transform.rotation=new(0,180,0,0);   
        }

        if (player == null)
        {
            return;
        }
        else
        {
            float directionX = player.position.x - transform.position.x;
            if (Mathf.Abs(directionX) > stopDistanceX)
            {
                float moveDirection = directionX > 0 ? 1 : -1;
                rigidbody2D.velocity = new Vector2(moveDirection * moveSpeed, rigidbody2D.velocity.y);
            }
            else
            {
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }
        }
    }

    private void Attack()
    {
        GameObject axe = Instantiate(bullet, transform.position,Quaternion.identity);
        cooldownTimer = cooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
