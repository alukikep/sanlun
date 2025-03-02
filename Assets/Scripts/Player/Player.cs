using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    public Idle idleState { get; private set; }
    public Move moveState { get; private set; }
    public Jump jumpState { get; private set; }
    public Fall fallState { get; private set; }
    public PlayerAttack attackState { get; private set; }
    public BatState batState { get; private set; }
    public MouseState mouseState { get; private set; }
    public AirAttack airAttackState { get; private set; }
    public Block blockState { get; private set; }
    public BlockSuc blockSucState { get; private set; }
    public HighJump highJumpState { get; private set; }
    public Retreat retreatState { get; private set; }






    public float raycastDistance = 1f;
    public LayerMask obstacleLayer;
    public float startTime;
    public float currentTime;
    public static Player Instance;//单例模式
    private HashSet<string> collectedPotions = new HashSet<string>();
    public Sprite CheckpointPicture;
    public float speedRate; // 速率系数
    private float recordSpeedRate;
    public float jumpForce; // 跳跃高度

    public Rigidbody2D rigidbody2D;
    public Animator anim;
    public bool canRestore;
    [SerializeField] private Renderer renderer;
    public GameObject blockEffect;


    private bool isSlowed;
    public bool isBat;
    public bool isMouse;
    public bool faceRight;
    public float jumpDir;
    private float maxSpeed;
    private bool highJump;
    private float leftSlowDuration;
    private bool protect;
    [SerializeField] private float protectTime;
    [SerializeField] private float recontrolTime;
    [SerializeField] private float hurtMove;
    public GameObject audio;
    public AudioController audioController;
    public BoxCollider2D boxCollider;


    [Header("格挡相关")]
    public bool isBlock;
    public bool blockSuc;
    public float blockCoolTimer;
    public float blockCoolTime;
    public float blockBonus;

    [Header("Attack")]
    public Transform attackCheck;
    public float attackRadius;
    public bool isAttack;
    public float ATK;
    private float recordATK;//用于记录ATK
    public float attackTime;
    public float attackTimer;


    [Header("Health")]
    public float maxHealth;
    public float health;
    [SerializeField] private float proTime;
    [SerializeField] private int proNumber;
    private float protTime;


    [Header("Mana")]
    public float maxMana;
    public float currentMana;
    public float ManaPerSecond;
    private float ManaPSOnSlow;

    [Header("副武器相关")]
   
    public KeyCode SwitchKey = KeyCode.I;
    public KeyCode UseKey = KeyCode.O;
    private List<string> collectedWeapons = new List<string> ();
    private int currentWeaponIndex = -1;
    private int maxSubWeaponNum = 0;
    public GameObject axe;
    public bool isAxe;
    private Axe axeScript;
    public GameObject familiar;
    public bool isFamiliar;
    private Familiar familiarScript;
    private bool isFamiliarAlive;
    private GameObject currentFamiliar;
    public GameObject guardian;
    public bool isGuardian;
    public int guardianNum;
    private Guardian guardianScript;
    public GameObject TimeSlow;
    private TimeSlow timeSlowScript;
    public bool isTimeSlowed;


    [HideInInspector] public float oriG;
    [HideInInspector] public float oriJumpForce;
    private bool speedFixed = false;


    public CapsuleCollider2D capsuleCollider2D;
    public float xSpeed;
    public int jumpNumber; // 0,1,2分别表示跳跃了0，1，2次，控制二段跳
    public int jumpLimit;



    public bool isdoubleJumpEnabled = false;
    public bool ishighJumpEnabled = false;
    public bool isbatTransformEnabled = false;
    public bool isratTransformEnabled = false;
    public bool isAxeEnabled = false;
    public bool isFamiliarEnabled = false;
    public bool isGuardianEnabled = false;
    public bool isTimeSlowEnabled = false;

    public GameObject pauseMenu;
    public bool isPauseMenuEnabled;
    public GameObject DieMenu;
    public bool isDieMenuEnabled;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this; // 确保单例引用正确
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        oriJumpForce = jumpForce;
        maxSpeed = speedRate;


        StateMachine = new PlayerStateMachine();

        idleState = new Idle(this, StateMachine, "Idle");
        moveState = new Move(this, StateMachine, "Move");
        jumpState = new Jump(this, StateMachine, "Jump");
        fallState = new Fall(this, StateMachine, "Jump");
        attackState = new PlayerAttack(this, StateMachine, "GroundAttack");
        batState = new BatState(this, StateMachine, "Bat");
        mouseState = new MouseState(this, StateMachine, "Mouse");
        airAttackState = new AirAttack(this, StateMachine, "AirAttack");
        blockState = new Block(this, StateMachine, "Block");
        blockSucState = new BlockSuc(this, StateMachine, "BlockSuc");
        highJumpState = new HighJump(this, StateMachine, "HighJump");
        retreatState = new Retreat(this, StateMachine, "Retreat");


    }
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.unscaledTime;
        LoadPlayerTime();
        oriG = rigidbody2D.gravityScale;
        anim = GetComponentInChildren<Animator>();

        StateMachine.Initialize(idleState);


        rigidbody2D = GetComponent<Rigidbody2D>();

        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        audio = GameObject.FindGameObjectWithTag("Audio");
        audioController = audio.GetComponent<AudioController>();
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
        if (axe == null)
        {
            return;
        }
        else
        {
            axeScript = axe.GetComponent<Axe>();
        }
        if (TimeSlow == null)
        {
            return;
        }
        else
        {
            timeSlowScript = TimeSlow.GetComponent<TimeSlow>();
        }
        if (familiar == null)
        {
            return;
        }
        else
        {
            familiarScript = familiar.GetComponent<Familiar>();
        }

        familiarScript = familiar.GetComponent<Familiar>();

        ManaPSOnSlow = ManaPerSecond / timeSlowScript.slowDownFactor;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)&&isPauseMenuEnabled==false)
        {
            pauseMenu.SetActive(true);
            isPauseMenuEnabled = true;
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.R) && isPauseMenuEnabled == true)
        {
            pauseMenu.SetActive(false);
            isPauseMenuEnabled = false;
            Time.timeScale = 1;
        }
            



        currentTime = Time.unscaledTime - startTime;
        StateMachine.currentState.Update();
        blockCoolTimer -= Time.deltaTime;

        HealthBar.maxHealth = maxHealth;
        HealthBar.currentHealth = health;
        MagicBar.maxMagic = maxMana;
        MagicBar.currentMagic = (int)currentMana;

        protTime -= Time.deltaTime;

        if (currentMana >= maxMana)
        {
            currentMana = maxMana;
        }
        attackTimer -= Time.deltaTime;


        if (isAttack == false && isBlock == false && timeSlowScript.TimeSlowActive == false)//修改了一下用于适配缓速的副武器
        {
            rigidbody2D.gravityScale = oriG;
            jumpForce = oriJumpForce;
         
            if (speedFixed == false)
            {
                if (Time.timeScale != 0)
                {
                    rigidbody2D.velocity = new Vector2(xSpeed * speedRate / Time.timeScale, rigidbody2D.velocity.y / 5);
                }
            }
            speedFixed = true;
        }
        else if (isAttack == false && isBlock == false && timeSlowScript.TimeSlowActive == true)
        {
            jumpForce = 38;
            rigidbody2D.gravityScale = 20;
            speedFixed = false;
          
            rigidbody2D.velocity = new Vector2(xSpeed * speedRate / Time.timeScale, rigidbody2D.velocity.y);
        }

        resetSpeed();
        SubWeapon();

        leftSlowDuration -= Time.deltaTime;

        if (health < 0)
        {
            DieMenu.SetActive(true);
            Time.timeScale = 0;
        }
        if (timeSlowScript.TimeSlowActive == false)
        {
            currentMana += ManaPerSecond * Time.deltaTime;
        }
        else
        {
            currentMana += ManaPSOnSlow * Time.deltaTime;
        }

        if (xSpeed > 0 && faceRight == false)
        {
            faceRight = true;
        }
        else if (xSpeed < 0 && faceRight == true)
        {
            faceRight = false;
        }
      
    }

    public bool CanRestore()
    {
        Vector2 raycastOrigin = capsuleCollider2D.transform.position - new Vector3(0, 0.5f, 0);

        raycastOrigin.y += capsuleCollider2D.size.y / 2;

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.up, raycastDistance, obstacleLayer);

        return hit.collider == null;
    }

    private void OnDrawGizmos()
    {
        if (capsuleCollider2D != null)
        {
            Vector2 raycastOrigin = capsuleCollider2D.transform.position - new Vector3(0, 0.5f, 0);
            raycastOrigin.y += capsuleCollider2D.size.y / 2;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(raycastOrigin, raycastOrigin + Vector2.up * raycastDistance);
        }
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rigidbody2D.velocity = new Vector2(_xVelocity, _yVelocity);
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

        if (Input.GetKeyDown(KeyCode.Q) && jumpNumber != 0)//在空中按Q高跳（可以无限跳)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce * 3);
            highJump = true;
            jumpNumber = 2;
        }
    }

    private void HurtProtect(int num, float seconds)
    {
        StartCoroutine(HurtPro(num, seconds));
    }

    IEnumerator HurtPro(int num, float seconds)
    {
        for (int i = 0; i < num * 2; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(seconds);
        }
        renderer.enabled = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当玩家与标签为“ground”的地面接触后，重置跳跃次数
        if (collision.gameObject.CompareTag("Ground"))
        {
            audioController.PlaySfx(audioController.fallGround);
            jumpNumber = 0;
            highJump = false;
        }

        //当player与UnlockItem碰撞时，解锁相应的技能
        UnlockItem UnlockItem = collision.GetComponent<UnlockItem>();
        if (UnlockItem != null)
        {
            Ability ability = UnlockItem.abilityToUnlock;
            switch (ability)
            {
                case Ability.DoubleJump:
                    isdoubleJumpEnabled = true;
                    jumpLimit = 2;
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
                case Ability.Axe:
                    isAxeEnabled = true;
                    collectedWeapons.Add("Axe");
                    maxSubWeaponNum++;
                    break;
                case Ability.Guardian:
                    isGuardianEnabled = true;
                    collectedWeapons.Add("Guardian");
                    maxSubWeaponNum++;
                    break;
                case Ability.TimeSlow:
                    isTimeSlowEnabled = true;
                    collectedWeapons.Add("TimeSlow");
                    maxSubWeaponNum++;
                    break;
                case Ability.familiar:
                    isFamiliarEnabled = true;
                    collectedWeapons.Add("Familiar");
                    maxSubWeaponNum++;
                    break;
            }
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && mouseState.isMouse == false)
        {
            StateMachine.ChangeState(fallState);
            jumpNumber = 1;
        }
    }


    public void GetDamage(float eATK)
    {
        if (isBlock == false && protTime < 0)
        {
            protTime = proTime * proNumber * 2;
            audioController.PlaySfx(audioController.playerHurt);
            protect = true;
            health = health - eATK;
            HurtProtect(proNumber, proTime);
        }
        if (isBlock == true)
        {
            StateMachine.ChangeState(blockSucState);
        }

    }



    public void SubWeapon()
    {
        if (Input.GetKeyDown(SwitchKey)&&timeSlowScript.TimeSlowActive==false)
        {
            
            currentWeaponIndex = (currentWeaponIndex+1)%maxSubWeaponNum;
            if (collectedWeapons[currentWeaponIndex]=="Axe")
            {
                isAxe = true;
                isFamiliar = false;
                isGuardian = false;
                isTimeSlowed = false;
            }
            else if (collectedWeapons[currentWeaponIndex]=="Familiar")
            {
                isAxe = false;
                isFamiliar = true;
                isGuardian = false;
                isTimeSlowed = false;

            }
            else if (collectedWeapons[currentWeaponIndex] =="Guardian")
            {
                isAxe = false;
                isFamiliar = false;
                isGuardian = true;
                isTimeSlowed = false;
            }
            else if (collectedWeapons[currentWeaponIndex] == "TimeSlow")
            {
                isAxe = false;
                isFamiliar = false;
                isGuardian = false;
                isTimeSlowed = true;
            }
            Destroy(currentFamiliar);
        }

        if (Input.GetKeyDown(UseKey) && isAxe && currentMana >= axeScript.neededMana)
        {
            GameObject SubWeapon = Instantiate(axe, transform.position, Quaternion.identity);
            currentMana -= axeScript.neededMana;
        }
        if (Input.GetKeyDown(UseKey) && isFamiliar && currentMana > 0 && isFamiliarAlive == false)
        {
            currentFamiliar = Instantiate(familiar, transform.position, Quaternion.identity);
            isFamiliarAlive = true;
        }
        else if (Input.GetKeyDown(UseKey) && isFamiliar && isFamiliarAlive == true)
        {
            Destroy(currentFamiliar);
            isFamiliarAlive = false;
        }
        if (currentMana >= 0 && isFamiliarAlive == true)
        {
            currentMana -= Time.deltaTime * familiarScript.neededManaPerS;
            if (currentMana < 0)
            {
                Destroy(currentFamiliar);
                isFamiliarAlive = false;
            }
        }
        if (Input.GetKeyDown(UseKey) && isGuardian && guardianScript != null && currentMana >= guardianScript.neededMana)
        {
            float radius = guardianScript.radius;
            float angleStep = 360 / guardianNum;
            for (int i = 0; i < guardianNum; i++)
            {
                float angle = angleStep * i;
                float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector3 Pos = transform.position + new Vector3(x, y, 0);
                GameObject subWeapon = Instantiate(guardian, Pos, Quaternion.identity);
            }
            currentMana -= guardianScript.neededMana;
        }
        if (Input.GetKeyDown(UseKey) && isTimeSlowed && currentMana >= timeSlowScript.neededMana)
        {
            StartCoroutine(timeSlowScript.ActiveTimeSlow());
            currentMana -= timeSlowScript.neededMana;
        }


    }
    public void getSlowed(float slowPercent, float slowDuration)
    {
        leftSlowDuration = slowDuration;


        if (leftSlowDuration > 0 && isSlowed == false)
        {
            isSlowed = true;
            speedRate *= slowPercent;

        }


    }
    public void resetSpeed()
    {
        if (leftSlowDuration <= 0)
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

    public void IncreaseEnternalAttack(int amount)
    {
        ATK += amount;
        Debug.Log("Player's attack increased! New attack: " + ATK);
    }
    public void IncreaseTemporaryAttack(int amount, int duration)
    {
        recordATK = ATK;
        ATK += amount; // 增加攻击力
        StartCoroutine(RestoreAttackAfterDelay(duration)); // 启动协程，等待恢复
    }
    // 协程：在指定时间后恢复攻击力
    private IEnumerator RestoreAttackAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration); // 等待指定时间
        ATK = recordATK; // 恢复原始攻击力
    }
    public void IncreaseMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana)
            currentMana = maxMana;

        Debug.Log(" Current mana: " + currentMana);
    }
    public void IncreaseMaxMana(int amount)
    {
        maxMana += amount;
        Debug.Log("Player's max mana increased! New max mana: " + maxMana);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string TargetSpawnName = LoadScene.TargetSpawnPoint;
        GameObject Player = GameObject.Find("Player");
        GameObject TargetSpawn = GameObject.Find(TargetSpawnName);
        if(TargetSpawn!=null)
        {
            transform.position = TargetSpawn.transform.position;

        }


        health = maxHealth;
        CinemachineVirtualCamera virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCam.Follow = Player.transform;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SavePlayerTime(currentTime);
    }
    
    public IEnumerator UpdateVirtualCameraAfterLoad()
    {
        // 等待场景加载完成
        yield return new WaitForSeconds(0.1f); // 等待0.1秒，确保场景加载完成

        // 获取虚拟摄像机
        CinemachineVirtualCamera virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCam != null)
        {
            virtualCam.Follow = transform; // 将虚拟摄像机的跟随目标设置为玩家
            virtualCam.LookAt = transform; // 将虚拟摄像机的跟随目标设置为玩家
        }
    }
    public void AddCollectedPotion(string potionID)
    {
        collectedPotions.Add(potionID);
    }

    public HashSet<string> GetCollectedPotions()
    {
        return collectedPotions;
    }
    public IEnumerator DestroyCollectedPotions()
    {
        yield return new WaitForSeconds(0.1f); // 等待0.1秒，确保场景加载完成

        GameObject[] allPotions = GameObject.FindGameObjectsWithTag("Potion");

        foreach (var potion in allPotions)
        {
            ItemObject itemObject = potion.GetComponent<ItemObject>();
            if (itemObject != null && collectedPotions.Contains(itemObject.potionID))
            {
                Debug.Log($"Destroying potion with ID: {itemObject.potionID}");
                Destroy(potion); // 销毁已收集的药水
            }
        }
    }
    
    private void SavePlayerTime(float time)
    {
        PlayerPrefs.SetFloat("PlayTime", time);
        PlayerPrefs.Save();
    }
    private void LoadPlayerTime()
    {
        float savedTime = PlayerPrefs.GetFloat("PlayTime", 0);

    }
}