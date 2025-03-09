using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public string currentSceneName;
    public List<string> collectedPotions;
    public Sprite currentCheckpointImage;
    public List<InventoryItemData> inventoryItems;
    public List<bool> collectedWeapons;
    public List<string> collectedWeaponsIndex;
    public List<string> weaponPrefabPaths; // Prefab Â·¾¶
    public string checkpointImagePath;
    public int maxSubWeaponNum;
}

[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public string iconPath;
    public int quantity;
}