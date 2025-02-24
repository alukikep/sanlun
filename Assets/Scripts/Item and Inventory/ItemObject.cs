using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
   
    [SerializeField] private ItemData ItemData1;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite =ItemData1.icon;
        gameObject.name ="Item object-"+ ItemData1.itemName;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (ItemData1.itemType == ItemData.ItemType.HealthMaxPotion || ItemData1.itemType == ItemData.ItemType.EnternalAttackPotion)
            {
                Inventory.Instance.AddItem(ItemData1);
                Inventory.Instance.UseItem(ItemData1);
                Destroy(gameObject);
            }
            else
            {
                Inventory.Instance.AddItem(ItemData1);
                Destroy(gameObject);
            }
        }
    }
}
