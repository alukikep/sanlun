using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBricks : MonoBehaviour
{
    public float health;

    public void GetDamage(float ATk)
    {
        health -= ATk;
        if(health<=0)
        {
            Destroy(gameObject);
        }
    }
}
