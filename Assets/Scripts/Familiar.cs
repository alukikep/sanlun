using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour
{

    
   
    public float attackRange;
    public float attackInterval;//攻击间隔
    public float farmiliarBulletSpeed;
    [Header("检测范围")]
    public float detectionWidth;//检测敌人的范围
    public float detectionHeight;

    public float chaseRange;//追击范围
    public float maxRangeToPlayer;//与玩家的最远距离
    public float moveSpeed;

    private float waitTimer = 0;
    private float waitInterval = 5;//每过几秒在玩家旁边移动一下
    private float attackTimer;

    private Rigidbody2D rb;
    private GameObject player;
    private Transform targetEnemy;
  
    public GameObject familiarBulletPrefab;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        attackTimer = 0;
       
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        getCurrentEnemyTarget();
        attack();
        moveToPlayer();
        moveToEnemy();
    }
    private void getCurrentEnemyTarget()//检测最近的在Enemy层的敌人 检测范围是矩形
    {
        Vector2 topLeft = new Vector2(transform.position.x - detectionWidth, transform.position.y + detectionHeight);
        Vector2 bottomRight = new Vector2(transform.position.x + detectionWidth, transform.position.y - detectionHeight);
        Collider2D[] enemies = Physics2D.OverlapAreaAll(topLeft, bottomRight,LayerMask.GetMask("Enemy"));
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach(Collider2D enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;                
            }

        }
        if (closestEnemy != null &&closestDistance<chaseRange)
        {
            targetEnemy = closestEnemy;
        }
        else
        {
            targetEnemy = null;
        }
        

    }
    private void attack()//对最近的敌人进行攻击
    {
        float distance = Vector3.Distance(targetEnemy.transform.position, transform.position);
        if (targetEnemy!=null&&attackTimer<=0&&distance< attackRange)
        {
            
            GameObject farmiliarBullet = Instantiate(familiarBulletPrefab, transform.position, Quaternion.identity);
            Vector2 direction = (targetEnemy.position - transform.position).normalized;
            Rigidbody2D rb = farmiliarBullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * farmiliarBulletSpeed;
            attackTimer = attackInterval;
        }
        
    }
    private void moveToPlayer()//移动向玩家且在玩家旁边行动
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        Vector2 direction = (player.transform.position - transform.position);
        Vector2 horizontalDirection = new Vector2(direction.x, 0).normalized;
        if(distance>maxRangeToPlayer)
        {
            rb.velocity = horizontalDirection * moveSpeed;
        }    
        if(distance<maxRangeToPlayer&&targetEnemy==null)//暂时用于其在玩家旁移动而不是傻站着
        {

            waitTimer += Time.deltaTime;
            if(waitTimer>waitInterval)
            {
                float randomNum = Random.Range(0, 2);
                if(randomNum>1.25)
                {
                    rb.velocity = new Vector2(1,0).normalized*moveSpeed;

                }
                if(randomNum<=0.75)
                {
                    rb.velocity = new Vector2(-1, 0).normalized*moveSpeed;
                }
                if(randomNum>0.75&&randomNum<=1.26)
                {
                    rb.velocity = new Vector2(0, 0);
                }
                waitTimer = 0;
            }

        }
    }
    private void moveToEnemy()//追击敌人
    {
        float distance = Vector3.Distance(targetEnemy.transform.position, transform.position);
        Vector2 direction = (targetEnemy.transform.position - transform.position);
        Vector2 horizontalDirection = new Vector2(direction.x, 0).normalized;
        if (distance >attackRange)
        {
            rb.velocity = horizontalDirection * moveSpeed;
        }
        if(distance<attackRange)
        {
            rb.velocity = new Vector2(-horizontalDirection.x, 0);
        }
        if (distance == attackRange)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
}
