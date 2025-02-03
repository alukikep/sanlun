using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public ObjectPool objectPool;
    public Transform player;
    public float spawnDistance;
    public float despawnDistance;
    public float respawnTime;

    private GameObject currentMonster;
    private float lastDespawnTime;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentMonster==null)
        {
            if(!isDead||Time.time - lastDespawnTime > respawnTime)
            {
                if(distanceToPlayer < spawnDistance)
                {
                    SpawnMonster();
                }
            }
        }
        else
        {
            if (distanceToPlayer >= despawnDistance)
            {
                DespawnMonster();
            }
        }
    }

    private void SpawnMonster()
    {
        currentMonster = objectPool.GetMonster();
        currentMonster.transform.position = transform.position;
        currentMonster.GetComponent<Enemy1>().OnDeath += OnMonsterDeath;
        isDead=false;
    }

    private void DespawnMonster()
    {
        objectPool.ReturnMonster(currentMonster);
        currentMonster.GetComponent<Enemy1>().OnDeath -= OnMonsterDeath;
        currentMonster = null;
        lastDespawnTime = Time.time;
    }

    private void OnMonsterDeath()
    {
        currentMonster = null;
        isDead = true;
        lastDespawnTime = Time.time;
    }
}
