using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JosieItem;

namespace JosieItemDatabase
{
    public class ItemDatabase : MonoBehaviour
    {
        public Item[] items;
        public Dictionary<string, Item> database = new Dictionary<string, Item>();
        public static ItemDatabase instance;

        private void Awake()
        {
            instance = this;

            for (int i = 0; i < items.Length; i++)
            {
                database.Add(items[i].itemName, items[i]);
            }
        }
    }
}