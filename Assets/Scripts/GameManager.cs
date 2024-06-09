using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // List of enemies
    private List<Actor> enemies = new List<Actor>();

    // List of consumables
    private List<Consumable> items = new List<Consumable>();

    // List of ladders
    private List<Ladder> ladders = new List<Ladder>();

    // List of tombstones
    private List<Tombstone> tombstones = new List<Tombstone>();

    // Variable for the player
    private Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: ensure the GameManager persists between scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Get { get => instance; }

    // Function to add an enemy to the list
    public void AddEnemy(Actor enemy)
    {
        enemies.Add(enemy);
    }

    // Function to set the player
    public void SetPlayer(Player player)
    {
        this.player = player;
        LoadPlayerData(); // Load player data when player is set
    }

    // Function to get the player
    public Player GetPlayer()
    {
        return player;
    }

    // Function to get an actor at a specific location
    public Actor GetActorAtLocation(Vector3 location)
    {
        // Check if the location matches the player's position
        if (player != null && player.transform.position == location)
        {
            return player;
        }

        // Check if the location matches an enemy's position
        foreach (Actor enemy in enemies)
        {
            if (enemy.transform.position == location)
            {
                return enemy;
            }
        }

        // If no actor is found at the location, return null
        return null;
    }

    // Function to create a new actor
    public Actor CreateActor(string actorType, Vector2 position)
    {
        GameObject actorPrefab = Resources.Load<GameObject>($"Prefabs/{actorType}");
        if (actorPrefab != null)
        {
            GameObject actorObject = Instantiate(actorPrefab, position, Quaternion.identity);
            Actor actorComponent = actorObject.GetComponent<Actor>();
            if (actorComponent != null)
            {
                // Add as enemy or player, depending on the type
                if (actorType == "Player")
                {
                    SetPlayer(actorComponent as Player);
                }
                else
                {
                    AddEnemy(actorComponent);
                }
                return actorComponent;
            }
        }
        Debug.LogError($"Actor type '{actorType}' could not be created.");
        return null;
    }

    // Function to start the turn for all enemies
    public void StartEnemyTurn()
    {
        foreach (Actor enemyActor in enemies)
        {
            Enemy enemyComponent = enemyActor.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RunAI();
            }
            else
            {
                Debug.LogError("Enemy component not found on Actor.");
            }
        }
    }

    // Function to remove an enemy from the list of enemies
    public void RemoveEnemy(Actor enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Destroy(enemy.gameObject);  // Remove the enemy GameObject from the scene
        }
        else
        {
            Debug.LogError("Enemy not found in the list.");
        }
    }

    // Function to add an item to the list
    public void AddItem(Consumable item)
    {
        items.Add(item);
    }

    // Function to remove an item from the list
    public void RemoveItem(Consumable item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Destroy(item.gameObject);  // Remove the item GameObject from the scene
        }
        else
        {
            Debug.LogError("Item not found in the list.");
        }
    }

    // Function to get an item at a specific location
    public Consumable GetItemAtLocation(Vector3 location)
    {
        foreach (Consumable item in items)
        {
            if (item.transform.position == location)
            {
                return item;
            }
        }
        // If no item is found at the location, return null
        return null;
    }

    // Function to get nearby enemies within a certain range of a location
    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();
        foreach (Actor enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, location) < 5)
            {
                nearbyEnemies.Add(enemy);
            }
        }
        return nearbyEnemies;
    }

    // Function to add a ladder to the list
    public void AddLadder(Ladder ladder)
    {
        ladders.Add(ladder);
    }

    // Function to get a ladder at a specific location
    public Ladder GetLadderAtLocation(Vector3 location)
    {
        foreach (Ladder ladder in ladders)
        {
            if (ladder.transform.position == location)
            {
                return ladder;
            }
        }
        // If no ladder is found at the location, return null
        return null;
    }

    // Function to add a tombstone to the list
    public void AddTombstone(Tombstone stone)
    {
        tombstones.Add(stone);
    }

    // Function to clear all floor elements
    public void ClearFloor()
    {
        // Destroy all enemies
        foreach (Actor enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();

        // Destroy all items
        foreach (Consumable item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();

        // Destroy all ladders
        foreach (Ladder ladder in ladders)
        {
            Destroy(ladder.gameObject);
        }
        ladders.Clear();

        // Destroy all tombstones
        foreach (Tombstone stone in tombstones)
        {
            Destroy(stone.gameObject);
        }
        tombstones.Clear();
    }

    // Function to save player data
    public void SavePlayerData()
    {
        if (player != null)
        {
            PlayerPrefs.SetInt("MaxHitPoints", player.MaxHitPoints);
            PlayerPrefs.SetInt("HitPoints", player.HitPoints);
            PlayerPrefs.SetInt("Defense", player.Defense);
            PlayerPrefs.SetInt("Power", player.Power);
            PlayerPrefs.SetInt("Level", player.Level);
            PlayerPrefs.SetInt("XP", player.XP);
            PlayerPrefs.SetInt("XpToNextLevel", player.XpToNextLevel);
            PlayerPrefs.SetInt("Floor", 1); // Player always starts on floor 1
            PlayerPrefs.Save();
        }
    }

    // Function to load player data
    public void LoadPlayerData()
    {
        if (player != null)
        {
            player.MaxHitPoints = PlayerPrefs.GetInt("MaxHitPoints", 100);
            player.HitPoints = PlayerPrefs.GetInt("HitPoints", 100);
            player.Defense = PlayerPrefs.GetInt("Defense", 10);
            player.Power = PlayerPrefs.GetInt("Power", 10);
            player.Level = PlayerPrefs.GetInt("Level", 1);
            player.XP = PlayerPrefs.GetInt("XP", 0);
            player.XpToNextLevel = PlayerPrefs.GetInt("XpToNextLevel", 100);
            int savedFloor = PlayerPrefs.GetInt("Floor", 1);

            // If the saved floor is not the same as the current floor, clear save data
            if (savedFloor != 1)
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }
    }

    // Function to remove the save data
    public void RemoveSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
