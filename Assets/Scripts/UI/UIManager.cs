using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("Documents")]
    public GameObject HealthBar; // Ensure this is assigned in the inspector
    public GameObject Messages;
    public GameObject Inventory; // New inventory GameObject variable

    private HealthBar healthBar;
    private Messages messagesController;
    private InventoryUI inventoryUI; // InventoryUI component variable

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Get the script components from the GameObjects
        if (HealthBar != null)
        {
            // HealthBar setup
        }

        if (Messages != null)
        {
            // Messages setup
        }

        if (Inventory != null)
        {
            Debug.Log("Inventory GameObject assigned.");
            inventoryUI = Inventory.GetComponent<InventoryUI>();
            if (inventoryUI == null)
            {
                Debug.LogError("InventoryUI component is not found on the assigned Inventory GameObject!");
            }
            else
            {
                Debug.Log("InventoryUI component found successfully.");
            }
        }
        else
        {
            Debug.LogError("Inventory is not assigned in the UIManager!");
        }

        // Initial clear and welcome message
        if (messagesController != null)
        {
            messagesController.Clear();
            messagesController.AddMessage("Welcome to the dungeon, Adventurer!", Color.yellow);
        }
    }

    // Public getter for accessing the InventoryUI component
    public InventoryUI InventoryUI
    {
        get
        {
            return inventoryUI;
        }
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthBar != null)
        {
            healthBar.SetValues(current, max);
        }
        else
        {
            Debug.LogError("HealthBar component is not assigned!");
        }
    }

    public void AddMessage(string message, Color color)
    {
        if (messagesController != null)
        {
            messagesController.AddMessage(message, color);
        }
    }
}
