using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Bool to indicate if the ladder goes up
    [SerializeField]
    private bool up;

    public bool Up
    {
        get { return up; }
        set { up = value; }
    }

    void Start()
    {
        // Add this ladder to the GameManager
        GameManager.Get.AddLadder(this);
    }
}
