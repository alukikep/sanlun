using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float health;
    public void GetDamage(float pATK)
    {
        health = health - pATK;
    }

    public void Update()
    {
        if(health<0)
        {
            Destroy(gameObject);
        }
    }


}
