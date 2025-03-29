using UnityEngine;

public class RewardTouchPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Destroy the reward object
            Destroy(gameObject);
        }
    }
}
