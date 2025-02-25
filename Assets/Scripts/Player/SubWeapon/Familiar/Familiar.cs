using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Familiar : MonoBehaviour
{
    public int neededManaPerS;
  
    [Header("基础属性")]
    public float maxhealth;
    public float currentHealth;
    public float damage;
    public float invulnerableDuration;//无敌时间
    
    public float attackRange;//攻击范围
    public float attackInterval;//攻击间隔
    public float farmiliarBulletSpeed;//子弹速度
    public int level=1;
    public float attackNums;//想做成连射的 还没做好
    private float intervalOfOneBullet=0.2f;
    private float shootTimer;
   
    [Header("移动相关")]
    private bool faceRight;
    public float chaseRange;//追击范围
    public float maxRangeToPlayer;//与玩家的最远距离
    public float moveSpeed;//移速
    public float jumpForce;//跳跃高度
    public float jumpInterVal = 3;//用于调整最短跳跃间隔 防止连跳？
    private float jumpTimer;//不用赋值
    private float waitInterval = 5;//每过几秒在玩家旁边移动一下
    private float verticalDistance;//与玩家的最大垂直距离 用于跳跃 不用赋值
    public float maxVerticalDistanceToPlayer;//给这个赋值用以调整跳跃行动
    public float Length;
    public float Width;

    [Header("升级相关")]
    public float increaseDamageCount;
    public float incresehealthCount;
    public float decreseAttackIntervalCount;
    [Header("检测范围")]
    public float detectionWidth;//检测敌人的范围
    public float detectionHeight;
    

    
    public bool invulnerable;
   
    private float waitTimer = 0;
   
    private float attackTimer;  
    private float invulnerableTimer;



    private bool isOnGround = true;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D rb;
    private GameObject player;
    private Player _player;
    private Transform targetEnemy;
  
    public GameObject familiarBulletPrefab;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = player.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        attackTimer = 0;
        jumpTimer = jumpInterVal;
        shootTimer = 0;
        
        invulnerable = false;
        invulnerableTimer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
        attackTimer -= Time.deltaTime;
        waitTimer += Time.deltaTime;
        jumpTimer -= Time.deltaTime;
        invulnerableTimer -=Time.deltaTime;
        shootTimer -= Time.deltaTime;
        //moveToPlayer();
        getCurrentEnemyTarget();
        attack();       
        //moveToEnemy();
        Flip();
        move();
      
        


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


        
            if (targetEnemy != null && attackTimer <= 0 && Vector2.Distance(targetEnemy.position, transform.position) < attackRange)
            {
            for (int i = 0; i < attackNums; i++)
            {
                if (shootTimer <= 0)
                {
                    GameObject farmiliarBullet = Instantiate(familiarBulletPrefab, transform.position, Quaternion.identity);
                    Vector2 direction = (targetEnemy.position - transform.position).normalized;
                    Rigidbody2D rb = farmiliarBullet.GetComponent<Rigidbody2D>();
                    rb.velocity = direction * farmiliarBulletSpeed;
                    attackTimer = attackInterval;
                    shootTimer = intervalOfOneBullet;
                }
            }
            }
        
        
    }

    

    private void moveToPlayer()//移动向玩家且在玩家旁边行动
    {
       
        float distance = Vector3.Distance(player.transform.position, transform.position);
        Vector2 direction = (player.transform.position - transform.position);
        Vector2 horizontalDirection = new Vector2(direction.x, 0).normalized;
        float horizontalDistance = Mathf.Abs(direction.x);
        if(horizontalDistance > maxRangeToPlayer)
        {
           
            rb.velocity = horizontalDirection * moveSpeed;
        }    
        if(horizontalDistance < maxRangeToPlayer)//暂时用于其在玩家旁移动而不是傻站着
        {
            

            if (waitTimer>waitInterval)
            {
               
                float randomNum = Random.Range(0, 2);
                if(randomNum>1.25)
                {
                    rb.velocity = new Vector2(1,rb.velocity.y).normalized*moveSpeed;

                }
                if(randomNum<=0.75)
                {
                    rb.velocity = new Vector2(-1, rb.velocity.y).normalized*moveSpeed;
                }
                if(randomNum>0.75&&randomNum<=1.26)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                waitTimer = 0;
            }
        

        }
         verticalDistance = Mathf.Abs(direction.y);//后面是跟随玩家跳跃的逻辑
        
        if (verticalDistance >=maxVerticalDistanceToPlayer  && isOnGround == true&&jumpTimer<=0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimer = jumpInterVal;
            isOnGround = false;
        }
    }
    private void moveToEnemy()//追击敌人
    {
        if (targetEnemy != null)
        {
            Vector2 direction = (targetEnemy.transform.position - transform.position);
            Vector2 horizontalDirection = new Vector2(direction.x, rb.velocity.y).normalized;
            if (Vector2.Distance(targetEnemy.position, transform.position) > attackRange)
            {
                rb.velocity = horizontalDirection * moveSpeed;
            }
            if (Vector2.Distance(targetEnemy.position, transform.position) < attackRange)
            {
                rb.velocity = new Vector2(-horizontalDirection.x, rb.velocity.y);
            }
            if (Vector2.Distance(targetEnemy.position, transform.position) == attackRange)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)//检测是否在地面上 用于跳跃
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
    public void upgrade()//升级代码 捡升级物品时调用
    {
        damage += increaseDamageCount;
        attackInterval-=decreseAttackIntervalCount;
        maxhealth += incresehealthCount;
        currentHealth = maxhealth;
        level++;
        if(level%3==0)
        {
            attackNums++;
        }
    }
    public  void TakeDamage(float damage)//未测试
    {
        if(invulnerableTimer<=0)
            invulnerable = false;
        if(invulnerableTimer>0)
            invulnerable = true;  
        if (invulnerable)
            return;
        if (!invulnerable)
        {
            currentHealth -= damage;
            invulnerableTimer = invulnerableDuration;
            if (currentHealth <= 0)
                Destroy(gameObject);
        }
       
        
    }
    private void Flip()
    {
        if (rb.velocity.x > 0 && faceRight == false)
        {
            _spriteRenderer.flipX = true;
            faceRight = true;
        }
        if (rb.velocity.x < 0 && faceRight == true)
        {
            _spriteRenderer.flipX = false;
            faceRight = false;
        }
    }
    private void move()
    {
        
        if (_player.faceRight==true)
        {
            _spriteRenderer.flipX = true ;
            transform.position = new Vector2(player.transform.position.x + Length, player.transform.position.y + Width);
        }
        else
        {
            _spriteRenderer.flipX = false;
            transform.position = new Vector2(player.transform.position.x - Length, player.transform.position.y + Width);
        }
    }
}
