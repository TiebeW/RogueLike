using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Enemy : MonoBehaviour
{
    private void Start()
    {
        // Haal het Actor-component op en voeg het toe aan de GameManager
        Actor actor = GetComponent<Actor>();
        GameManager.Get.AddEnemy(actor);
    }
}
