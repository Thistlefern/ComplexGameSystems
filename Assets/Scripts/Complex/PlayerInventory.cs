using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] allPossibleItems;
    public Item[] itemSlots;
    public UI ui;

    void Start()
    {
        itemSlots = new Item[ui.inventorySlots.Length];
    }

    void Update()
    {
        
    }
}