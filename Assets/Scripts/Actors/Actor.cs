using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AdamMilVisibility algorithm;
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    [Header("Powers")]
    [SerializeField] private int maxHitPoints = 30;
    [SerializeField] private int hitPoints = 30;
    [SerializeField] private int defense;
    [SerializeField] private int power;
    [SerializeField] private int level = 1; // New variable for Level
    [SerializeField] private int xp = 0; // New variable for XP
    [SerializeField] private int xpToNextLevel = 100; // New variable for XP required to reach next level

    public int MaxHitPoints => maxHitPoints;
    public int HitPoints => hitPoints;
    public int Defense => defense;
    public int Power => power;
    public int Level => level; // Getter for Level
    public int XP => xp; // Getter for XP
    public int XPToNextLevel => xpToNextLevel; // Getter for XP required to reach next level

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.SetLevel(level);
            UIManager.Instance.SetXP(xp);
        }
    }

    public void AddXp(int xpAmount)
    {
        xp += xpAmount;

        while (xp >= xpToNextLevel)
        {
            xp -= xpToNextLevel;
            LevelUp();
        }

        if (GetComponent<Player>())
        {
            UIManager.Instance.SetXP(xp);
        }
    }

    private void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        maxHitPoints += 10;
        defense += 2;
        power += 2;

        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessage("You leveled up!", Color.green);
            UIManager.Instance.SetLevel(level);
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }
    }

    private void Die()
    {
        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessage("You died!", Color.red);
        }
        else
        {
            UIManager.Instance.AddMessage($"{name} is dead!", Color.green);
            GameManager.Get.RemoveEnemy(this);
        }

        Actor gravestoneActor = GameManager.Get.CreateActor("Dead", transform.position);
        if (gravestoneActor != null)
        {
            gravestoneActor.name = $"Remains of {name}";
        }

        Destroy(gameObject);
    }

    public void DoDamage(int hp, Actor attacker)
    {
        hitPoints -= hp;
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }

        if (hitPoints == 0)
        {
            Die();
            if (attacker != null && attacker.GetComponent<Player>() != null)
            {
                attacker.AddXp(xp);
            }
        }
    }

    public void Move(Vector3 direction)
    {
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction;
        }
    }

    public void UpdateFieldOfView()
    {
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);

        FieldOfView.Clear();
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        if (GetComponent<Player>())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }

    public void Heal(int hp)
    {
        int healAmount = Mathf.Min(maxHitPoints - hitPoints, hp);
        hitPoints += healAmount;

        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.AddMessage($"You were healed for {healAmount} HP!", Color.green);
        }
    }
}
