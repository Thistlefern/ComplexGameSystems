using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public Item[] allPossibleItems; // holds every item in the game ***** IMPORTANT TO DO TO MAKE THIS WORK: YOU MUST FILL THIS WITH PREFABS THAT HAVE THE ITEM SCRIPT THROUGH THE ENGINE *****
    public Recipe[] craftableItems; // fills automatically with all items that exist that have a recipe
    public Item[] itemSlots;        // holds the player's items (in my implementation, this include both a backpack and a hotbar, the hotbar being the first row of the backpack's items)
    public int[] itemQuantities;    // keeps track of how many of each item in the array above the player currently has
    public int maxItems;            // keeps track of the maximum number of items that player can hold in total ***** IMPORTANT TO DO TO MAKE THIS WORK: YOU MUST ENTER THIS IN THE ENGINE *****
    public int hotbarItems;         // keeps track of how many items the player's hotbar can hold, if using one ***** IMPORTANT TO DO TO MAKE THIS WORK: YOU MUST ENTER THIS IN THE ENGINE *****
    public int currentRecipes;      // keeps track of how many recipes the player currently has access to

    public int firstEmpty;          // used while determining the first empty space in the player's inventory
    public bool firstEmptyFound;    // is true if the first empty space has been found in relevent functions
    public int slotToAddTo;         // used while determining where in the player's inventory to place an item
    public bool invFull;            // is true if the player's inventory is full

    private void Awake()
    {
        itemSlots = new Item[maxItems];
        itemQuantities = new int[maxItems];

        for (int i = 0; i < allPossibleItems.Length; i++)   // this can be adjusted later to include only recipes of a certain level, and can be called again when that crafting level increases to increase the number of available recipes
        {
            if (allPossibleItems[i].GetComponent<Recipe>())
            {
                currentRecipes++;
            }
        }
        craftableItems = new Recipe[currentRecipes];
        int tmp = 0;
        for (int i = 0; i < allPossibleItems.Length; i++)
        {
            if (allPossibleItems[i].GetComponent<Recipe>())
            {
                craftableItems[tmp] = allPossibleItems[i].gameObject.GetComponent<Recipe>();
                tmp++;
            }
        }
    }

    void Start()
    {
        firstEmpty = 0;
        firstEmptyFound = false;
        slotToAddTo = 0;
        invFull = false;
    }

    public void FindFirstEmpty()
    {
        int count = 0;
        firstEmptyFound = false;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] == null || itemQuantities[i] == 0)
            {
                firstEmptyFound = true;
                firstEmpty = i;
                return;
            }
            else
            {
                count++;
            }
        }

        if (count == itemSlots.Length)    // if no slots have this item, go back to the first empty slot
        {
            if (!firstEmptyFound)
            {
                invFull = true;
            }
        }
    }
    public void Sort(int max)
    {
        for (int i = 0; i < max; i++)
        {
            if (itemSlots[i] != null)
            {
                for (int j = 0; j < allPossibleItems.Length; j++)
                {
                    if (allPossibleItems[j].name == itemSlots[i].name)
                    {
                        itemSlots[i].sortID = j;
                    }
                }
            }
        }

        BubbleSort(itemSlots, itemQuantities, max);
    }

    void BubbleSort(Item[] items, int[] quantities, int max)
    {
        bool toTheLeft = false;
        bool sorted = false;

        while (!toTheLeft)
        {
            int sortCount = 0;
            
            for(int s = 0; s < (max - 1); s++)
            {
                if(items[s] == null && items[s+1] != null)
                {
                    items[s] = items[s+1];
                    itemQuantities[s] = itemQuantities[s + 1];
                    items[s + 1] = null;
                    itemQuantities[s + 1] = 0;
                }
                else
                {
                    sortCount++;
                }
            }

            if(sortCount == (max - 1))
            {
                toTheLeft = true;
            }
        }

        while (!sorted)
        {
            int sortCount = 0;

            for (int s = 0; s < (max - 1); s++)
            {
                if (items[s] != null && items[s + 1] != null && items[s].sortID > items[s + 1].sortID)
                {
                    Item tmp = items[s];
                    int tmp2 = quantities[s];

                    items[s] = items[s + 1];
                    quantities[s] = quantities[s + 1];
                    
                    items[s + 1] = tmp;
                    quantities[s + 1] = tmp2;
                }
                else
                {
                    sortCount++;
                }
            }

            if (sortCount == (max - 1))
            {
                sorted = true;
            }
        }
    }

    public void PickUpItem(Item thing, int quant)
    {
        if (invFull)
        {
            Debug.Log("Inventory is full");
        }
        else
        {
            if (slotToAddTo != 0)
            {
                for (int i = 0; i < allPossibleItems.Length; i++)
                {
                    if (allPossibleItems[i].itemName == thing.itemName)
                    {
                        itemSlots[slotToAddTo - 1] = allPossibleItems[i];
                        itemQuantities[slotToAddTo - 1] += quant;
                    }
                }
            }
        }
    }

    public void DropItem(int invSlot, int quant)
    {
        if(itemSlots[invSlot] == null)
        {
            Debug.Log("Slot empty, nothing to drop");
        }
        else
        {
            if(quant >= itemQuantities[invSlot])
            {
                itemQuantities[invSlot] = 0;
                itemSlots[invSlot] = null;
            }
            else
            {
                itemQuantities[invSlot] -= quant;
            }
        }
    }
}