using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveLoadPanel : MonoBehaviour
{
    private GameObject panel;
    public GameObject slot;
    public TextMeshProUGUI textMeshPro;
    private void Start()
    {
        panel = gameObject;
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        // ��ȡ��ǰ����������
        string currentSceneName = SceneManager.GetActiveScene().name;
        textMeshPro.text = currentSceneName;
    }
    private void Update()
    {
        if (panel.active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                slot.SetActive(false);
                Time.timeScale = 1;
                Player.Instance.enabled = true;
                panel.SetActive(false);
            }
        }
    }
}
