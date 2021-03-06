using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public bool[] componentCheck;
    public PlayerInventory player;

    public UI ui;

    public bool CanCraftCheck(int number)
    {
        bool canBuild = false;

        if (player.craftableItems[number].components.Length != player.craftableItems[number].componentQuantities.Length)
        {
            Debug.LogError(gameObject.name + " discrepency with crafting requirements. Has " + player.craftableItems[number].components.Length + " components, and " + player.craftableItems[number].componentQuantities.Length + " component quantities listed.");
        }

        componentCheck = new bool[player.craftableItems[number].components.Length];
        for (int j = 0; j < componentCheck.Length; j++)
        {
            componentCheck[j] = false;
        }

        int tmp = 0;        // temporary placeholder for counting the number of components that you have enough of said resource
        int invCheck = 0;   // counts how many empty slots the player has

        for (int c = 0; c < player.itemSlots.Length; c++)
        {
            if (player.itemSlots[c] != null)
            {
                for (int i = 0; i < componentCheck.Length; i++)
                {
                    if (player.craftableItems[number].components[i].itemName == player.itemSlots[c].itemName)
                    {
                        if(player.craftableItems[number].componentQuantities[i] <= player.itemQuantities[c])
                        {
                            componentCheck[i] = true;   // Player has enough of this item
                        }
                    }
                }
            }
            else
            {
                invCheck++;
            }
        }

        for (int i = 0; i < componentCheck.Length; i++)
        {
            if (componentCheck[i] == true)
            {
                tmp++;
            }
        }

        if (tmp == componentCheck.Length)
        {
            canBuild = true;
        }

        return canBuild;
    }

    public void Craft(int number)
    {
        if(ui != null)
        {
            ui.SelectItemToCraftUI();
        }

        bool canBuild = CanCraftCheck(number); // if using my UI script, craft ID will be set in crafting menu. Otherwise, set the ID in Unity or via your own means

        if (canBuild)
        {
            for(int i = 0; i < player.itemSlots.Length; i++)
            {
                if(player.itemSlots[i] != null)
                {
                    for (int j = 0; j < player.craftableItems[number].components.Length; j++)
                    {
                        if (player.itemSlots[i].itemName == player.craftableItems[number].components[j].itemName)
                        {
                            player.itemQuantities[i] -= player.craftableItems[number].componentQuantities[j];
                        }
                    }
                }
            }

            player.FindFirstEmpty();

            player.itemSlots[player.firstEmpty] = player.craftableItems[number].GetComponent<Item>();
            player.itemQuantities[player.firstEmpty]++;
            player.firstEmptyFound = false;

            if (ui != null)
            {
                for (int d = 0; d < player.maxItems; d++)
                {
                    ui.UpdateSpritesAndQuantities(d);
                }
            }
        }
        else
        {
            // Player cannot craft this item
        }
    }
}