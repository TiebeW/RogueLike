using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("Documents")]
    public GameObject HealthBar; // Ensure this is assigned in the inspector
    public GameObject Messages;

    private HealthBar healthBar;
    private Messages messagesController;

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
            Debug.Log("HealthBar GameObject is assigned in the Inspector.");
            healthBar = HealthBar.GetComponent<HealthBar>();
            if (healthBar == null)
            {
                Debug.LogError("HealthBar component is not found on the assigned HealthBarObject!");
            }
            else
            {
                Debug.Log("HealthBar component found successfully.");
            }
        }
        else
        {
            Debug.LogError("HealthBar is not assigned in the UIManager!");
        }

        if (Messages != null)
        {
            Debug.Log("Messages GameObject assigned.");
            messagesController = Messages.GetComponent<Messages>();
            if (messagesController == null)
            {
                Debug.LogError("Messages component is not found on the assigned MessagesObject!");
            }
            else
            {
                Debug.Log("Messages component found successfully.");
            }
        }
        else
        {
            Debug.LogError("Messages is not assigned in the UIManager!");
        }

        // Initial clear and welcome message
        if (messagesController != null)
        {
            messagesController.Clear();
            messagesController.AddMessage("Welcome to the dungeon, Adventurer!", Color.yellow);
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
