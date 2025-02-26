using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePanel : MonoBehaviour
{
    public GameObject panel;
    public GameObject checkPanel;
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
