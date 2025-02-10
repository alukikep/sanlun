using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public ObjectPool objectPool;
    public Transform player;
    private GameObject _Player;
    public float spawnDistance;
    public float despawnDistance;
    public float respawnTime;
    public float Radius;

    private GameObject currentMonster;
    private float lastDespawnTime;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        _Player = GameObject.Find("Player");
       
    }

    // Update is called once per frame
    void Update()
    {
        player = _Player.transform;
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
        currentMonster.GetComponent<EnemyHealth>().OnDeath += OnMonsterDeath;
        isDead=false;
    }

    private void DespawnMonster()
    {
        objectPool.ReturnMonster(currentMonster);
        currentMonster.GetComponent<EnemyHealth>().OnDeath -= OnMonsterDeath;
        currentMonster = null;
        lastDespawnTime = Time.time;
    }

    private void OnMonsterDeath()
    {
        if (currentMonster != null)
        {
            float maxHealth = currentMonster.GetComponent<EnemyHealth>().maxHealth;
            currentMonster.GetComponent<EnemyHealth>().health = maxHealth;
            currentMonster.SetActive(false);
            currentMonster = null;
            isDead = true;
            lastDespawnTime = Time.time;
        }
        else
        {
            return;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
