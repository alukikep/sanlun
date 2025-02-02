using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour//一个简单的代码实现移动平台
{
    public float horizonRange;
    public float verticalRange;
    public float movingTime;
    private float movingTimer;
    private bool isMovingRight;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingTimer = movingTime; // 初始化 movingTimer
    }
    private void Update()
    {
        moving();
    }
    private void moving()
    {
        float speed = Mathf.Sqrt(horizonRange * horizonRange + verticalRange * verticalRange) / movingTime;        
        movingTimer -= Time.deltaTime;
        if(isMovingRight == true)
        {
            rb.velocity = new Vector3(horizonRange / movingTime,  verticalRange / movingTime).normalized * speed;
            if (movingTimer <= 0)
            {
                movingTimer = movingTime;
                isMovingRight = false;
            }
            
        }
        if (isMovingRight == false)
        {
            rb.velocity = new Vector3(-horizonRange / movingTime, -verticalRange / movingTime).normalized * speed;
            if (movingTimer <= 0)
            {
                movingTimer = movingTime;
                isMovingRight = true;
            }

        }

    }
}
