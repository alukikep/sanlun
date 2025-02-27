using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ArcherChase : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public Transform player; // ��Ҷ����Transform���
    public float cooldown;//�����ӵ����ʱ��
    private float cooldownTimer;//��¼��ȴʱ��
    public GameObject bullet;
    private Animator animator;
    public Player _player;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cooldownTimer = 1;
        animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0 && player != null)
        {
            animator.Play("Attack");
        }
    }

    private void Shoot()
    {
        GameObject b1 = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector3 direction = player.position - transform.position;//����Archer����ҵ�����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//����Ƕȣ��Զ�Ϊ��λ��
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        b1.transform.rotation = rotation;
        b1.GetComponent<ChaseBullet>().angle = angle;
        cooldownTimer = cooldown;    
            _player.audioController.PlaySfx(_player.audioController.lightningSkeletonAttack);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float ATK = 20;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetDamage(ATK);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
