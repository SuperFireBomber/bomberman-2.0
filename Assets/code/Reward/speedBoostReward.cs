using UnityEngine;

public class SpeedBoostReward : MonoBehaviour
{
    public float autoDestroyTime = 8f;  // Auto destroy after this time
    public float duration = 7f;         // Duration of the power-up effect

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
            Debug.Log("Player collected the SpeedBoost power-up.");

            // Activate the speed boost effect via the PlayerController
            if (PlayerController.instance != null)
            {
                PlayerController.instance.ActivateSpeedBoost(duration);
            }
            else
            {
                Debug.LogError("PlayerController instance is null.");
            }

            // Destroy the reward object immediately upon collision
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("SpeedBoost Reward auto-destroyed after " + autoDestroyTime + " seconds.");
    }
}
