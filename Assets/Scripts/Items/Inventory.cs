using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Consumable> Items = new List<Consumable>();
    private int selectedItemIndex = 0;

    public bool AddItem(Consumable item)
    {
        if (Items.Count < 10)  // Assuming a maximum inventory size of 10 items
        {
            Items.Add(item);
            return true;
        }
        return false;
    }

    public void DropItem(Consumable item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
        }
    }

    public Consumable SelectedItem()
    {
        if (Items.Count > 0)
        {
            return Items[selectedItemIndex];
        }
        return null;
    }

    public void SelectNextItem()
    {
        if (Items.Count > 0)
        {
            selectedItemIndex = (selectedItemIndex + 1) % Items.Count;
        }
    }

    public void SelectPreviousItem()
    {
        if (Items.Count > 0)
        {
            selectedItemIndex = (selectedItemIndex - 1 + Items.Count) % Items.Count;
        }
    }
}
