using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Dracula : MonoBehaviour
{
    public Vector2 areaCenter;
    public Transform player;
    public Vector2 areaSize;
    public bool faceRight;
    [Header("×Óµ¯")]
    public GameObject fireBall;
    public GameObject hugeFireBall;
    public GameObject lightRain;

    [Header("»ðÇò¼ä¾à")]
    [SerializeField]private float littleSpacing;
    [SerializeField] private float hugeSpacing;
    [SerializeField]private float lightRainSpacing;

    private void Update()
    {
        Flip();

        if(Input.GetKeyDown(KeyCode.O))
        {
            FireBall();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            HugeFireBalluUP();
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            HugeFireBallDown();
        }
        if(Input.GetKeyDown (KeyCode.L))
        {
            LightRain();
        }
    }

    public void TeleportToRandomPosition()
    {
        float randomX = Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2);
        float randomZ = Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2);
        float currentY = transform.position.y;
        Vector3 randomPosition = new Vector3(randomX, currentY, randomZ);
        transform.position = randomPosition;
    }

    public void FireBall()
    {
        if (player == null || fireBall == null)
        {
            Debug.Log("Player or fireball prefab is not assigned!");
            return;
        }
        Vector3 space = new Vector3(2,littleSpacing,0);
        Vector3 middleSpace = new Vector3(2, 0, 0);
        Vector3 direction = new Vector3(faceRight?1:-1,0,0);
        Vector3 up = transform.position +space;
        Vector3 middle  = transform.position +middleSpace;
        Vector3 down = transform.position -space+2*middleSpace;

        GameObject fireball1 = Instantiate(fireBall, up,Quaternion.Euler(direction));
        GameObject fireball2 = Instantiate(fireBall, middle, Quaternion.Euler(direction));
        GameObject fireball3 = Instantiate(fireBall, down, Quaternion.Euler(direction));

    }

    public void HugeFireBalluUP()
    {
        if (player == null || hugeFireBall == null)
        {
            Debug.Log("Player or fireball prefab is not assigned!");
            return;
        }
        Vector3 space = new Vector3(0, hugeSpacing, 0);
        Vector3 direction = new Vector3(faceRight ? 1 : -1, 0, 0);
        Vector3 up = transform.position+space;

        GameObject HugeFireBall= Instantiate(hugeFireBall, up, Quaternion.Euler(direction));

    }

    public void HugeFireBallDown()
    {

        if (player == null || hugeFireBall == null)
        {
            Debug.Log("Player or fireball prefab is not assigned!");
            return;
        }
        Vector3 space = new Vector3(0, -hugeSpacing, 0);
        Vector3 direction = new Vector3(faceRight ? 1 : -1, 0, 0);
        Vector3 down = transform.position + space;

        GameObject HugeFireBall = Instantiate(hugeFireBall, down, Quaternion.Euler(direction));
    }

    public void LightRain ()
    {
        if (player == null || lightRain == null)
        {
            Debug.Log("Player or fireball prefab is not assigned!");
            return;
        }
        Vector3 space = new Vector3 (lightRainSpacing, 0, 0);
        Vector3 height = new Vector3(0, 3, 0);
        Vector3 startPos = transform.position-8*space+height;
        for (int i = 0; i < 16; i++)
        {
            Vector3 setPos = startPos + space * i;
            GameObject LightRain = Instantiate(lightRain, setPos,Quaternion.identity);
        }

    }

    private void Flip()
    {
        if(player.position.x>transform.position.x&&faceRight==false)
        {
            transform.Rotate(0, 180, 0);
            faceRight = true;
        }
        if(player.position.x < transform.position.x&&faceRight==true)
        {
            transform.Rotate(0,180,0);
            faceRight = false;
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
