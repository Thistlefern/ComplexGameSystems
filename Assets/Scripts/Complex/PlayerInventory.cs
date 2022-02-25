using UnityEngine;
using System;
using JosieItem;
using JosieRecipe;
using JosieItemDatabase;

namespace JosiePlayerInventory
{
    [Serializable]
    public struct SlotInfo
    {
        public Item item;
        public int quantity;
    }

    public class PlayerInventory : MonoBehaviour
    {
        [HideInInspector]
        public Recipe[] craftableItems; // fills automatically with all items that exist that have a recipe
        [HideInInspector]
        public SlotInfo[] itemSlots;    // holds the player's items (in my implementation, this include both a backpack and a hotbar, the hotbar being the first row of the backpack's items)

        public int maxItems;            // keeps track of the maximum number of items that player can hold in total ***** IMPORTANT TO DO TO MAKE THIS WORK: YOU MUST ENTER THIS IN THE ENGINE *****

        [HideInInspector]
        public int firstEmpty;          // used while determining the first empty space in the player's inventory
        [HideInInspector]
        public bool firstEmptyFound;    // is true if the first empty space has been found in relevent functions
        [HideInInspector]
        public int slotToAddTo;         // used while determining where in the player's inventory to place an item
        [HideInInspector]
        public bool invFull;            // is true if the player's inventory is full

        private void Awake()
        {
            itemSlots = new SlotInfo[maxItems];

            int currentRecipes = 0;

            for (int i = 0; i < ItemDatabase.instance.items.Length; i++)   // this can be adjusted later to include only recipes of a certain level, and can be called again when that crafting level increases to increase the number of available recipes
            {
                if (ItemDatabase.instance.items[i].GetComponent<Recipe>())
                {
                    currentRecipes++;
                }
            }

            craftableItems = new Recipe[currentRecipes];
            int tmp = 0;
            for (int i = 0; i < ItemDatabase.instance.items.Length; i++)
            {
                if (ItemDatabase.instance.items[i].GetComponent<Recipe>())
                {
                    craftableItems[tmp] = ItemDatabase.instance.items[i].gameObject.GetComponent<Recipe>();
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
            firstEmpty = 0;
            firstEmptyFound = false;
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].quantity == 0)
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

            if (count == itemSlots.Length)
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
                if (itemSlots[i].quantity != 0)
                {
                    for (int j = 0; j < ItemDatabase.instance.items.Length; j++)
                    {
                        if (ItemDatabase.instance.items[j].name == itemSlots[i].item.name)
                        {
                            itemSlots[i].item.sortID = j;
                        }
                    }
                }
            }

            BubbleSort(itemSlots, max);
        }

        void BubbleSort(SlotInfo[] items, int max)
        {
            bool toTheLeft = false;
            bool sorted = false;

            while (!toTheLeft)
            {
                int sortCount = 0;

                for (int s = 0; s < (max - 1); s++)
                {
                    if (items[s].quantity == 0 && items[s + 1].quantity != 0)
                    {
                        items[s] = items[s + 1];
                        items[s + 1].item = null;
                        itemSlots[s + 1].quantity = 0;
                    }
                    else
                    {
                        sortCount++;
                    }
                }

                if (sortCount == (max - 1))
                {
                    toTheLeft = true;
                }
            }

            while (!sorted)
            {
                int sortCount = 0;

                for (int s = 0; s < (max - 1); s++)
                {
                    if (items[s].quantity != 0 && items[s + 1].quantity != 0 && items[s].item.sortID > items[s + 1].item.sortID)
                    {
                        SlotInfo tmp = items[s];
                        items[s] = items[s + 1];
                        items[s + 1] = tmp;
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

        public bool PickUpItem(Item thing, int quant)
        {
            bool success = false;

            FindFirstEmpty();

            if (invFull)
            {
                Debug.Log("Inventory is full");
            }
            else
            {
                if (slotToAddTo != 0)
                {
                    itemSlots[slotToAddTo - 1].item = ItemDatabase.instance.database[thing.itemName];
                    itemSlots[slotToAddTo - 1].quantity += quant;
                    success = true;
                }
                else
                {
                    Debug.LogError("Pick up failed.");
                }
            }

            return success;
        }

        public void DropItem(int invSlot, int quant)
        {
            if (itemSlots[invSlot].quantity == 0)
            {
                Debug.Log("Slot empty, nothing to drop");
            }
            else
            {
                if (quant >= itemSlots[invSlot].quantity)
                {
                    itemSlots[invSlot].quantity = 0;
                    itemSlots[invSlot].item = null;
                }
                else
                {
                    itemSlots[invSlot].quantity -= quant;
                }
            }
        }
    }
}