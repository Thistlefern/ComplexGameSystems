using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Item : MonoBehaviour
{
    [HorizontalLine(color: EColor.Red)]
    #region basics
    [Header("Basics")]
    public string itemName;     // the display name for the item
    public Sprite itemImage;    // the image that will be displayed in the player's inventory when they have this item
    public int sortID;          // the number assigned to this item so that it can be sorted
    public enum ItemType    // list of item types
    {
        SELECT,
        Resource,   // can be collected to use for recipes
        Source,     // can be destroyed with tools to gain resources
        Tool,       // can be used to complete tasks
        Weapon,     // hurt enemies
        Food,       // food that doesn't have to be crafted
        Meal,       // food that requires crafting
        Drink,      // drinks
        Seed,       // a seed for a crop, contains the growth stages for said crop except the final stage, which is then a food item
        Furniture   // self explanitory (includes workbenches)
    };
    public ItemType type;   // the item type assigned to THIS item
    public GameObject resourceType; // if the item is a Source item, this is the type of Resource that it can yield by harvesting

    [Tooltip("Only needed if implementing a shopping/selling system.")]
    public float cost;  // how much the item costs
    // Ideas to consider adding: health, stamina
    #endregion

    [HorizontalLine(color: EColor.Red)]

    #region damage types
    [Header("Damage Types")]
    [Tooltip("Damage to enemies.")]
    public float attack;    // how much damage the item does to enemies
    [Tooltip("Damage to environment.")]
    public float destruction;   // how much damage the item does to the environment
    public enum DamageSpecialty // list of environment types that the item deals extra damage to (ex. an axe would deal extra damage to wood, but not to stone)
    {
        noneone,
        stone,
        wood
    };
    public DamageSpecialty specialty;   // the type of environment that THIS item deals extra damage to
    // Ideas to consider adding: weapon range, weapon accuracy, degradation
    #endregion

    [HorizontalLine(color: EColor.Red)]

    #region food
    [Header("Food")]
    public float foodVal;   // how much hunger does this food get rid of?
    public float drinkVal;  // how much thirst does this drink get rid of?
    // Ideas to consider adding: cooking meals, adding health to player, adding buffs to player
    #endregion

    [HorizontalLine(color: EColor.Red)]

    #region crops
    [Header ("Crops")]
    public bool canBePlanted;           // is this item a seed?
    public int currentGrowthStage;      // keeps track of the current stage of growth (0 is seed, 1 is seedling, etc)
    public GameObject[] growthStages;   // an array for the prefabs/art assets of each growth stage
    public float growthSpeed;           // how quickly this plant grows
    // Ideas to consider adding: check for final stage of growth in Grow function and add ability to harvest, requires watering boolean, requires weeding/pest
    #endregion

    [HorizontalLine(color: EColor.Red)]

    #region crafting
    [Header("Crafting")]
    public bool canBeCrafted;       // does this item have a crafting recipe?
    public int craftLvlRequired;    // level required to craft this item
    // Ideas to consider adding: requires a specific workbench, player XP from crafting, check for player crafting level
    #endregion

    public void Grow()
    {
        if(currentGrowthStage < growthStages.Length)
        {
            currentGrowthStage++;
        }
    }
}