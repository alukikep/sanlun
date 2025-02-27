using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    public GameObject SaveButton;
    public GameObject LoadButton;
    private void Awake()
    {
        SaveButton.SetActive(false);
        LoadButton.SetActive(false);
    }
    public void Activate()
    {
        SaveButton.SetActive(true);
        LoadButton.SetActive(true);
    }
}
