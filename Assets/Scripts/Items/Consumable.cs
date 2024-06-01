using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public enum ItemType
    {
        HealthPotion,
        Fireball,
        ScrollOfConfusion
    }

    [SerializeField]
    private ItemType type;

    [SerializeField]
    private int healingAmount;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float confusionDuration;

    public ItemType Type
    {
        get { return type; }
    }

    public int HealingAmount
    {
        get { return healingAmount; }
    }

    public int Damage
    {
        get { return damage; }
    }

    public float ConfusionDuration
    {
        get { return confusionDuration; }
    }

    private void Start()
    {
        GameManager.Get.AddItem(this);
    }
}
