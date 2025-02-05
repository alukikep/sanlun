using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{ 
  private Rigidbody2D rigidbody2D;

[SerializeField] private float moveSpeed;
private float destroyTimer = 10;//×Ô»ÙÊ±¼ä
public float ATK;
    private GameObject player;
// Start is called before the first frame update
void Start()
{
        player = GameObject.FindWithTag("Player");
        float movingDir = player.transform.position.x > transform.position.x ? 1 : -1;
    rigidbody2D = GetComponent<Rigidbody2D>();
    rigidbody2D.velocity =new Vector2(moveSpeed*movingDir, 0);
}

// Update is called once per frame
void Update()
{
    destroyTimer -= Time.deltaTime;
    if (destroyTimer < 0)
    {
        Destroy(gameObject);
    }
}
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        Destroy(gameObject);
    }
    if (collision.gameObject.CompareTag("Player"))
    {
        collision.gameObject.GetComponent<Player>().GetDamage(ATK);
        Destroy(gameObject);
    }
}
}

