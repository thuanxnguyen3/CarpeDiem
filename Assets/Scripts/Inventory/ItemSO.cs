using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu (fileName = "New Item", menuName = "Survival Game/Inventory/New Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { Generic, Consumable, MeleeWeapon}

    [Header("General")]
    public ItemType itemType;
    public Sprite icon;
    public string itemName;
    public string description = "New Item Description";

    public bool isStackable;
    public int maxStack = 1;

}
