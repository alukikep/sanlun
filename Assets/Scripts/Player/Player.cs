using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;//����ģʽ



    public float speedRate; // ����ϵ��
    public float jumpForce; // ��Ծ�߶�
    private float oriJumpForce;
    private bool isSlowed;
    public bool isBat;
    public bool isMouse;
    public bool faceRight;
    public float jumpDir;
    private float maxSpeed;
    private bool highJump;
    private float leftSlowDuration;
   
    

    [Header("�����")]
    public bool isBlock;
    public bool blockSuc;
    private float blockCoolTimer;
    [SerializeField]private float blockCoolTime;
    public float blockBonus;

    [Header("Attack")]
    public Transform attackCheck;
    public float attackRadius;
    public bool isAttack;
    public float ATK;
   

    [Header("Health")]
    public float maxHealth;
    public float health;
    [Header("Mana")]
    public float maxMana;
    public float currentMana;
    public float ManaPerSecond;
    private float ManaPSOnSlow;
    [Header("���������")]

    public GameObject axe;
    public bool isAxe;
    public GameObject guardian;
    public bool isGuardian;
    public int guardianNum;
    public GameObject TimeSlow;
    private TimeSlow timeSlowScript;
    public bool isTimeSlowed;
    private Guardian guardianScript;
    private Axe axeScript;
   


    private Rigidbody2D rigidbody2D;
    private Animator animation;
    private BoxCollider2D boxCollider2D;
    private float xSpeed;
    private int jumpNumber = 0; // 0,1,2�ֱ��ʾ��Ծ��0��1��2�Σ����ƶ�����
    private int jumpLimit;

    private bool isdoubleJumpEnabled = false;
    private bool ishighJumpEnabled=false;
    private bool isbatTransformEnabled=false;
    private bool isratTransformEnabled = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // ȷ������������ȷ
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        oriJumpForce = jumpForce;
        maxSpeed = speedRate;
        Debug.Log("start");
        rigidbody2D = GetComponent<Rigidbody2D>();
        animation = GetComponentInChildren<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        isBat = false;
        faceRight = true;
        jumpLimit = 1;
        currentMana = maxMana;
        if (guardian == null)
        {
            return;
        }
        else
        {
            guardianScript = guardian.GetComponent<Guardian>();
        }
        if(axe ==null)
        {
            return ;
        }
        else
        {
            axeScript = axe.GetComponent<Axe>();
        }
        if(TimeSlow==null)
        {
            return;
        }
        else
        {
            timeSlowScript = TimeSlow.GetComponent<TimeSlow>();
        }
        ManaPSOnSlow = ManaPerSecond /timeSlowScript.slowDownFactor;


    }

    // Update is called once per frame
    void Update()
    {
        xSpeed = Input.GetAxisRaw("Horizontal");
        SpeedUp();
        
       
        if(currentMana>=maxMana)
        {
            currentMana = maxMana;
        }
        if (Input.GetButtonDown("Jump") && jumpNumber < jumpLimit)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpNumber++;
        }

        if (isAttack == false&&isBlock==false&& timeSlowScript.TimeSlowActive==false)//�޸���һ���������仺�ٵĸ�����
        {
            rigidbody2D.velocity = new Vector2(xSpeed * speedRate, rigidbody2D.velocity.y);
            rigidbody2D.gravityScale = 2;
            jumpForce = oriJumpForce;
           
        }
        else if(isAttack == false && isBlock == false && timeSlowScript.TimeSlowActive == true)
        {
            jumpForce = 38;
            rigidbody2D.gravityScale = 20;      
            rigidbody2D.velocity = new Vector2(xSpeed * speedRate / Time.timeScale,rigidbody2D.velocity.y);           
        }
        else
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }

        Flip();
        AnimatorControllers();
        DoubleJump();
        HighJump();
        BatTranform();
        MouseTransform();
        Attack();
        Block();
        SubWeapon();
        resetSpeed();
        blockCoolTimer-=Time.deltaTime;
        leftSlowDuration -= Time.deltaTime;

        if (health<0)
        {
            Destroy(gameObject);
        }
        if(timeSlowScript.TimeSlowActive==false)
        {
            currentMana += ManaPerSecond * Time.deltaTime;
        }
        else
        {
            currentMana += ManaPSOnSlow * Time.deltaTime;
        }


    }
    private void SpeedUp()
    {
        // ��סshift�ٶȷ���
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            xSpeed *= 2;
        }
    }

    private void DoubleJump()
    {
        if (!isdoubleJumpEnabled) return;
        Debug.Log("highjump");
        jumpLimit = 2;
    }

    private void HighJump()
    {
        if (!ishighJumpEnabled) return;

        if (Input.GetKeyDown(KeyCode.Q) && jumpNumber != 0)//�ڿ��а�Q����������������)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce * 3);
            highJump = true;
            jumpNumber = 2;
        }
    }
    private void BatTranform()//��R�������𣨿���ʱ��
    {
        if (!isbatTransformEnabled) return;

        //������̬�����裩
        if (Input.GetKeyDown(KeyCode.R) && isBat == false && isMouse == false)
        {
            rigidbody2D.drag = 10;
            isBat = true;
            boxCollider2D.size = new Vector2(0.4f, 0.8f);
        }
        //��r��������ʹ�����ٶȱ����Ҳ�������Ծ
        else if (Input.GetKeyDown(KeyCode.R) && isBat == true && isMouse == false)
        {
            rigidbody2D.drag = 1;
            isBat = false;
            boxCollider2D.size = new Vector2(0.4f, 1.7f);
        }
        //�ɿ�r��ԭ
    }

    private void MouseTransform()//��E��������
    {
        if (!isratTransformEnabled) return;


        if (Input.GetKeyDown(KeyCode.E) && isMouse == false)
        {
            isMouse = true;
            jumpForce = 8;
            boxCollider2D.size = new Vector2(0.4f, 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isMouse == true)
        {
            isMouse = false;
            jumpForce = 14;
            boxCollider2D.size = new Vector2(0.4f, 1.7f);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && isMouse == false && isBat == false && isAttack == false)
        {
            isAttack = true;
            isBlock = false;
        }
    }

    private void Block()
    {
        if (Input.GetKeyDown(KeyCode.F) && isBlock == false&&blockCoolTimer<0)
        {
            isBlock = true;
            blockCoolTimer = blockCoolTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��������ǩΪ��ground���ĵ���Ӵ���������Ծ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            jumpNumber = 0;
            highJump = false;
        }

        //��player��UnlockItem��ײʱ��������Ӧ�ļ���
        UnlockItem UnlockItem = collision.GetComponent<UnlockItem>();
        if (UnlockItem != null)
        {
            Ability ability = UnlockItem.abilityToUnlock;
            switch (ability)
            {
                case Ability.DoubleJump:
                    isdoubleJumpEnabled = true;
                    break;
                case Ability.HighJump:
                    ishighJumpEnabled = true;
                    break;
                case Ability.BatTransform:
                    isbatTransformEnabled = true;
                    break;
                case Ability.RatTransform:
                    isratTransformEnabled = true;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
           jumpNumber = 1;
        }
    }

    private void AnimatorControllers()//���ƶ���
    {
        bool isMoving = rigidbody2D.velocity.x != 0;
        bool isGround = jumpNumber == 0;
        if (rigidbody2D.velocity.y > 0)
        {
            jumpDir = 1;
        }
        if (rigidbody2D.velocity.y < 0)
        {
            jumpDir = -1;
        }
        animation.SetBool("isMoving", isMoving);
        animation.SetBool("isBat", isBat);
        animation.SetBool("isGround", isGround);
        animation.SetFloat("jump", jumpDir);
        animation.SetBool("highJump", highJump);
        animation.SetBool("isMouse", isMouse);
        animation.SetBool("isAttack", isAttack);
        animation.SetBool("isBlock", isBlock);
        animation.SetBool("blockSuc", blockSuc);
    }

    private void Flip()//����ת��
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

    public void GetDamage(float eATK)
    {
        if (isBlock == false)
        {
            health = health - eATK;
        }
        if(isBlock == true)
        {
            blockSuc = true;
        }
       
    }
    public void SubWeapon()
    {
        if (Input.GetKeyDown(KeyCode.T)&&isAxe&&currentMana>=axeScript.neededMana)
        {
            GameObject SubWeapon = Instantiate(axe, transform.position, Quaternion.identity);
            currentMana-=axeScript.neededMana;
        }     
        if (Input.GetKeyDown(KeyCode.T)&& isGuardian&&guardianScript != null&&currentMana>=guardianScript.neededMana)
        {
            float radius = guardianScript.radius;
            float angleStep = 360 / guardianNum;
            for(int i=0;i<guardianNum;i++)
            {
                float angle = angleStep * i;
                float x = radius*Mathf.Cos(angle*Mathf.Deg2Rad);
                float y = radius*Mathf.Sin(angle*Mathf.Deg2Rad);
                Vector3 Pos = transform.position+new Vector3(x, y, 0);
                GameObject subWeapon = Instantiate(guardian, Pos, Quaternion.identity);
            }
            currentMana-=guardianScript.neededMana;
        }
        if(Input.GetKeyDown(KeyCode.T)&&isTimeSlowed&&currentMana>=timeSlowScript.neededMana)
        {
            StartCoroutine(timeSlowScript.ActiveTimeSlow());
            currentMana-=timeSlowScript.neededMana;
        }
    }
    public void getSlowed(float slowPercent, float slowDuration)
    {   
        leftSlowDuration = slowDuration;
        
        
        if (leftSlowDuration>0&&isSlowed==false)
        {
            isSlowed = true;
            speedRate *=  slowPercent;
            
        }
        
        
    }
    public void resetSpeed()
    {
        if(leftSlowDuration<=0)
        {
            speedRate = maxSpeed;
            isSlowed = false;
        }

    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;

        Debug.Log("Player healed! Current health: " + health);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        Debug.Log("Player's max health increased! New max health: " + maxHealth);
    }

    public void IncreaseAttack(int amount)
    {
        ATK += amount;
        Debug.Log("Player's attack increased! New attack: " + ATK);
    }
}