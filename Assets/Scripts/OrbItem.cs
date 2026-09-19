using UnityEngine;

public class OrbItem : MonoBehaviour
{
    public int value = 10;

    public void OnCollected()
    {
        // Add your custom resource/score logic here
        Debug.Log($"Collected Orb worth {value} points!");
    }
}