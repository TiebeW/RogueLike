using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Label[] labels = new Label[8];

    private VisualElement root;
    private int selected;
    private int numItems;

    public int Selected => selected;

    private void Clear()
    {
        foreach (var label in labels)
        {
            label.text = string.Empty;
        }
    }

    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            labels[i] = root.Q<Label>($"Item{i + 1}");
        }

        Clear();
        root.style.display = DisplayStyle.None;
    }

    private void UpdateSelected()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (i == selected)
            {
                labels[i].style.backgroundColor = Color.green;
            }
            else
            {
                labels[i].style.backgroundColor = Color.clear;
            }
        }
    }

    public void SelectNextItem()
    {
        selected = (selected + 1) % numItems;
        UpdateSelected();
    }

    public void SelectPreviousItem()
    {
        selected = (selected - 1 + numItems) % numItems;
        UpdateSelected();
    }

    public void Show(List<Consumable> list)
    {
        selected = 0;
        numItems = list.Count;

        Clear();

        for (int i = 0; i < numItems && i < labels.Length; i++)
        {
            labels[i].text = list[i].name;
        }

        UpdateSelected();
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }
}
