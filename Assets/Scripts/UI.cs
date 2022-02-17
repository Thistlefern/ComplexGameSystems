using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class UI : MonoBehaviour
{
    // TODO* tool order setting
    // TODO* reorganize yourself

    public GameObject hotbarUI;
    public GameObject backpackUI;
    public TMP_Text pickupText;
    
    public Image[] inventorySlots;
    public TMP_Text[] inventoryQuantities;

    public Image[] hotbarSlots;
    public TMP_Text[] hotbarQuantities;

    public PlayerInventory playerInventory;
    public PlayerController playerController;
    public Sprite nullSprite;

    public GameObject[] selectIndicators;
    public int selectedItem;

    public int craftID;
    public GameObject craftingMenu;
    public bool currentlyCrafting;
    public Crafting craftingScript;
    public bool[] componentCheck;

    public TMP_Dropdown dropdown;
    public List<string> dropdownOptions;
    public Image[] requirementSprites;
    public GameObject[] requirementNotEnough;
    public TMP_Text[] requirementQuantity;

    public GameObject pauseMenu;

    public GameObject settingsMenu;
    public bool inSettings;
    public bool invertScroll;
    public Slider rotationSlider;

    private void Start()
    {
        backpackUI.SetActive(false);

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

        for (int i = 0; i < hotbarQuantities.Length; i++)
        {
            hotbarQuantities[i].text = "";
        }

        if (inventorySlots.Length != playerInventory.maxItems)
        {
            Debug.LogError("Mismatched number of inventory slots: UI has space for " + inventorySlots.Length + " items, while player inventory has space to hold " + playerInventory.maxItems + " items.");
        }

        hotbarUI.SetActive(true);
        pickupText.gameObject.SetActive(true);
        craftingMenu.SetActive(false);
        currentlyCrafting = false;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        inSettings = false;
        invertScroll = false;

        for(int i = 0; i < playerInventory.craftableItems.Length; i++)
        {
            dropdownOptions.Add(playerInventory.craftableItems[i].GetComponent<Item>().itemName);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(dropdownOptions);
    }

    private void Update()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i].sprite = inventorySlots[i].sprite;
            hotbarQuantities[i].text = inventoryQuantities[i].text;
        }

        if (playerController.gameIsPaused)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }

        if (playerController.itemInRange)
        {
            string correctTool = "";

            for (int i = 0; i < playerInventory.craftableItems.Length; i++)
            {
                if (playerController.item.GetComponent<Item>().resourceType != null)
                {
                    if (playerInventory.craftableItems[i].GetComponent<Item>().specialty.ToString() == playerController.item.GetComponent<Item>().resourceType.GetComponent<Item>().itemName)
                    {
                        correctTool = playerInventory.craftableItems[i].GetComponent<Item>().itemName;
                    }
                }
            }

            if (playerController.item != null)
            {
                if (playerController.item.GetComponent<Item>().type.ToString() == "Resource")
                {
                    pickupText.text = "Press E to pick up " + playerController.item.GetComponent<Item>().itemName;
                }
                else if (playerController.item.GetComponent<Item>().type.ToString() == "Source")
                {
                    pickupText.text = "Select " + correctTool + " and click to harvest " + playerController.item.GetComponent<Item>().resourceType.GetComponent<Item>().itemName;
                }
            }
        }
        else
        {
            pickupText.text = "";
        }
    }

    public void DropdownValueChanged(TMP_Dropdown change)
    {
        SelectItemToCraftUI();
        CheckRequirementsUI(craftID);
    }

    public void ToggleScroll()
    {
        invertScroll = !invertScroll;
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
        if (!currentlyCrafting)
        {
            for (int i = 0; i < playerInventory.hotbarItems; i++)
            {
                if (playerInventory.itemSlots[number] != null)
                {
                    inventorySlots[number].sprite = playerInventory.itemSlots[number].itemImage;
                    inventoryQuantities[number].text = playerInventory.itemQuantities[number].ToString();   // NOTE: if a player has an item dragged into their inventory in the editor during play,
                }                                                                                           // the quantity will show as 0.

                if (playerInventory.itemQuantities[i] == 0 && inventorySlots[i].sprite.name != "UIMask")
                {
                    inventorySlots[i].sprite = nullSprite;
                    inventoryQuantities[i].text = "";
                    playerInventory.itemSlots[i] = null;
                }
            }
        }
        else
        {
            for (int i = 0; i < playerInventory.maxItems; i++)
            {
                if (playerInventory.itemSlots[number] != null)
                {
                    inventorySlots[number].sprite = playerInventory.itemSlots[number].itemImage;
                    inventoryQuantities[number].text = playerInventory.itemQuantities[number].ToString();   // NOTE: if a player has an item dragged into their inventory in the editor during play,
                }                                                                                           // the quantity will show as 0.

                if (playerInventory.itemQuantities[i] == 0 && inventorySlots[i].sprite.name != "UIMask")
                {
                    inventorySlots[i].sprite = nullSprite;
                    inventoryQuantities[i].text = "";
                    playerInventory.itemSlots[i] = null;
                }
            }
        }
    }

    public void SelectUp()
    {
        selectIndicators[selectedItem].SetActive(false);

        if (!invertScroll)
        {
            if (selectedItem == hotbarSlots.Length - 1)
            {
                selectedItem = 0;
            }
            else
            {
                selectedItem++;
            }
        }
        else
        {
            if (selectedItem == 0)
            {
                selectedItem = hotbarSlots.Length - 1;
            }
            else
            {
                selectedItem--;
            }
        }

        selectIndicators[selectedItem].SetActive(true);
    }
    public void SelectDown()
    {
        selectIndicators[selectedItem].SetActive(false);
        if (invertScroll)
        {
            if (selectedItem == hotbarSlots.Length - 1)
            {
                selectedItem = 0;
            }
            else
            {
                selectedItem++;
            }
        }
        else
        {
            if (selectedItem == 0)
            {
                selectedItem = hotbarSlots.Length - 1;
            }
            else
            {
                selectedItem--;
            }
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
                    craftID = i;
                }
            }
        }
    }

    public void CheckRequirementsUI(int number)
    {
        componentCheck = new bool[playerInventory.craftableItems[number].components.Length];
        
        for (int j = 0; j < componentCheck.Length; j++)
        {
            componentCheck[j] = false;
        }

        for(int s = 0; s < requirementSprites.Length; s++)
        {
            requirementSprites[s].sprite = nullSprite;
            requirementQuantity[s].text = "";
            requirementNotEnough[s].SetActive(false);

        }
        for (int i = 0; i < componentCheck.Length; i++)
        {
            requirementSprites[i].sprite = playerInventory.craftableItems[number].components[i].itemImage;
            requirementQuantity[i].text = "x" + playerInventory.craftableItems[number].componentQuantities[i].ToString();
        }

        int nullCount = 0;

        for (int c = 0; c < playerInventory.itemSlots.Length; c++)
        {
            if (playerInventory.itemSlots[c] != null)
            {
                for (int i = 0; i < componentCheck.Length; i++)
                {
                    if (playerInventory.craftableItems[number].components[i].itemName == playerInventory.itemSlots[c].itemName)
                    {
                        if (playerInventory.craftableItems[number].componentQuantities[i] <= playerInventory.itemQuantities[c])
                        {
                            componentCheck[i] = true;   // Player has enough of this item
                        }
                        else
                        {
                            requirementNotEnough[i].SetActive(true);    // Player does not have enough of this item
                        }
                    }
                }
            }
            else
            {
                nullCount++;
            }

            if (nullCount == playerInventory.itemSlots.Length)
            {
                // Player's inventory is empty
                for(int i = 0; i < playerInventory.craftableItems[number].components.Length; i++)
                {
                    requirementNotEnough[i].SetActive(true);
                }
            }
        }

        for(int i = 0; i < playerInventory.craftableItems[number].componentQuantities.Length; i++)
        {
            int noneOfThisItem = 0;
            for(int c = 0; c < playerInventory.itemSlots.Length; c++)
            {
                if (playerInventory.itemSlots[c] != null)
                {
                    if (playerInventory.itemSlots[c].itemName != playerInventory.craftableItems[number].components[i].itemName)
                    {
                        noneOfThisItem++;
                    }
                }
                else
                {
                    noneOfThisItem++;
                }
            }

            if(noneOfThisItem == playerInventory.itemSlots.Length)
            {
                requirementNotEnough[i].SetActive(true);
            }
        }
    }

    public void ActuallyCraft()
    {
        SelectItemToCraftUI();
        CheckRequirementsUI(craftID);
        bool success = craftingScript.CanCraftCheck(craftID);
        craftingScript.Craft(craftID);
        if (success)
        {
            for(int i = 0; i < playerInventory.itemSlots.Length; i++)
            {
                UpdateSpritesAndQuantities(i);
            }
        }
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void ChangeRotSpeed()
    {
        playerController.rotateSpeed = rotationSlider.value;
    }
}