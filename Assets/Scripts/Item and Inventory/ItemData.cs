using UnityEngine;

//���Ҽ��д���һ���µĲ˵�
[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
//�ɽű���������һ�����������������ڱ���������ݣ����������ʵ��
public class ItemData : ScriptableObject 
{
    public string itemName;
    public Sprite icon;
    public E_ItemType itemType; 
    public int healAmount;
    public int increaseHealthMax;
    public int increaseAttack;
    public int increaseManaMax;
    public int increaseMana;
    public int Duration;

    // ��Ʒ����ö��
    public enum E_ItemType
    {
        None,
        HealingPotion,
        HealthMaxPotion,
        EnternalAttackPotion,
        TemporaryAttackPotion,
        ManaCapacityPotion,
        ManaRestorePotion
    }
}
