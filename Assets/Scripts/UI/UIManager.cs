using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Documents")]
    public GameObject HealthBar;
    public GameObject Messages;
    public GameObject Inventory;
    public GameObject FloorInfo;

    private HealthBar healthBar;
    private Messages messagesController;
    private InventoryUI inventoryUI;
    private Text floorText;
    private Text enemiesText;

    private void Awake()
    {
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
        if (HealthBar != null)
        {
            healthBar = HealthBar.GetComponent<HealthBar>();
        }

        if (Messages != null)
        {
            messagesController = Messages.GetComponent<Messages>();
        }

        if (Inventory != null)
        {
            inventoryUI = Inventory.GetComponent<InventoryUI>();
            if (inventoryUI == null)
            {
                Debug.LogError("InventoryUI component is not found on the assigned Inventory GameObject!");
            }
        }
        else
        {
            Debug.LogError("Inventory is not assigned in the UIManager!");
        }

        if (FloorInfo != null)
        {
            floorText = FloorInfo.GetComponentInChildren<Text>();
            enemiesText = FloorInfo.GetComponentsInChildren<Text>()[1];
        }
        else
        {
            Debug.LogError("FloorInfo is not assigned in the UIManager!");
        }

        if (messagesController != null)
        {
            messagesController.Clear();
            messagesController.AddMessage("Welcome to the dungeon, Adventurer!", Color.yellow);
        }
    }

    public InventoryUI InventoryUI => inventoryUI;

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

    public void SetLevel(int level)
    {
        if (healthBar != null)
        {
            healthBar.SetLevel(level);
        }
        else
        {
            Debug.LogError("HealthBar component is not assigned!");
        }
    }

    public void SetXP(int xp)
    {
        if (healthBar != null)
        {
            healthBar.SetXP(xp);
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

    // Nieuwe methoden toegevoegd
    public void UpdateFloorInfo(int floorNumber)
    {
        if (floorText != null)
        {
            floorText.text = "Floor " + floorNumber;
        }
    }

    public void UpdateEnemiesInfo(int enemiesLeft)
    {
        if (enemiesText != null)
        {
            enemiesText.text = enemiesLeft + " enemies left";
        }
    }
}
