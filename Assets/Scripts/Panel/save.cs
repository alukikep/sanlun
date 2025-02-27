using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class save : MonoBehaviour
{
    public static save Instance;
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this; // 确保单例引用正确
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}