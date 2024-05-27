using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Lijst van vijanden
    private List<Actor> enemies = new List<Actor>();

    // Variabele voor de speler
    private Actor player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Optioneel: zorg ervoor dat de GameManager blijft bestaan tussen scènes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Get { get => instance; }

    // Functie om een vijand toe te voegen aan de lijst
    public void AddEnemy(Actor enemy)
    {
        enemies.Add(enemy);
    }

    // Functie om de speler in te stellen
    public void SetPlayer(Actor player)
    {
        this.player = player;
    }

    // Functie om de speler op te halen
    public Actor GetPlayer()
    {
        return player;
    }

    // Functie om een actor op een locatie te krijgen
    public Actor GetActorAtLocation(Vector3 location)
    {
        // Controleer of de locatie gelijk is aan de positie van de speler
        if (player != null && player.transform.position == location)
        {
            return player;
        }

        // Controleer of de locatie gelijk is aan de positie van een vijand
        foreach (Actor enemy in enemies)
        {
            if (enemy.transform.position == location)
            {
                return enemy;
            }
        }

        // Als geen actor gevonden is op de locatie, geef null terug
        return null;
    }

    // Functie om een nieuwe Actor te creëren
    public Actor CreateActor(string actorType, Vector2 position)
    {
        GameObject actorPrefab = Resources.Load<GameObject>($"Prefabs/{actorType}");
        if (actorPrefab != null)
        {
            GameObject actorObject = Instantiate(actorPrefab, position, Quaternion.identity);
            Actor actorComponent = actorObject.GetComponent<Actor>();
            if (actorComponent != null)
            {
                // Voeg toe als vijand of speler, afhankelijk van het type
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

    // Functie om de beurt van alle vijanden te starten
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

    // Functie om een vijand uit de lijst met vijanden te verwijderen
    public void RemoveEnemy(Actor enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Destroy(enemy.gameObject);  // Verwijder het vijand GameObject uit de scène
        }
        else
        {
            Debug.LogError("Enemy not found in the list.");
        }
    }
}
