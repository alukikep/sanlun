using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Z;
    public float interactRange;
    public GameObject[] bricksToDesTroy;
    private bool isPlayerNearBy = false;

    private void Update()
    {
        CheckPlayer();
        if(isPlayerNearBy&&Input.GetKeyDown(interactKey))
        {
            DestroyBricks();
        }
    }
    private void CheckPlayer()
    {
        Collider2D[] Player = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach(var hit in Player)
        {
            if(hit.CompareTag("Player"))
            {
                isPlayerNearBy = true;
            }
        }

    }
    private void DestroyBricks()
    {
        if(bricksToDesTroy!=null)
        {
            foreach(GameObject brick in bricksToDesTroy)
            {
                if(brick!=null)
                {
                    Destroy(brick);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
