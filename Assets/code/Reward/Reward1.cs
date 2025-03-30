using UnityEngine;

public class Reward1 : MonoBehaviour
{
    public float autoDestroyTime = 8f;  // Auto destroy after this time
    public float effectDuration = 8f;   // Duration of the power-up effect

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
            Debug.Log("Player collected the BombLarger power-up (Tag: Reward).");

            // Activate the bomb larger effect via the manager
            BombEffectManager.instance.ActivateBombLarger(effectDuration);

            // Destroy the reward object immediately upon collision
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Reward auto-destroyed after " + autoDestroyTime + " seconds.");
    }
}
