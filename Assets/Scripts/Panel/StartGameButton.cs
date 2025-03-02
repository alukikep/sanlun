using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public GameObject Inventory;

    public void StartGame()
    {

        SceneManager.LoadScene("CastleHall");
        GameObject[] allObj = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach(GameObject obj in allObj)
        {
            if(obj.name =="Player")
            {
               obj.SetActive(true);
                obj.transform.position = new Vector3(88.60001f, -25.07f, 0.04504037f);
                GameObject PauMenu = GameObject.Find("PauseMenu");
                if (PauMenu != null)
                {   
                    
                     PauMenu.SetActive(false);
                    Time.timeScale = 1f;
                }

                  
            }
        }
        Inventory.SetActive(true);
    }
   
}
