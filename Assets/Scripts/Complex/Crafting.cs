using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JosiePlayerInventory;
using JosieItem;

namespace JosieCrafting
{
    public class Crafting : MonoBehaviour
    {
        public bool[] componentCheck;   // each bool becomes true if the player has enough of the item in that location of the Recipe Components array
        public PlayerInventory player;  // checks the player's inventory for items to make sure that crafting can occur

        public bool CanCraftCheck(int number)
        {
            bool canBuild = false;

            if (player.craftableItems[number].components.Length != player.craftableItems[number].components.Length)
            {
                Debug.LogError(gameObject.name + " discrepency with crafting requirements. Has " + player.craftableItems[number].components.Length + " components, and " + player.craftableItems[number].components.Length + " component quantities listed.");
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
                if (player.itemSlots[c].quantity != 0)
                {
                    for (int i = 0; i < componentCheck.Length; i++)
                    {
                        if (player.craftableItems[number].components[i].item == player.itemSlots[c].item)
                        {
                            if (player.craftableItems[number].components[i].quantity <= player.itemSlots[c].quantity)
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

            if (player.invFull)                     // In this implementation, you can't build even if you would have room after ingredients are removed during crafting
            {
                canBuild = false;
            }

            return canBuild;
        }

        public void Craft(int number)
        {
            bool canBuild = CanCraftCheck(number);  // if using my UI script, craft ID will be set in crafting menu. Otherwise, set the ID in Unity or via your own means

            if (canBuild)
            {
                for (int i = 0; i < player.itemSlots.Length; i++)
                {
                    if (player.itemSlots[i].quantity != 0)
                    {
                        bool resourceFound = false;
                        for (int j = 0; j < player.craftableItems[number].components.Length; j++)
                        {
                            if (player.itemSlots[i].item == player.craftableItems[number].components[j].item && !resourceFound)
                            {
                                player.itemSlots[i].quantity -= player.craftableItems[number].components[j].quantity;
                                resourceFound = true;
                            }
                        }
                    }
                }

                player.FindFirstEmpty();

                player.itemSlots[player.firstEmpty].item = player.craftableItems[number].GetComponent<Item>();
                player.itemSlots[player.firstEmpty].quantity++;
                player.firstEmptyFound = false;
            }
        }
    }
}