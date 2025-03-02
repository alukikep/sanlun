using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public GameObject Inventory;
    private GameObject playerParent;
    private void Start()
    {
        playerParent = GameObject.Find("PlayerParent");
    }
    public void StartGame()
    {

        SceneManager.LoadScene("CastleHall");
        GameObject[] allObj = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach(GameObject obj in allObj)
        {
            if(obj.name =="Player")
            {
                obj.SetActive(true);
              
            }
        }
        Inventory.SetActive(true);
    }
   
}
