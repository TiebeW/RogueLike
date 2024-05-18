using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private Actor actorComponent;

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
        if (context.performed)
        {
            Move();
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        // Handle exit logic if necessary
    }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Debug.Log("roundedDirection");
        Action.Move(actorComponent, roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }
}
