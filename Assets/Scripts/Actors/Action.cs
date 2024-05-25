using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void MoveOrHit(Actor actor, Vector2 direction)
    {
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);
        if (target == null)
        {
            Move(actor, direction);
        }
        else
        {
            Hit(actor, target);
        }
        EndTurn(actor);
    }

    static public void Move(Actor actor, Vector2 direction)
    {
        if (MapManager.Get.IsWalkable(actor.transform.position + (Vector3)direction))
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }
    }

    static public void Hit(Actor actor, Actor target)
    {
        int damage = actor.Power - target.Defense;
        if (damage > 0)
        {
            target.DoDamage(damage);
            if (actor.GetComponent<Player>() != null)
            {
                UIManager.Instance.AddMessage($"{actor.name} hits {target.name} for {damage} damage!", Color.white);
            }
            else
            {
                UIManager.Instance.AddMessage($"{actor.name} hits {target.name} for {damage} damage!", Color.red);
            }
        }
        else
        {
            if (actor.GetComponent<Player>() != null)
            {
                UIManager.Instance.AddMessage($"{actor.name} hits {target.name} but does no damage.", Color.white);
            }
            else
            {
                UIManager.Instance.AddMessage($"{actor.name} hits {target.name} but does no damage.", Color.red);
            }
        }
    }

    static private void EndTurn(Actor actor)
    {
        if (actor.GetComponent<Player>() != null)
        {
            GameManager.Get.StartEnemyTurn();
        }
    }
}
