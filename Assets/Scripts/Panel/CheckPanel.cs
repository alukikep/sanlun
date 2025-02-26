using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPanel : MonoBehaviour
{
    public GameObject panel;
    public GameObject savePanel;
    public GameObject loadPanel;
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
