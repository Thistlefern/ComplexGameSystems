using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Image[] inventorySlots;
    public TMP_Text[] inventoryQuantities;
    public TMP_Text pickupText;
    public PlayerInventory playerInventory;

    public GameObject[] selectIndicators;
    public int selectedItem;

    private void Start()
    {
        selectedItem = 0;
        for(int i = 0; i < selectIndicators.Length; i++)
        {
            selectIndicators[i].SetActive(false);
        }
        selectIndicators[selectedItem].SetActive(true);

        for(int i = 0; i < inventoryQuantities.Length; i++)
        {
            inventoryQuantities[i].text = "";
        }

        if (inventorySlots.Length != playerInventory.maxItems)
        {
            Debug.LogError("Mismatched number of inventory slots: UI has space for " + inventorySlots.Length + " items, while player inventory has space to hold " + playerInventory.maxItems + " items.");
        }
    }

    private void Update()
    {
        if (playerInventory.itemInRange)
        {
            pickupText.text = "Press E to pick up " + playerInventory.item.GetComponent<Item>().itemName;
        }
        else
        {
            pickupText.text = "";
        }
    }

    public void AddItem()
    {
        // NOTE: this gets called when the player picks up an item, and thus it doesn't need to check for when the player picks up an item
        // Above note made on a day when I am dead tired, so I don't sin because of forgetting the truth in said note
        
        if(inventorySlots[playerInventory.slotToAddTo - 1].sprite.name == "UIMask")
        {
            inventorySlots[playerInventory.slotToAddTo - 1].sprite = playerInventory.itemSlots[playerInventory.slotToAddTo - 1].itemImage;
        }

        inventoryQuantities[playerInventory.slotToAddTo - 1].text = playerInventory.itemQuantities[playerInventory.slotToAddTo - 1].ToString();
    }
}