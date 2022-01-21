using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject[] inventorySlots;
    public TMP_Text[] inventoryQuantities;
    public TMP_Text pickupText;
    public PlayerInventory playerInventory;
    public int selectedItem;

    private void Start()
    {
        selectedItem = 0;

        if (inventorySlots.Length != playerInventory.maxItems)
        {
            Debug.LogError("Mismatched number of inventory slots: UI has space for " + inventorySlots.Length + " items, while player inventory has space to hold " + playerInventory.maxItems + " items.");
        }
    }

    private void Update()
    {
        if (playerInventory.itemInRange)
        {
            pickupText.text = "Press E to pick up ITEM";
        }
        else
        {
            pickupText.text = "";
        }
    }

    public void AddItem()
    {

    }
}