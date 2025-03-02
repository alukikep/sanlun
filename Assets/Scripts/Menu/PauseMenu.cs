using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private Player player;
    public GameObject _player;
    public GameObject inventory;
    public void Start()
    {
        player = GetComponentInParent<Player>();
        inventory= GameObject.Find("Inventory");
        _player = GameObject.Find("Player");
    }
    public void Continue()
    {
        player.pauseMenu.SetActive(false);
        player.isPauseMenuEnabled = false;
        Time.timeScale = 1;
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("StartMenu");
        _player.SetActive(false);
        inventory.SetActive(false);
    }
}
