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
        Inventory.SetActive(true);
    }
}
