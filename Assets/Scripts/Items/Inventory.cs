using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public List<Consumable> Items = new List<Consumable>();
    public int MaxItems;

    public bool AddItem(Consumable item)
    {
        if (Items.Count < MaxItems)
        {
            Items.Add(item);
            return true;
        }
        else
        {
            Debug.Log("Inventory is full.");
            return false;
        }
    }

    public void DropItem(Consumable item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            // Optional: Add logic here to drop the item into the game world
        }
        else
        {
            Debug.Log("Item not found in inventory.");
        }
    }
}

