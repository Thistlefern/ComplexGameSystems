using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    // TODO can use tools
    // TODO can drop item
    // TODO DOCUMENTATION
    // TODO* settings menu

    public Item[] allPossibleItems;     // fill this array with the prefabs for every item. Might be a good idea to seperate this from the player if you have a large amount of possible items to pick up.
    public Recipe[] craftableItems;
    public Item[] itemSlots;
    public int[] itemQuantities;
    public int maxItems;
    public int currentRecipes;

    public int firstEmpty;
    public bool firstEmptyFound;
    public int slotToAddTo;
    public bool invFull;

    public UI ui;

    public int selectedItem;

    public Crafting crafting;

    private void Awake()
    {
        itemSlots = new Item[maxItems];
        itemQuantities = new int[maxItems];

        for (int i = 0; i < allPossibleItems.Length; i++)   // this can be adjusted later to include only recipes of a certain level, and can be called again when that crafting leverl increases to increase the number of available recipes
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
        selectedItem = 0;
    }
    public void TestFunction()
    {
        Debug.Log("Use this for testing a function.");
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
    public void Sort()
    {
        for (int i = 0; i < maxItems; i++)
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

        BubbleSort(itemSlots, itemQuantities);
    }

    void BubbleSort(Item[] items, int[] quantities)
    {
        bool toTheLeft = false;
        bool sorted = false;

        while (!toTheLeft)
        {
            int sortCount = 0;
            
            for(int s = 0; s < (maxItems - 1); s++)
            {
                if(items[s] == null && items[s+1] != null)
                {
                    items[s] = items[s+1];
                    items[s + 1] = null;
                }
                else
                {
                    sortCount++;
                }
            }

            if(sortCount == (maxItems - 1))
            {
                if(ui != null)
                {
                    for (int d = 0; d < maxItems; d++)
                    {
                        ui.UpdateSpritesAndQuantities(d);
                    }
                }

                toTheLeft = true;
            }
        }

        while (!sorted)
        {
            int sortCount = 0;

            for (int s = 0; s < (maxItems - 1); s++)
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

            if (sortCount == (maxItems - 1))
            {
                if (ui != null)
                {
                    for (int d = 0; d < maxItems; d++)
                    {
                        ui.UpdateSpritesAndQuantities(d);
                    }
                }

                sorted = true;
            }
        }
    }
}