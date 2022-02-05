using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    public Item[] components;           // components required to craft this item ***** IMPORTANT TO DO TO MAKE THIS WORK: YOU MUST FILL THIS WITH PREFABS THAT HAVE THE ITEM SCRIPT THROUGH THE ENGINE *****
    public int[] componentQuantities;   // the quantities of the components above ***** IMPORTANT TO DO TO MAKE THIS WORK: YOU MUST FILL THIS WITH INTEGERS THROUGH THE ENGINE *****
}