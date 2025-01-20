using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy2 : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public Transform player;

    [SerializeField] private float wanderRadius;// �����ε��ķ�Χ
    [SerializeField] private float moveSpeed;// �����ε����ٶ�
    private float leftBoundary;
    private float rightBoundary;
    private bool isMovingRight = true; // ���ڼ�¼��ǰ�ƶ�����

    public float cooldown;//�����ӵ����ʱ��
    private float cooldownTimer;//��¼��ȴʱ��
    public GameObject bullet;
    private GameObject[] bullets = new GameObject[4];//��¼4���ӵ�
    private int[] angles = { 45, 135, 225, 315 }; // Ԥ�ȼ�����ӵ��Ƕ�

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cooldownTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        leftBoundary = player.position.x - wanderRadius;
        rightBoundary = player.position.x + wanderRadius;
        // ����Ƿ񵽴�߽粢��ת����
        if (transform.position.x <= leftBoundary)
        {
            isMovingRight = true;
        }
        else if (transform.position.x >= rightBoundary)
        {
            isMovingRight = false;
        }
        // ���ݵ�ǰ���������ٶ�
        float direction = isMovingRight ? 1 : -1;
        rigidbody2D.velocity = new Vector2(direction * moveSpeed,0);

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i]= Instantiate(bullet, transform.position, Quaternion.identity);
                bullets[i].GetComponent<Bullet1>().angle = angles[i];
            }
            cooldownTimer = cooldown;
        }
    }
}
