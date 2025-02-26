using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveLoadPanel : MonoBehaviour
{
    private GameObject panel;
    public GameObject checkPanel;
    public GameObject text;
    private TextMeshProUGUI textMeshPro;
    private void Start()
    {
        panel = gameObject;
        // ��ȡ��ǰ�����������
        textMeshPro = text.GetComponent<TextMeshProUGUI>();
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
                panel.SetActive(false);
                checkPanel.SetActive(true);
            }
        }
    }
}
