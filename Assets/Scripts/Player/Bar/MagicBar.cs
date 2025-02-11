using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    public Text magicText;
    public static int currentMagic;
    public static float maxMagic;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = currentMagic / maxMagic;
        magicText.text = currentMagic.ToString();
    }
}
