using UnityEngine;

public class HealthReward : MonoBehaviour
{
    public float autoDestroyTime = 8f;  // Auto destroy after this time

    private void Start()
    {
        // Schedule the reward to be destroyed after a set time
        Destroy(gameObject, autoDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the tag "Player"
        if (collision.CompareTag("Player"))
        {

            // Increase health if possible
            if (PlayerHealthController.instance != null)
            {
                PlayerHealthController.instance.IncreaseHealth();
            }
            else
            {
                Debug.LogError("PlayerHealthController instance is null.");
            }

            // Destroy the reward object immediately upon collision
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
    }
}
