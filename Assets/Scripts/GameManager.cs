using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // List of enemies
    private List<Actor> enemies = new List<Actor>();

    // List of consumables
    private List<Consumable> items = new List<Consumable>();

    // Variable for the player
    private Actor player;

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
    public void SetPlayer(Actor player)
    {
        this.player = player;
    }

    // Function to get the player
    public Actor GetPlayer()
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
                    SetPlayer(actorComponent);
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
}
