using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthLabel;
    private Label levelLabel; 
    private Label xpLabel; 

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        healthBar = root.Q<VisualElement>("HealthBar");
        healthLabel = root.Q<Label>("HealthText");
        levelLabel = root.Q<Label>("LevelText");
        xpLabel = root.Q<Label>("XPText"); 
    }

    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        float percent = (float)currentHitPoints / maxHitPoints * 100;
        healthBar.style.width = new Length(percent, LengthUnit.Percent);
        healthLabel.text = $"{currentHitPoints}/{maxHitPoints} HP";
    }

    public void SetLevel(int level)
    {
        levelLabel.text = $"Level: {level}";
    }

    public void SetXP(int xp)
    {
        xpLabel.text = $"XP: {xp}";
    }
}
