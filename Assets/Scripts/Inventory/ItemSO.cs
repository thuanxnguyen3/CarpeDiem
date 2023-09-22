using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu (fileName = "New Item", menuName = "Survival Game/Inventory/New Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { Generic, Consumable, Weapon, MeleeWeapon}

    [Header("General")]
    public ItemType itemType;
    public Sprite icon;
    public string itemName;
    public string description = "New Item Description";

    public bool isStackable;
    public int maxStack = 1;
    /*
    [Header("Weapon")]
    public float damage;

    [Header("Food")]
    public float nutrition;
    */
    [Header("Consumable")]
    public float healthChange = 0f;
    public float hungerChange = 0f;
    public float thirstChange = 0f;
}


