using UnityEngine;

public class MaxBombsReward : MonoBehaviour
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
            Debug.Log("Player collected the MaxBombs power-up.");

            // Activate the max bombs increase effect via the PlayerController
            if (PlayerController.instance != null)
            {
                PlayerController.instance.ActivateMaxBombsBoost(duration);
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
        Debug.Log("MaxBombs Reward auto-destroyed after " + autoDestroyTime + " seconds.");
    }
}
