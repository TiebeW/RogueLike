using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private Actor actorComponent;
    public Inventory inventory;

    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;

    private void Awake()
    {
        controls = new Controls();
    }

    private void Start()
    {
        // Set the Camera position
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);

        // Get the Actor component
        actorComponent = GetComponent<Actor>();

        // Set this player in the GameManager
        if (actorComponent != null)
        {
            GameManager.Get.SetPlayer(actorComponent);
        }
        else
        {
            Debug.LogError("Actor component not found on Player.");
        }

        // Ensure inventory is not null
        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
            if (inventory == null)
            {
                Debug.LogError("Inventory component not found on Player.");
            }
        }
    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (inventoryIsOpen)
        {
            Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();

            if (direction.y > 0)
            {
                UIManager.Instance.InventoryUI.SelectPreviousItem();
            }
            else if (direction.y < 0)
            {
                UIManager.Instance.InventoryUI.SelectNextItem();
            }
        }
        else
        {
            if (context.performed)
            {
                Move();
            }
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                UIManager.Instance.InventoryUI.Hide();

                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
            else
            {
                // Handle exit logic here if needed when the inventory is not open
            }
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleGrab();
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                UIManager.Instance.InventoryUI.Show(inventory.Items);
                inventoryIsOpen = true;
                droppingItem = true;
            }
            else
            {
                HandleDrop();
            }
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                UIManager.Instance.InventoryUI.Show(inventory.Items);
                inventoryIsOpen = true;
                usingItem = true;
            }
            else
            {
                UseItem();
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context) { }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Action.MoveOrHit(actorComponent, roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    private void HandleGrab()
    {
        Consumable item = GameManager.Get.GetItemAtLocation(transform.position);
        if (item == null)
        {
            Debug.Log("No item at the player's location.");
        }
        else
        {
            if (inventory.AddItem(item))
            {
                item.gameObject.SetActive(false);
                GameManager.Get.RemoveItem(item);
                Debug.Log("Item added to inventory.");
            }
        }
    }

    private void HandleDrop()
    {
        if (inventory.Items.Count > 0)
        {
            Consumable itemToDrop = inventory.Items[inventory.Items.Count - 1]; // Drop the last item added
            inventory.DropItem(itemToDrop);
            itemToDrop.transform.position = transform.position; // Drop the item at the player's position
            itemToDrop.gameObject.SetActive(true);
            GameManager.Get.AddItem(itemToDrop); // Add the item back to the GameManager
            Debug.Log("Item dropped from inventory.");
        }
        else
        {
            Debug.Log("No items to drop.");
        }
    }

    private void UseItem()
    {
        if (usingItem)
        {
            Consumable item = inventory.SelectedItem();
            if (item != null)
            {
                if (item.Type == Consumable.ItemType.HealthPotion)
                {
                    actorComponent.Heal(item.HealingAmount);
                    UIManager.Instance.AddMessage($"You use a health potion and heal for {item.HealingAmount} HP.", Color.green);
                }
                else if (item.Type == Consumable.ItemType.Fireball)
                {
                    List<Actor> nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
                    foreach (Actor enemy in nearbyEnemies)
                    {
                        Action.Hit(actorComponent, enemy);
                        UIManager.Instance.AddMessage($"You cast a fireball, dealing {item.Damage} damage to {enemy.name}.", Color.red);
                    }
                }
                else if (item.Type == Consumable.ItemType.ScrollOfConfusion)
                {
                    List<Actor> nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
                    foreach (Actor enemy in nearbyEnemies)
                    {
                        Enemy enemyComponent = enemy.GetComponent<Enemy>();
                        if (enemyComponent != null)
                        {
                            enemyComponent.Confuse(item.ConfusionDuration);
                            UIManager.Instance.AddMessage($"You used a scroll of confusion, confusing {enemy.name}.", Color.blue);
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("No item selected.");
            }
        }
    }
}
