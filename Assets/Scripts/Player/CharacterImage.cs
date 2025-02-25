using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImage : MonoBehaviour
{
    public Sprite normal;
    public Sprite dangerous;
    private Image image;
    private Player player;


    private void Start()
    {
        player = GetComponentInParent<Player>();
       image = GetComponent<Image>();
        image.sprite = normal;
    }

    private void Update()
    {
        if(player.health/player.maxHealth<0.5)
        {
            image.sprite = dangerous;
        }
        else
        {
            image.sprite = normal;
        }
    }
}

