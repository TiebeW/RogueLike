using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Lijst van vijanden
    private List<Actor> enemies = new List<Actor>();

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

    // Functie om een actor op een locatie te krijgen (nog te implementeren)
    public Actor GetActorAtLocation(Vector3 location)
    {
        return null;
    }

    // Voeg deze functie toe
    public Actor CreateActor(string actorType, Vector2 position)
    {
        GameObject actorPrefab = Resources.Load<GameObject>($"Prefabs/{actorType}");
        if (actorPrefab != null)
        {
            GameObject actorObject = Instantiate(actorPrefab, position, Quaternion.identity);
            Actor actorComponent = actorObject.GetComponent<Actor>();
            if (actorComponent != null)
            {
                AddEnemy(actorComponent);
                return actorComponent;
            }
        }
        Debug.LogError($"Actor type '{actorType}' could not be created.");
        return null;
    }
}
