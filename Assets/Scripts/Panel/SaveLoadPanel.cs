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
    public ToggleGroup toggleGroup; // ��Ҫ������ ToggleGroup
    private bool lastState = false; // ��һ�ε�״̬

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
                Time.timeScale = 1;
                Player.Instance.enabled = true;
                panel.SetActive(false);
            }
        }
        // ���״̬�Ӳ��ɼ���Ϊ�ɼ������� SetAllTogglesOff ����
        if (!lastState && gameObject.activeSelf)
        {
            DisableAllToggles();
        }

        // ����״̬
        lastState = gameObject.activeSelf;
    }
    private void DisableAllToggles()
    {
        // �������� Toggle
        toggleGroup.SetAllTogglesOff();
    }
}
