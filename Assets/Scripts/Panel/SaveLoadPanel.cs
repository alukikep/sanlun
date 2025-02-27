using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveLoadPanel : MonoBehaviour
{
    private GameObject panel;
    public GameObject checkPanel;
    private void Start()
    {
        panel = gameObject;
        
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
