using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float ATK;//目前未做伤害处理
    public float speed;
    public float movingInterval;//调整左右移动的频率
    private float movingTimer;
    public float attackInterval;//调整所有攻击的间隔
    public GameObject BatTrans;
    [Header("召唤蝙蝠")]
    public int batNum;
    public float spawnInterval;
    public GameObject littleBatPrefab;
    public float spawnWidth;
    private bool LittleBat;
    private bool startSpawn;
    private float spawnTimer;
    private Vector3 spawnUp;
    private Vector3 spawnDown;
    private int currentNum;
    [Header("超声波")]
    public GameObject wavePrefab;
    private bool Wave;

 
     
    private float attackTimer;
    private bool isAttacking;
    private bool diving;

    private EnemyHealth enemyHealth;
    private GameObject Player;
    private Rigidbody2D rb;
    private Vector3 targetPosition; 
    private Vector3 originalPosition; 
    private bool hasStartedDiving = false; 
    


    private void Start()
    {
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        movingTimer = 2;
        spawnTimer = 0;
        attackTimer = attackInterval;
    }
    private void Update()
    {
        movingTimer-= Time.deltaTime;
        spawnUp = new Vector3(transform.position.x,Player.transform.position.y+spawnWidth,transform.position.z);
        spawnDown = new Vector3(transform.position.x, Player.transform.position.y - spawnWidth, transform.position.z);
        if (!isAttacking)
        {
            moving();
           
        }
        if(enemyHealth.health<=0)
        {
            GameObject batTrans = Instantiate(BatTrans, transform.position, Quaternion.identity);
        }
        
        
        Attack(); 
        
        

    }
    private void Attack()
    {
        if (!isAttacking)
        { 
            attackTimer -= Time.deltaTime; 
        }
        if(attackTimer<=0)
        {
            int attackNum = Random.Range(0, 3);//随机数控制攻击方式 
            {
                if (attackNum == 0)
                {
                    diving = true;
                    isAttacking = true;
                    hasStartedDiving = false ;
                    attackTimer = attackInterval;
                }
                if(attackNum == 1)
                {
                    LittleBat = true;
                    isAttacking = true;
                    attackTimer = attackInterval;
                    startSpawn = false;
                }
                if(attackNum == 2)
                {
                    Wave = true;
                    isAttacking = true;
                    attackTimer = attackInterval;
                }
            }
        }
       
       
        if (diving)//冲向玩家然后返回的攻击方式
        {
            rb.velocity = Vector2.zero;
            if (!hasStartedDiving)
            {
                
                originalPosition = transform.position;
                targetPosition = Player.transform.position;
                hasStartedDiving = true;
            }

            
            Vector2 directionToTarget = (targetPosition - transform.position).normalized;
            rb.velocity = directionToTarget * speed * 3;

           
            if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
            {
                rb.velocity = Vector2.zero;
                hasStartedDiving = false; 
                diving = false; 
            }
        }
        else if (!hasStartedDiving && Vector3.Distance(transform.position, originalPosition) > 0.1f && isAttacking)
        {
           
            Vector2 directionToOriginal = (originalPosition - transform.position).normalized;
            rb.velocity = directionToOriginal * speed;

           
            if (Vector3.Distance(transform.position, originalPosition) <= 0.5f)
            {
                rb.velocity = Vector2.zero;
                transform.position = originalPosition;                
                isAttacking = false;
            }
        }
        

        if(LittleBat)
        {
         
            spawnTimer-=Time.deltaTime;
            if(!startSpawn)
            {
                startSpawn = true;
                currentNum = 0;
            }
            if(spawnTimer<=0&&startSpawn==true)
            {
                float vertical = Random.Range(spawnDown.y, spawnUp.y);
                Vector3 spawnPos = new Vector3(transform.position.x,Player.transform.position.y-spawnWidth,transform.position.z);              //暂时设置为定点召唤吧                                                                                                                                              
                GameObject littlebat =  Instantiate(littleBatPrefab, spawnPos, Quaternion.identity);               
                currentNum += 1;
                spawnTimer = spawnInterval;
                if(currentNum==batNum)
                {
                    startSpawn=false;
                    LittleBat = false;
                    isAttacking =false; 
                }
            }

        }
        if(Wave)
        {
            GameObject wave = Instantiate(wavePrefab,transform.position,Quaternion.identity);
            isAttacking = false ;
            Wave = false;
        }
    }

    
  
    private void moving()
    {
        Vector2 direction = (Player.transform.position - transform.position).normalized;
        Vector2 moveDirection = new Vector2(direction.x, 0);
        float moveNum = Random.Range(0,4);//随机数控制不攻击时左右移动以及不动 后续调整概率
        if (movingTimer < 0)
        {
            
            if (moveNum > 0 && moveNum <= 1)
            {
                
                rb.velocity = moveDirection * speed;
                movingTimer = movingInterval;
            }
            if (moveNum > 1 && moveNum <= 2)
            {
                
                rb.velocity = -moveDirection * speed;
                movingTimer = movingInterval;
            }
            if (moveNum > 2)
            {
               
                rb.velocity = Vector2.zero;
                movingTimer = movingInterval;
                
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().GetDamage(ATK);
        }
    }
}
