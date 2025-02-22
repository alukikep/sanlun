using Unity.Mathematics;
using UnityEngine;

public class DemonWarrior : MonoBehaviour
{
    public int phase = 1;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Transform player;
    private CapsuleCollider2D capsuleCollider;
    private EnemyHealth enemyHealth;

    [Header("Attack")]
    public Transform attackCheck;
    public float attackRadius;
    public bool isAttack;

    [Header("阶段参数")]
    public int maxHealth = 3;
    public float stopDistance = 3f;
    public float phaseTransitionDuration = 1f;

    [Header("阶段1 - 挥击")]
    public float phase1MoveSpeed = 3f;
    public float phase1AttackCD = 2f;
    public float swipeDamage = 10f;

    [Header("阶段2 - 召唤")]
    public GameObject[] spawnPoints;
    public GameObject impPrefab;
    public float summonInterval = 5f;
    public float phase2YOffset = 2f;       // Y坐标与玩家的相对距离
    public float phase2XOffset = 5f;       // X坐标与玩家的相对距离
    private bool hasReceivedAttackInPhase2 = false; // 是否在第二阶段受到一次攻击

    [Header("阶段3 - 强化")]
    public float phase3SpeedMultiplier = 1.5f;
    public float phase3AttackMultiplier = 2f;
    private GameObject shieldEffect;
    public GameObject shieldEffectPrefab; // 盾牌效果预制体
    public float phase3ShieldDuration = 5f; // 盾牌持续时间
    public float phase3ShieldCooldown = 10f; // 盾牌冷却时间
    //public ParticleSystem phase3Effect;

    public GameObject doubleJump;
    private bool spawnItem=false;
    private float currentHealth;
    private bool isInRange;
    private float attackTimer;
    private float summonTimer;
    private float shieldTimer;
    private bool isShieldActive = false;
    private bool isTransitioning;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindFirstObjectByType<Player>().transform;
        capsuleCollider=GetComponent<CapsuleCollider2D>();

        currentHealth = enemyHealth.maxHealth;
    }

    void Update()
    {
        if (isTransitioning || player == null) return;
        currentHealth = enemyHealth.health;
        HandleSpriteFlip();
        HandlePhaseBehavior();
        if(currentHealth<=0&&spawnItem==false)
        {
            GameObject DoubleJump = Instantiate(doubleJump,transform.position,quaternion.identity);
            spawnItem = true;
        }
    }

    void HandleSpriteFlip()
    {
        // 翻转Sprite和attackCheck的位置
        bool shouldFlip = player.position.x > transform.position.x;
        UpdateAttackCheckPosition();
        UpdateSpriteScale(shouldFlip);
    }

    void HandlePhaseBehavior()
    {
        switch (phase)
        {
            case 1:
                Phase1Logic();
                break;
            case 2:
                Phase2Logic();
                break;
            case 3:
                Phase3Logic();
                break;
        }
    }

    void Phase1Logic()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        isInRange = distance <= stopDistance;

        if (!isInRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * GetCurrentSpeed();
            animator.SetBool("IsMoving", true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= phase1AttackCD && isInRange)
        {
            PerformSwipeAttack();
            attackTimer = 0f;
        }
    }

    void Phase2Logic()
    {
        summonTimer += Time.deltaTime;
        if (summonTimer >= summonInterval)
        {
            SummonImps();
            summonTimer = 0f;
        }
    }

    void Phase3Logic()
    {
        // 增加盾牌机制
        shieldTimer += Time.deltaTime;
        if (!isShieldActive && shieldTimer >= phase3ShieldCooldown)
        {
            ActivateShield();
            shieldTimer = 0f;
        }

        // 增加攻击速度和伤害
        attackTimer += Time.deltaTime;
        if (attackTimer >= (phase1AttackCD / phase3AttackMultiplier) && isInRange)
        {
            PerformEnhancedSwipe();
            attackTimer = 0f;
        }
        // 增加移动速度
        float distance = Vector2.Distance(transform.position, player.position);
        isInRange = distance <= stopDistance;

        if (!isInRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * GetCurrentSpeed() * phase3SpeedMultiplier;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    void PerformSwipeAttack()
    {
        animator.SetTrigger("Swipe");
    }

    void PerformEnhancedSwipe()
    {
        animator.SetTrigger("Swipe");
        //phase3Effect.Play();
        // 强化版攻击处理
    }

    void SummonImps()
    {
        animator.SetTrigger("Summon");
        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(impPrefab, transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isTransitioning || isShieldActive) return; // 如果正在过渡或盾牌激活，忽略伤害

        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        // 在第二阶段，每次受到攻击后调整 X 坐标
        if (phase == 2)
        {
            float targetX = player.position.x + phase2XOffset;
            float deltaX = targetX - transform.position.x;
            float maxSpeed = 10f; // 设置最大速度
            rb.velocity = new Vector2(Mathf.Clamp(deltaX / Time.deltaTime, -maxSpeed, maxSpeed), rb.velocity.y);
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(TransitionPhase());
        }
    }

    System.Collections.IEnumerator TransitionPhase()
    {
        isTransitioning = true;
        animator.SetTrigger("PhaseTransition");

        yield return new WaitForSeconds(phaseTransitionDuration);

        phase++;
        if (phase > 3) phase = 3;

        ApplyPhaseModifiers();
        currentHealth = maxHealth;
        isTransitioning = false;
    }

    void ApplyPhaseModifiers()
    {
        switch (phase)
        {
            case 2:
                float targetY = player.position.y + phase2YOffset;
                float deltaY = targetY - transform.position.y;
                rb.velocity = new Vector2(rb.velocity.x, deltaY / Time.deltaTime);
                // 初始化召唤阶段参数
                summonTimer = summonInterval;
                break;
            case 3:
                // 初始化盾牌机制
                shieldTimer = phase3ShieldCooldown;
                isShieldActive = false;
                //phase3Effect.Play();
                break;
        }
    }

    float GetCurrentSpeed()
    {
        return phase == 3 ? phase1MoveSpeed * phase3SpeedMultiplier : phase1MoveSpeed;
    }

  
    private void OnSwipeHit()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, LayerMask.GetMask("Player"));

        foreach (var hit in player)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Player>().GetDamage(swipeDamage);
            }
        }
    }

    private void ActivateShield()
    {
        isShieldActive = true;
        animator.SetTrigger("ReleaseShield");
        shieldEffect = Instantiate(shieldEffectPrefab,
            new Vector3(transform.position.x + 0.26f, transform.position.y - 1.48f, 0),
            Quaternion.identity); 
        Invoke("DeactivateShield", phase3ShieldDuration);
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        Destroy(shieldEffect); // 销毁盾牌效果
    }

    // Gizmos 显示攻击范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    private void UpdateAttackCheckPosition()
    {
        // 根据角色的翻转状态调整 attackCheck 的位置
        if (!spriteRenderer.flipX)
        {
            attackCheck.localPosition = new Vector3(-Mathf.Abs(attackCheck.localPosition.x), attackCheck.localPosition.y, attackCheck.localPosition.z);
        }
        else
        {
            attackCheck.localPosition = new Vector3(Mathf.Abs(attackCheck.localPosition.x), attackCheck.localPosition.y, attackCheck.localPosition.z);
        }
    }
    private void UpdateSpriteScale(bool shouldFlip)
    {
        // 更新角色的 localScale 以实现左右翻转
        Vector3 newScale = transform.localScale;
        newScale.x = shouldFlip ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }
}
