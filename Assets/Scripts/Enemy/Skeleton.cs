using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackRadius;
    [SerializeField] private bool faceRight;

    



    public Transform attackCheck;
    public float ATK;

    private Animator animator;


    private Rigidbody2D rigidbody2D;

    public Transform playerPosition;
    private Player _player;


    public float stopDistanceX;//敌人移动到距主角一定距离停下
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == null)
        {
            return;
        }
        else
        {
            float directionX = playerPosition.position.x - transform.position.x;
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
        attackTimer -= Time.deltaTime;
        AttackPlayer();
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }

    private void AttackAnim()
    {
        animator.Play("SkeletonAttack");
    }


    private void AttackPlayer()
    {
        if (attackTimer < 0)
        {
            AttackAnim();
            attackTimer = attackTime;
        }
    }

    private void Attack()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, LayerMask.GetMask("Player"));

        foreach (var hit in player)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().GetDamage(ATK);
            }
        }
    }

    private void AttackEnd()
    {
        animator.Play("SkeletonMove");
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


