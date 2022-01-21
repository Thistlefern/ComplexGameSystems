using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public Item[] allPossibleItems;
    public Item[] itemSlots;
    public int[] itemQuantities;
    public int maxItems;

    public UI ui;
    public bool itemInRange;
    public GameObject item;
    public bool noRoom;
    int count;
    int firstEmpty;
    bool firstEmptyFound;
    public int slotToAddTo;

    void Start()
    {
        itemSlots = new Item[maxItems];
        itemQuantities = new int[maxItems];

        itemInRange = false;
        item = null;
        count = 0;
        firstEmpty = 0;
        firstEmptyFound = false;
        slotToAddTo = 0;
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
                    Debug.Log("This will be added to the stack in slot " + slotToAddTo);
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
                    Debug.Log("First empty slot: " + firstEmpty);
                }
            }
        }

        if (count == itemSlots.Length)    // if no slots have this item, go back to the first empty slot
        {
            if (!firstEmptyFound)
            {
                Debug.Log("No room!");  // if there is no empty slot, display a message
            }
            else
            {
                slotToAddTo = firstEmpty + 1;  // if there is an empty slot, select that slot
            }
        }

        Debug.Log("Slot to add to: " + slotToAddTo);
    }

    private void OnTriggerExit(Collider other)
    {
        itemInRange = false;
        item = null;
        slotToAddTo = 0;
        firstEmptyFound = false;
        count = 0;
    }

    public void PickUpItem()
    {
        if (itemInRange)
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

            itemInRange = false;
            slotToAddTo = 0;
            firstEmptyFound = false;
            count = 0;
            Destroy(item);
            item = null;
        }
    }
}