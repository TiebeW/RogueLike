using UnityEngine;
using UnityEngine.UI;

public class FloorInfo : MonoBehaviour
{
    public Text floorText;
    public Text enemiesText;

    public void SetFloor(int floorNumber)
    {
        floorText.text = "Floor " + floorNumber;
    }

    public void SetEnemiesLeft(int enemiesLeft)
    {
        enemiesText.text = enemiesLeft + " enemies left";
    }
}
