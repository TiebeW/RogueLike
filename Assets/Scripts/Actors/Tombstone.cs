using UnityEngine;

public class Tombstone : MonoBehaviour
{
    private void Start()
    {
        // Voeg deze tombstone toe aan de GameManager
        GameManager.Get.AddTombStone(this);
    }
}
