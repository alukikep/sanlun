using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject monster;
    public int poolSize=10;

    private List<GameObject> pool;
    // Start is called before the first frame update
    void Awake()
    {
        pool = new List<GameObject>();
        for(int i = 0; i < poolSize; i++)
        {
            GameObject Monster = Instantiate(monster);
            Monster.SetActive(false);
            pool.Add(Monster);
        }       
    }

    public GameObject GetMonster()
    {
        foreach(GameObject Monster in pool)
        {
            if(!Monster.activeInHierarchy)
            { 
                Monster.SetActive(true);
                return Monster;
            }
        }
        GameObject newMonster = Instantiate(monster);
        pool.Add(newMonster);
        return newMonster;
            
    }

    public void ReturnMonster(GameObject monster)
    {
        monster.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
