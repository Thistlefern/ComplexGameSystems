using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public Item[] allPossibleItems;     // fill this array with the prefabs for every item. Might be a good idea to seperate this from the player if you have a large amount of possible items to pick up.
    public Recipe[] craftableItems;
    public Item[] itemSlots;
    public int[] itemQuantities;
    public int maxItems;
    public int currentRecipes;

    public UI ui;
    public bool itemInRange;
    public GameObject item;
    public bool noRoom;
    int count;
    int firstEmpty;
    bool firstEmptyFound;
    public int slotToAddTo;
    public bool invFull;

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
        itemInRange = false;
        item = null;
        count = 0;
        firstEmpty = 0;
        firstEmptyFound = false;
        slotToAddTo = 0;
        invFull = false;

        selectedItem = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        itemInRange = true;
        item = other.gameObject;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                if (other.GetComponent<Item>().itemName == itemSlots[i].itemName) // check all slots for the item being picked up
                {
                    slotToAddTo = i + 1;   // if you already have one, select the slot it is in
                    return;
                }
                else
                {
                    count++;    // counting the slots that don't contain the item being picked up (this one counts filled slots)
                }
            }
            else
            {
                count++;        // counting the slots that don't contain the item being picked up (this one counts empty slots)
                if (!firstEmptyFound)   // keep track of the first empty slot that the player has
                {
                    firstEmptyFound = true;
                    firstEmpty = i;
                }
            }
        }

        if (count == itemSlots.Length)    // if no slots have this item, go back to the first empty slot
        {
            if (!firstEmptyFound)
            {
                invFull = true;
            }
            else
            {
                slotToAddTo = firstEmpty + 1;  // if there is an empty slot, select that slot
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        itemInRange = false;
        item = null;
        slotToAddTo = 0;
        firstEmptyFound = false;
        count = 0;
    }

    public void TestFunction()
    {
        if (crafting.CanCraftCheck(0))
        {
            itemSlots[5] = craftableItems[0].gameObject.GetComponent<Item>();
            itemQuantities[0] -= 2;
            itemQuantities[1] -= 2;
            itemQuantities[5]++;
            for(int i = 0; i < itemSlots.Length; i++)
            {
                ui.UpdateSpritesAndQuantities(i);
            }
        }
    }

    public void PickUpItem()
    {
        if (itemInRange)
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
                        if (allPossibleItems[i].itemName == item.GetComponent<Item>().itemName)
                        {
                            itemSlots[slotToAddTo - 1] = allPossibleItems[i];
                            itemQuantities[slotToAddTo - 1]++;
                        }
                    }
                }

                ui.AddItem();
                itemInRange = false;
                slotToAddTo = 0;
                firstEmptyFound = false;
                count = 0;
                Destroy(item);
                item = null;
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
                for(int d = 0; d < maxItems; d++)
                {
                    ui.UpdateSpritesAndQuantities(d);
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
                for (int d = 0; d < maxItems; d++)
                {
                    ui.UpdateSpritesAndQuantities(d);
                }
                sorted = true;
            }
        }
    }
}