using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public float health;
    public float currentMana;
    public Vector3 position;
    public bool isDoubleJumpEnabled;
    public bool isHighJumpEnabled;
    public bool isBatTransformEnabled;
    public bool isRatTransformEnabled;
    public bool isAxeEnabled;
    public bool isGuardianEnabled;
    public bool isTimeSlowEnabled;
    public float attack;
    public List<InventoryItemData> inventoryItems;
}

[Serializable]
public class InventoryItemData
{
    public string itemName;
    public string iconPath;
    public int quantity;
}