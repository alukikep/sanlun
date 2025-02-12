using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubWeaponUI : MonoBehaviour
{
    [SerializeField] private Sprite axe;
    [SerializeField] private Sprite guard;
    [SerializeField] private Sprite clock;
    [SerializeField] private Image image;
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
       image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(player.isAxe==true)
        {
            image.sprite = axe;
        }
        if(player.isGuardian==true)
        {
            image.sprite = guard;
        }
        if(player.isTimeSlowed==true)
        {
            image.sprite=clock;
        }
    }
}
