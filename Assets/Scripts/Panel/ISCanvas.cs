using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISCanvas : MonoBehaviour
{
    public static ISCanvas Instance;
    private GameObject gameObject;
    public GameObject inventory;
    public GameObject savepanel;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 确保单例引用正确
        }
        else
        {
            Destroy(base.gameObject);
        }
        DontDestroyOnLoad(base.gameObject);
    }
    private void Start()
    {
        gameObject = base.gameObject;
    }
    public void ActivateInventory()
    {
        inventory.SetActive(true);
    }
    public void ActivateSavepanel()
    {
        savepanel.SetActive(true);
    }
    public void DeactivateSavepanel()
    {
        savepanel.SetActive(false);
    }
}