using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] allPossibleItems;
    public Item[] itemSlots;
    public int maxItems;

    public UI ui;
    public bool itemInRange;
    public bool noRoom;

    void Start()
    {
        itemSlots = new Item[maxItems];
        itemInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        itemInRange = true;

        // Auto pickup
        // Destroy(other.gameObject);
        int count = 0;
        int firstEmpty = 0;
        bool firstEmptyFound = false;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                if (other.GetComponent<Item>().itemName == itemSlots[i].itemName) // check all slots for the item being picked up
                {
                    Debug.Log(i + 1);   // if you already have one, add the one picked up to that slot
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
                Debug.Log("No room!");  // if there is no empty slot, display a message
            }
            else
            {
                Debug.Log("First empty slot: slot " + firstEmpty);  // if there is an empty slot, put this new item there
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        itemInRange = false;
    }
}