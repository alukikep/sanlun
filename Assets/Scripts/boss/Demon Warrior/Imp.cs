using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Imp : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 3f;      // �����ƶ��ٶ�
    public float acceleration = 5f;   // �ƶ����ٶ�
    public float stoppingDistance = 1f; // ����ұ��־���

    [Header("��������")]
    public float fireballDamage = 10f;
    public float attackInterval = 3f;
    public float projectileSpeed = 8f;
    public GameObject fireballPrefab;

    [Header("��������")]
    public int maxHealth = 2;
    public float deathEffectDuration = 0.5f;
    public GameObject deathParticle;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private float attackTimer;
    private int currentHealth;

    void Start()
    {
        player = Player.Instance.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth =maxHealth;

        // ��̬���ɾ�������
        CreateDetectionZone();
    }

    void Update()
    {
        if (player == null) return;

        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float currentDistance = Vector2.Distance(transform.position, player.position);

        // ��̬�ٶ�����
        float speedModifier = Mathf.Clamp(
            currentDistance / stoppingDistance,
            0.5f,
            1.5f
        );

        // ƽ�������ƶ�
        rb.velocity = Vector2.Lerp(
            rb.velocity,
            dirToPlayer * moveSpeed * speedModifier,
            acceleration * Time.deltaTime
        );

        // ��ת����
        if (dirToPlayer.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void HandleAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            anim.SetTrigger("Attack");
            attackTimer = 0f;
        }
    }

    // �����¼����õĹ�������
    public void LaunchProjectile()
    {
        Vector2 fireDirection = (player.position - transform.position).normalized;

        GameObject fireball = Instantiate(
            fireballPrefab,
            transform.position + (Vector3)fireDirection * 0.5f,
            Quaternion.identity
        );

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

    IEnumerator DeathSequence()
    {
        anim.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(deathEffectDuration);

        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // ���ɶ�̬��������
    void CreateDetectionZone()
    {
        GameObject zone = new GameObject("DetectionZone");
        zone.transform.SetParent(transform);
        zone.transform.localPosition = Vector3.zero;

        CircleCollider2D col = zone.AddComponent<CircleCollider2D>();
        col.radius = 5f;
        col.isTrigger = true;

        zone.AddComponent<ImpDetection>();
    }
}

// ����������ű�
public class ImpDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Imp>().enabled = true;
        }
    }
}
