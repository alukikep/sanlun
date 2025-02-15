using System.Collections;
using UnityEngine;
public enum ImpState
{
    Idle,
    Moving,
    Attacking,
    Dying
}

public class Imp : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 3f;          // 移动速度
    public float acceleration = 5f;       // 加速度
    public float retreatDistance = 5f;   // 停止距离

    [Header("攻击参数")]
    public float fireballDamage = 10f;    // 火球伤害
    public float attackInterval = 3f;     // 攻击间隔
    public float projectileSpeed = 8f;    // 火球速度
    public GameObject fireballPrefab;     // 火球预制体

    [Header("生命参数")]
    public int maxHealth = 2;             // 最大生命值
    public float deathEffectDuration = 0.5f; // 死亡效果持续时间
    public GameObject deathParticle;      // 死亡特效

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private float attackTimer;
    private int currentHealth;
    private ImpState currentState = ImpState.Idle;


    void Start()
    {
        player = Player.Instance.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        ChangeState(ImpState.Idle);
        CreateDetectionZone();
    }

    void Update()
    {
        if (player == null) return;
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        switch (currentState)
        {
            case ImpState.Idle:
                HandleIdle();
                break;
            case ImpState.Moving:
                HandleMovement(dirToPlayer);
                break;
            case ImpState.Attacking:
                HandleAttack();
                break;
            case ImpState.Dying:
                HandleDeath();
                break;
        }

        // 更新朝向
        if (dirToPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void HandleIdle()
    {
        // 空闲时的逻辑（可以在这里添加一些随机行为）
        ChangeState(ImpState.Moving);
    }

    private void HandleMovement(Vector2 vector2)
    {
        float currentDistance = Vector2.Distance(transform.position, player.position);

        // 如果玩家距离小于 retreatDistance，远离玩家
        if (currentDistance < retreatDistance)
        {
            rb.velocity = -vector2 * moveSpeed; // 远离玩家
        }
        else
        {
            rb.velocity = Vector2.zero; // 停止移动
        }

    }

    private void HandleAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
            LaunchProjectile();
        }
    }

    private void HandleDeath()
    {
        // 死亡逻辑
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;

        if (deathParticle != null)
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, deathEffectDuration);
    }

    public void LaunchProjectile()
    {
        Vector2 fireDirection = (player.position - transform.position).normalized;
        Vector3 spawnPosition = transform.position + (Vector3)fireDirection * 0.5f;

        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
        Fireball fb = fireball.GetComponent<Fireball>();
        fb.Initialize(fireDirection, projectileSpeed, fireballDamage);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        ChangeState(ImpState.Dying);
        anim.SetTrigger("Die");

        yield return new WaitForSeconds(deathEffectDuration);
    }

    public void ChangeState(ImpState newState)
    {
        currentState = newState;
        anim.SetInteger("State", (int)newState);
    }

    private void CreateDetectionZone()
    {
        GameObject zone = new GameObject("DetectionZone");
        zone.transform.SetParent(transform);
        zone.transform.localPosition = Vector3.zero;

        CircleCollider2D col = zone.AddComponent<CircleCollider2D>();
        col.radius = 10f;
        col.isTrigger = true;

        zone.AddComponent<ImpDetection>();
    }
}

public class ImpDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Imp>().enabled = true;
            GetComponentInParent<Imp>().ChangeState(ImpState.Moving);
        }
    }
}