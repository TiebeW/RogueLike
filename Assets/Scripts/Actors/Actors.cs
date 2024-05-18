using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Static method to end the turn
    static private void EndTurn(Actor actor)
    {
        // Check if the actor has a player component
        if (actor.GetComponent<Player>() != null)
        {
            // Call StartEnemyTurn from the GameManager
            GameManager.Get.StartEnemyTurn();
        }
    }

    // Static method to move an actor
    static public void Move(Actor actor, Vector2 direction)
    {
        // Check if there is an actor at the target position
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        // If no actor is at the target position, move the actor
        if (target == null)
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }

        // End turn in case this is the player
        EndTurn(actor);
    }
}
