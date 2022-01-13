using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Item : MonoBehaviour
{
    [HorizontalLine(color: EColor.Red)]
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

    [HorizontalLine(color: EColor.Red)]
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

    [HorizontalLine(color: EColor.Red)]
    [Header("Food")]
    public float foodVal;
    public float drinkVal;

    [HorizontalLine(color: EColor.Red)]
    [Header ("Crops")]
    public bool canBePlanted;
    public int currentGrowthStage;
    public GameObject[] growthStages;
    public float growthSpeed;

    [HorizontalLine(color: EColor.Red)]
    [Header("Crafting")]
    public bool canBeCrafted;
    public int craftLvlRequired;

    public void Grow()
    {
        if(currentGrowthStage < growthStages.Length)
        {
            currentGrowthStage++;
        }
    }
}