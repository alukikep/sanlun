using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : MonoBehaviour
{
    public float damage;
    public float speed;
    public float radius;
    public float totalAngle;
    private float initialAngle;
    private Transform Player;
    private Rigidbody2D rb;
    private void Start()
    {
        Player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        Vector2 direction = (transform.position - Player.transform.position);
        initialAngle = Mathf.Atan2(direction.y, direction.x);
    }
    private void Update()
    {
        rotate();
       
    }
    public void rotate()
    {
        float angle = Time.deltaTime * speed; 
        totalAngle += angle; 
        float x = Player.position.x+radius*Mathf.Cos(initialAngle+totalAngle);
        float y = Player.position.y+radius*Mathf.Sin(initialAngle+totalAngle);
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
