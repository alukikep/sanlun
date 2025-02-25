using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    
    public ItemData ItemData1;
    private bool isProcessed = false; // ���һ�����
    public string potionID;

    private void Awake()
    {
        // ʹ��λ�ú���������Ψһ��ʶ��
        potionID = $"{ItemData1.itemName}_{transform.position.x}_{transform.position.y}";
    }
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite =ItemData1.icon;
        gameObject.name ="Item object-"+ ItemData1.itemName;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null&& !isProcessed)
        {
            isProcessed = true;
            Player.Instance.AddCollectedPotion(potionID);
            if (ItemData1.itemType == ItemData.ItemType.HealthMaxPotion || ItemData1.itemType == ItemData.ItemType.EnternalAttackPotion|| ItemData1.itemType == ItemData.ItemType.ManaCapacityPotion)
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
