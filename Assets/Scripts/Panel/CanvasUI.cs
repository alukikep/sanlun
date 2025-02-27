using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    public static CanvasUI Instance;
    internal RenderMode renderMode;
    internal Camera worldCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // ȷ������������ȷ
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
