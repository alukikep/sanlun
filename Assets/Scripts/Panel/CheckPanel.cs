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
        // 获取当前物体的子物体
        textMeshPro = text.GetComponent<TextMeshProUGUI>();
        // 获取当前场景的名称
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
