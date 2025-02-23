using UnityEngine;

//���Ҽ��д���һ���µĲ˵�
[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
//�ɽű���������һ�����������������ڱ���������ݣ����������ʵ��
public class ItemData : ScriptableObject 
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType; 
    public int healAmount;    // ����������������ҩˮ��Ч
    public int increaseHealthMax; // ��������ֵ����
    public int increaseAttack;    // ���ӹ�����
    public int speedEnhanceRate;
    public int Duration;


    // ��Ʒ����ö��
    public enum ItemType
    {
        None,
        HealingPotion,
        HealthMaxPotion,
        EnternalAttackPotion,
        TemporaryAttackPotion,
        SpeedPotion
    }
}
