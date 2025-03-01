using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SaveLoadPanel : MonoBehaviour
{
    private GameObject panel;
    public TextMeshProUGUI textMeshPro;
    public ToggleGroup toggleGroup; // 需要操作的 ToggleGroup
    private bool lastState = false; // 上一次的状态

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
                Time.timeScale = 1;
                Player.Instance.enabled = true;
                panel.SetActive(false);
            }
        }
        // 如果状态从不可见变为可见，调用 SetAllTogglesOff 方法
        if (!lastState && gameObject.activeSelf)
        {
            DisableAllToggles();
        }

        // 更新状态
        lastState = gameObject.activeSelf;
    }
    private void DisableAllToggles()
    {
        // 禁用所有 Toggle
        toggleGroup.SetAllTogglesOff();
    }
}
