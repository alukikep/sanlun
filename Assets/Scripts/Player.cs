using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speedRate; // 速率系数
    public float jumpForce; // 跳跃高度

    private Rigidbody2D rigidbody2D;
    private float xSpeed;
    private int jumpNumber = 0; // 0,1,2分别表示跳跃了0，1，2次，控制二段跳

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xSpeed = Input.GetAxisRaw("Horizontal");
        // 按住shift速度翻倍
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            xSpeed *= 2;
        }
        rigidbody2D.velocity = new Vector2(xSpeed * speedRate, rigidbody2D.velocity.y);

        // 跳跃
        if (Input.GetButtonDown("Jump") && jumpNumber < 2)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpNumber++;
        }
    }

    // 当玩家与标签为“ground”的地面接触后，重置跳跃次数
    //OnCollisionEnter2D适用于两者都不是trigger的情况，后续可能需要改成OnTriggerEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpNumber = 0;
        }
    }
}