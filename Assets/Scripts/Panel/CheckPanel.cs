using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CheckPanel : MonoBehaviour
{
    private GameObject panel;
    public GameObject savePanel;
    public GameObject loadPanel;
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
        ExitPanel();
    }
    private void ExitPanel()
    {
        if (panel.active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                Player.Instance.enabled = true;
                panel.SetActive(false);
            }
        }
    }
    public void SaveButton()
    {
        panel.SetActive(false);
        savePanel.SetActive(true);
    }
    public void LoadButton()
    {
        panel.SetActive(false);
        loadPanel.SetActive(true);
    }
}
