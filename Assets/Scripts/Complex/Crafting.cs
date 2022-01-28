using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public bool[] componentCheck;
    public bool canBuild;
    public PlayerInventory player;

    //private void Start()
    //{
    //    canBuild = false;

    //    for (int i = 0; i < player.craftableItems.Length; i++)
    //    {
    //        if (player.craftableItems[i].components.Length != player.craftableItems[i].componentQuantities.Length)
    //        {
    //            Debug.LogError(gameObject.name + " discrepency with crafting requirements. Has " + player.craftableItems[i].components.Length + " components, and " + player.craftableItems[i].componentQuantities.Length + " component quantities listed.");
    //        }

    //        componentCheck = new bool[player.craftableItems[i].components.Length];
    //        for (int j = 0; j < componentCheck.Length; j++)
    //        {
    //            componentCheck[j] = false;
    //        }

    //        Debug.Log(player.craftableItems[i].name + " needs " + player.craftableItems[i].componentQuantities[0] + " " + player.craftableItems[i].components[0].itemName + " and " + player.craftableItems[i].componentQuantities[1] + " " + player.craftableItems[i].components[1].itemName);
    //    }
    //}

    public bool CanCraftCheck(int number)
    {
        canBuild = false;

        if (player.craftableItems[number].components.Length != player.craftableItems[number].componentQuantities.Length)
        {
            Debug.LogError(gameObject.name + " discrepency with crafting requirements. Has " + player.craftableItems[number].components.Length + " components, and " + player.craftableItems[number].componentQuantities.Length + " component quantities listed.");
        }

        componentCheck = new bool[player.craftableItems[number].components.Length];
        for (int j = 0; j < componentCheck.Length; j++)
        {
            componentCheck[j] = false;
        }

        // Debug.Log(player.craftableItems[number].name + " needs " + player.craftableItems[number].componentQuantities[0] + " " + player.craftableItems[number].components[0].itemName + " and " + player.craftableItems[number].componentQuantities[1] + " " + player.craftableItems[number].components[1].itemName);

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
                            componentCheck[i] = true;
                            Debug.Log("You have enough " + player.craftableItems[number].components[i].itemName);
                        }
                        else
                        {
                            Debug.Log("Not enough " + player.craftableItems[number].components[i].itemName + " for " + player.craftableItems[number].name);
                        }
                    }
                }
            }
            else
            {
                invCheck++;
            }
        }

        if (invCheck == player.itemSlots.Length)
        {
            Debug.Log("You have no items!");
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
            Debug.Log("Can craft");
            canBuild = true;
        }

        return canBuild;
    }

    //public void Craft()
    //{

    //}
}
