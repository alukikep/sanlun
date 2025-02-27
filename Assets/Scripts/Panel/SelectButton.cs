using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    public GameObject SaveButton;
    public GameObject LoadButton;
    public SaveLoadPanel saveLoadPanel;
    public Image recordimage;

    private void Start()
    {
        saveLoadPanel= gameObject.GetComponentInParent<SaveLoadPanel>();
    }
    public void Activate()
    {
        if (recordimage != null)
        {
            Slot.Instance.image = recordimage;
        }
        saveLoadPanel.Deactivate();
        SaveButton.SetActive(true);
        LoadButton.SetActive(true);
    }
}
