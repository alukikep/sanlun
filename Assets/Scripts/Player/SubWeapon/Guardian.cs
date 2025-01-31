using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : MonoBehaviour
{
    public float damage;
    public float speed;
    public float radius;
    public float totalAngle;
    public float time;
    private Transform Player;
    private Rigidbody2D rb;
    private void Start()
    {
        Player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        time = 2 * Mathf.PI / (speed * Mathf.Deg2Rad);
    }
    private void Update()
    {
        rotate();
       
    }
    public void rotate()
    {
        float angle = Time.deltaTime * speed; 
        totalAngle += angle; 
        Vector3 Pos = Player.position + new Vector3(Mathf.Cos(totalAngle) * radius, Mathf.Sin(totalAngle) * radius, 0);
        transform.position = Pos;
    }
}
