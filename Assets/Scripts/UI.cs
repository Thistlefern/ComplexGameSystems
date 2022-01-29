﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class UI : MonoBehaviour
{
    public GameObject invPanel;
    public Image[] inventorySlots;
    public TMP_Text[] inventoryQuantities;
    public TMP_Text pickupText;
    public PlayerInventory playerInventory;
    public Sprite nullSprite;

    public GameObject[] selectIndicators;
    public int selectedItem;

    public GameObject craftingMenu;
    public bool currentlyCrafting;
    public Crafting craftingScript;

    public TMP_Dropdown dropdown;
    public List<string> dropdownOptions;

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

        invPanel.SetActive(true);
        pickupText.gameObject.SetActive(true);
        craftingMenu.SetActive(false);
        currentlyCrafting = false;

        for(int i = 0; i < playerInventory.craftableItems.Length; i++)
        {
            dropdownOptions.Add(playerInventory.craftableItems[i].GetComponent<Item>().itemName);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(dropdownOptions);
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

    public void UpdateSpritesAndQuantities(int number)
    {
        for (int i = 0; i < playerInventory.maxItems; i++)
        {
            if(playerInventory.itemSlots[number] != null)
            {
                inventorySlots[number].sprite = playerInventory.itemSlots[number].itemImage;
                inventoryQuantities[number].text = playerInventory.itemQuantities[number].ToString();   // NOTE: if a player has an item dragged into their inventory in the editor during play,
            }                                                                                           // the quantity will show as 0.

            if (playerInventory.itemQuantities[i] == 0 && inventorySlots[i].sprite.name != "UIMask")
            {
                //Debug.Log("Slot " + i + " is empty");
                //Debug.Log(inventorySlots[i].sprite.name);
                inventorySlots[i].sprite = nullSprite;
                inventoryQuantities[i].text = "";
            }
        }
    }

    public void SelectUp()
    {
        selectIndicators[selectedItem].SetActive(false);
        if(selectedItem == inventorySlots.Length - 1)
        {
            selectedItem = 0;
        }
        else
        {
            selectedItem++;
        }
        selectIndicators[selectedItem].SetActive(true);
    }
    public void SelectDown()
    {
        selectIndicators[selectedItem].SetActive(false);
        if (selectedItem == 0)
        {
            selectedItem = inventorySlots.Length - 1;
        }
        else
        {
            selectedItem--;
        }
        selectIndicators[selectedItem].SetActive(true);
    }

    public void SelectItemToCraftUI()
    {
        for (int i = 0; i < playerInventory.craftableItems.Length; i++)
        {
            if (playerInventory.craftableItems[i].GetComponent<Item>().itemName == dropdown.captionText.text)
            {
                if (playerInventory.craftableItems[i].GetComponent<Item>().itemName == dropdown.captionText.text)
                {
                    craftingScript.craftID = i;
                }
            }
        }
    }
}