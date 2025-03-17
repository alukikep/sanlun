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
    [Header("�ƶ�����")]
    public float moveSpeed = 3f;          // �ƶ��ٶ�
    public float acceleration = 5f;       // ���ٶ�
    public float retreatDistance = 5f;   // ֹͣ����

    [Header("��������")]
    public float fireballDamage = 10f;    // �����˺�
    public float attackInterval = 3f;     // �������
    public float projectileSpeed = 8f;    // �����ٶ�
    public GameObject fireballPrefab;     // ����Ԥ����

    public float LifeTime;
    private EnemyHealth enemyHealth;
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private float attackTimer;
    private float currentHealth;
    private ImpState currentState = ImpState.Idle;


    void Start()
    {
        player = Player.Instance.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        currentHealth = enemyHealth.maxHealth;

        ChangeState(ImpState.Idle);
        CreateDetectionZone();
    }

    void Update()
    {
        currentHealth = enemyHealth.health;
        if(currentHealth<1)
        {
            Destroy(gameObject);
        }
        if(LifeTime<0)
        {
            Destroy(gameObject);
        }
        LifeTime-=Time.deltaTime;
        if (player == null) return;
        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        HandleAttack();
        HandleMovement(dirToPlayer);

        // ���³���
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
        // ����ʱ���߼����������������һЩ�����Ϊ��
        ChangeState(ImpState.Moving);
    }

    private void HandleMovement(Vector2 vector2)
    {
        float currentDistance = Vector2.Distance(transform.position, player.position);

        // �����Ҿ���С�� retreatDistance��Զ�����
        if (currentDistance < retreatDistance)
        {
            rb.velocity = -vector2 * moveSpeed; // Զ�����
        }
        else
        {
            rb.velocity = Vector2.zero; // ֹͣ�ƶ�
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


    public void LaunchProjectile()
    {
        Vector2 fireDirection = (player.position - transform.position).normalized;
        Vector3 spawnPosition = transform.position + (Vector3)fireDirection * 0.5f;

        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
        Fireball fb = fireball.GetComponent<Fireball>();
        fb.Initialize(fireDirection, projectileSpeed, fireballDamage);
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