using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Text healtehText;
    public static float currentHealth;
    public static float maxHealth;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth<0)
        {
            currentHealth = 0;
        }
        image.fillAmount = currentHealth/maxHealth;
        healtehText.text = currentHealth.ToString();
    }
}
