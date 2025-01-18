using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public Transform player; // ��Ҷ����Transform���
    public float cooldown;//�����ӵ����ʱ��
    private float cooldownTimer;//��¼��ȴʱ��
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cooldownTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            GameObject b1= Instantiate(bullet, transform.position, Quaternion.identity);
            Vector3 direction = player.position - transform.position;//����Archer����ҵ�����
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//����Ƕȣ��Զ�Ϊ��λ��
            b1.GetComponent<Bullet1>().angle = angle;
            cooldownTimer = cooldown;
        }
    }
}
