using UnityEngine;

//在右键中创建一个新的菜单
[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
//可脚本化对象是一个数据容器，可用于保存大量数据，独立于类的实例
public class ItemData : ScriptableObject 
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType; 
    public int healAmount;    // 治疗量，仅对治疗药水有效
    public int increaseHealthMax; // 增加生命值上限
    public int increaseAttack;    // 增加攻击力
    public int speedEnhanceRate;
    public int Duration;


    // 物品类型枚举
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
