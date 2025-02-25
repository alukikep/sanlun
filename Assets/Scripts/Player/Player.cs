using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEditor.SearchService;
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
    public BlockSuc blockSucState {  get; private set; }
    public HighJump highJumpState {  get; private set; }
    public Retreat retreatState { get; private set; }
  





    public float raycastDistance = 1f; 
    public LayerMask obstacleLayer; 

    public static Player Instance;//单例模式

    public float speedRate; // 速率系数
    private float recordSpeedRate;
    public float jumpForce; // 跳跃高度

    public Rigidbody2D rigidbody2D;
    public Animator anim;
    private Menu menu;
    public bool canRestore;
    [SerializeField]private Renderer renderer;

   
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
    public KeyCode SwitchKey = KeyCode.U;
    private int CurrentSubWeaponNum=0;
    private int maxSubWeaponNum = 3;
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
    [HideInInspector]public float oriG;
    [HideInInspector] public float oriJumpForce;
    private bool speedFixed=false;


    public  CapsuleCollider2D capsuleCollider2D;
    public float xSpeed;
    public int jumpNumber; // 0,1,2分别表示跳跃了0，1，2次，控制二段跳
    public int jumpLimit;

    

    public bool isdoubleJumpEnabled = false;
    public bool ishighJumpEnabled=false;
    public bool isbatTransformEnabled=false;
    public bool isratTransformEnabled = false;
    public bool isAxeEnabled = false;
    public bool isGuardianEnabled = false;
    public bool isTimeSlowEnabled = false;
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

        oriG = rigidbody2D.gravityScale;
        anim = GetComponentInChildren<Animator>();

        StateMachine.Initialize(idleState);

        
        Debug.Log("start");
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
        StateMachine.currentState.Update();
        blockCoolTimer -= Time.deltaTime;

        HealthBar.maxHealth = maxHealth;
        HealthBar.currentHealth = health;
        MagicBar.maxMagic = maxMana;
        MagicBar.currentMagic = (int)currentMana;

        protTime -= Time.deltaTime;


        if (currentMana>=maxMana)
        {
            currentMana = maxMana;
        }
        attackTimer -= Time.deltaTime;
        

        if (isAttack == false&&isBlock==false&& timeSlowScript.TimeSlowActive==false)//修改了一下用于适配缓速的副武器
        {
            rigidbody2D.gravityScale = oriG;
            jumpForce = oriJumpForce;
          
            if (speedFixed == false)
            {
                rigidbody2D.velocity = new Vector2(xSpeed * speedRate / Time.timeScale, rigidbody2D.velocity.y / 5);
                Debug.Log("yes");
            }
            speedFixed = true;
        }
        else if(isAttack == false && isBlock == false && timeSlowScript.TimeSlowActive == true)
        {
            jumpForce = 38;
            rigidbody2D.gravityScale = 20;
            speedFixed = false;
            rigidbody2D.velocity = new Vector2( xSpeed * speedRate / Time.timeScale,rigidbody2D.velocity.y);           
        }
       
        resetSpeed();
        SubWeapon();
       
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

       if(xSpeed>0)
        {
            faceRight = true;
        }
       if(xSpeed<0)
        {
            faceRight= false;
        }
    }

    public bool CanRestore()
    {
        Vector2 raycastOrigin =capsuleCollider2D.transform.position-new Vector3(0,0.5f,0);
       
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

    public void SetVelocity(float _xVelocity,float _yVelocity)
    {
        Debug.Log(_xVelocity);
        rigidbody2D.velocity= new Vector2(_xVelocity, _yVelocity);
        Debug.Log("Move");
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

    private void HurtProtect(int num,float seconds)
    {
        StartCoroutine(HurtPro(num, seconds));
    }

    IEnumerator HurtPro(int num, float seconds)
    {
        for(int i = 0; i < num*2; i++)
        {
            renderer.enabled=!renderer.enabled;
            yield return new WaitForSeconds(seconds);
        }
        renderer.enabled=true;
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
                    maxSubWeaponNum++;
                    break;
                case Ability.Guardian:
                    isGuardianEnabled = true;
                    maxSubWeaponNum++;
                    break;
                case Ability.TimeSlow:
                    isTimeSlowEnabled = true;
                    maxSubWeaponNum++;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")&&mouseState.isMouse==false)
        {
           StateMachine.ChangeState(fallState);
           jumpNumber = 1;
        }
    }


    public void GetDamage(float eATK)
    {
        if (isBlock == false&&protTime<0)
        {
            protTime = proTime * proNumber * 2;
            audioController.PlaySfx(audioController.playerHurt);
            protect = true;
            health = health - eATK;
            HurtProtect(proNumber, proTime);
        }
        if(isBlock == true)
        {
            StateMachine.ChangeState(blockSucState);
        }
       
    }



    public void SubWeapon()
    {   if(Input.GetKeyDown(SwitchKey))
        {
            CurrentSubWeaponNum =(CurrentSubWeaponNum+1)%maxSubWeaponNum;
            if(CurrentSubWeaponNum==0)
            {
                isAxe = true;
                isGuardian = false;
                isTimeSlowed = false;
            }
            else if(CurrentSubWeaponNum ==1)
            {
                isAxe = false;
                isGuardian = true;
                isTimeSlowed = false;

            }
            else if(CurrentSubWeaponNum == 2)
            {
                isAxe = false;
                isGuardian = false;
                isTimeSlowed = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.U)&&isAxe&&currentMana>=axeScript.neededMana)
        {
            GameObject SubWeapon = Instantiate(axe, transform.position, Quaternion.identity);
            currentMana-=axeScript.neededMana;
        }     
        if (Input.GetKeyDown(KeyCode.U)&& isGuardian&&guardianScript != null&&currentMana>=guardianScript.neededMana)
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
        if(Input.GetKeyDown(KeyCode.U)&&isTimeSlowed&&currentMana>=timeSlowScript.neededMana)
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

    public void IncreaseEnternalAttack(int amount)
    {
        ATK += amount;
        Debug.Log("Player's attack increased! New attack: " + ATK);
    }
    public void IncreaseTemporaryAttack(int amount,int duration)
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
    public void IncreaseSpeedRate(float rate,float duration)
    {
        recordSpeedRate = maxSpeed;
        maxSpeed*=rate;
        StartCoroutine(RestorexSpeedAfterDelay(duration));
    }
    private IEnumerator RestorexSpeedAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration); // 等待指定时间
        maxSpeed = recordSpeedRate;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string TargetSpawnName = LoadScean.TargetSpawnPoint;
        GameObject Player = GameObject.Find("Player");
        GameObject TargetSpawn = GameObject.Find(TargetSpawnName);
        transform.position = TargetSpawn.transform.position;



        CinemachineVirtualCamera virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCam.Follow = Player.transform;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded-=OnSceneLoaded;
    }
    public void SavePlayer()
    {
        PlayerSaveData saveData = new PlayerSaveData
        {
            health = health,
            currentMana = currentMana,
            position = transform.position,
            isDoubleJumpEnabled = isdoubleJumpEnabled,
            isHighJumpEnabled = ishighJumpEnabled,
            isBatTransformEnabled = isbatTransformEnabled,
            isRatTransformEnabled = isratTransformEnabled,
            isAxeEnabled = isAxeEnabled,
            isGuardianEnabled = isGuardianEnabled,
            isTimeSlowEnabled = isTimeSlowEnabled,
            attack = ATK,
            currentSceneName = SceneManager.GetActiveScene().name,
            inventoryItems = SaveInventory()
        };

        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }
    public void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(json);

            health = saveData.health;
            currentMana = saveData.currentMana;
            transform.position = saveData.position;
            isdoubleJumpEnabled = saveData.isDoubleJumpEnabled;
            ishighJumpEnabled = saveData.isHighJumpEnabled;
            isbatTransformEnabled = saveData.isBatTransformEnabled;
            isratTransformEnabled = saveData.isRatTransformEnabled;
            isAxeEnabled = saveData.isAxeEnabled;
            isGuardianEnabled = saveData.isGuardianEnabled;
            isTimeSlowEnabled = saveData.isTimeSlowEnabled;
            ATK = saveData.attack;
            SceneManager.LoadScene(saveData.currentSceneName);

            // Load inventory items
            LoadInventory(saveData.inventoryItems);
        }
    }

    private List<InventoryItemData> SaveInventory()
    {
        List<InventoryItemData> inventoryItems = new List<InventoryItemData>();
        foreach (var item in Inventory.Instance.InventoryItems)
        {
            InventoryItemData itemData = new InventoryItemData
            {
                itemName = item.data.itemName,
                iconPath = item.data.icon.name,
                quantity = item.stackSize
            };
            inventoryItems.Add(itemData);
        }
        return inventoryItems;
    }

    private void LoadInventory(List<InventoryItemData> inventoryData)
    {
        foreach (var itemData in inventoryData)
        {
            ItemData item = Resources.Load<ItemData>("Items/" + itemData.itemName); // 需要根据实际情况调整路径
            if (item != null)
            {
                Inventory.Instance.AddItem(item);
                Inventory.Instance.inventoryDictionary[item].stackSize = itemData.quantity;
            }
        }
    }
}