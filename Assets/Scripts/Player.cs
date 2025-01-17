using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speedRate; // ����ϵ��
    public float jumpForce; // ��Ծ�߶�
    public bool isBat;

    private Rigidbody2D rigidbody2D;
    private float xSpeed;
    private int jumpNumber = 0; // 0,1,2�ֱ��ʾ��Ծ��0��1��2�Σ����ƶ�����

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        isBat = false;
    }

    // Update is called once per frame
    void Update()
    {
        xSpeed = Input.GetAxisRaw("Horizontal");
        // ��סshift�ٶȷ���
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            xSpeed *= 2;
        }
        rigidbody2D.velocity = new Vector2(xSpeed * speedRate, rigidbody2D.velocity.y);

        // ��Ծ
        if (Input.GetButtonDown("Jump") && jumpNumber < 2)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpNumber++;
        }
        //������̬�����裩
        if (Input.GetKey(KeyCode.R) && isBat ==false)
        {
            rigidbody2D.drag = 10;
            isBat = true;
            jumpNumber=2;
        }
        //������סr��������ʹ�����ٶȱ����Ҳ�������Ծ
        if (Input.GetKeyUp(KeyCode.R) && isBat == true)
        {
            rigidbody2D.drag = 1;
            isBat = false;
        }
        //�ɿ�r��ԭ

    }

    // ��������ǩΪ��ground���ĵ���Ӵ���������Ծ����
    //OnCollisionEnter2D���������߶�����trigger�����������������Ҫ�ĳ�OnTriggerEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpNumber = 0;
        }
    }
}