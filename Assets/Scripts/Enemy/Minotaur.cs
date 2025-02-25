using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackRadius;
    [SerializeField] private bool faceRight;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSet;
    [SerializeField] private Player _player;
    [SerializeField] private float attackStartDis;
    public bool isMinotaur;
    public bool isLightning;





    public Transform attackCheck;
    public float ATK;

    private Animator animator;


    private Rigidbody2D rigidbody2D;

    public Transform playerPosition;


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

        Vector2 dirToPlayer = (playerPosition.position - transform.position).normalized;
        if (dirToPlayer.x > 0 && faceRight == false)
        {
            transform.Rotate(0, 180, 0);
            faceRight = true;
        }
        if (dirToPlayer.x<0 && faceRight == true)
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
        float disToPlayer;
        if(playerPosition!=null)
        {
            disToPlayer = Vector2.Distance(playerPosition.position,transform.position);
        }
        else
        {
            disToPlayer = 100;
        }
        if (attackTimer < 0&&attackStartDis>disToPlayer)
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

        GameObject Bullet = Instantiate(bullet, bulletSet.position, Quaternion.identity);
        if(isMinotaur==true)
        {
            _player.audioController.PlaySfx(_player.audioController.minotaurAttack);
        }
        if(isLightning==true)
        {
            _player.audioController.PlaySfx(_player.audioController.lightningSkeletonAttack);
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
