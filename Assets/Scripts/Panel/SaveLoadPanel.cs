using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SaveLoadPanel : MonoBehaviour
{
    private GameObject panel;
    public GameObject slot;
    public TextMeshProUGUI textMeshPro;
    public List<Button> buttons = new List<Button>();

    private void Start()
    {
        panel = gameObject;
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        // 获取当前场景的名称
        string currentSceneName = SceneManager.GetActiveScene().name;
        textMeshPro.text = currentSceneName;
    }
    private void Update()
    {
        if (panel.active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Deactivate();
                slot.SetActive(false);
                Time.timeScale = 1;
                Player.Instance.enabled = true;
                panel.SetActive(false);
            }
        }
    }
    public void Deactivate()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
