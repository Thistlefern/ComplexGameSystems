using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Basics")]
    public string itemName; // the display name for the item
    public enum ItemType
    {
        SELECT,
        Food,       // food items that don't have to be crafted
        Meal,       // food items that require crafting
        Drink,      // drink items
        Seed,       // a seed for a crop, contains the growth stages for said crop except the final stage, which is then a food item
        Tool,       // items which can be used to complete tasks
        Weapon,     // items which hurt enemies
        Furniture   // self explanitory (includes workbenches)
    };
    public ItemType type;
    public float cost;

    [Header("Damage Types")]
    public float attack;
    public float destruction;
    public enum DamageSpecialty
    {
        None,
        Wood,
        Stone
    };
    public DamageSpecialty specialty;

    [Header("Food")]
    public float foodVal;
    public float drinkVal;
    public bool canBePlanted;
    public int growthStage;

    [Header("Crafting")]
    public bool canBeCrafted;
    public int craftLvlRequired;
}