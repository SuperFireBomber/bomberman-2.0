using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Static counter to track the number of bombs currently in the scene.
    public static int currentBombCount = 0;

    // Time (in seconds) before the bomb explodes.
    public float explosionDelay = 5f;

    // Explosion radius in grid units (1 means explosion covers bomb cell plus adjacent cells up/down/left/right).
    public int explosionRadius = 1;

    // Explosion prefab to instantiate explosion effect.
    public GameObject explosionPrefab;

    // Layer mask for detecting obstacles (walls) that can be destroyed.
    public LayerMask obstacleLayer;

    private Collider2D bombCollider;

    void Start()
    {
        // Increment bomb counter.
        currentBombCount++;

        // Set the bomb's scale to 0.9 to match the desired size.
        transform.localScale = new Vector3(0.9f, 0.9f, 1f);

        // Get the bomb's collider.
        bombCollider = GetComponent<Collider2D>();

        // Always set the collider as trigger so the player can pass through.
        bombCollider.isTrigger = true;

        // Start the explosion countdown.
        StartCoroutine(ExplosionCountdown());
    }

    IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        // Determine the bomb's grid position (rounding to nearest integer).
        Vector2 bombGridPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        // Create explosion in the center.
        CreateExplosion(bombGridPosition);

        // Define four cardinal directions.
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        // Generate explosion in each direction.
        foreach (Vector2 dir in directions)
        {
            for (int i = 1; i <= explosionRadius; i++)
            {
                Vector2 pos = bombGridPosition + dir * i;

                // Check if there is an obstacle (wall) at the position.
                Collider2D hit = Physics2D.OverlapPoint(pos, obstacleLayer);
                if (hit != null)
                {
                    // Destroy the wall and create explosion effect at this cell.
                    Destroy(hit.gameObject);
                    CreateExplosion(pos);
                    // Stop further explosion propagation in this direction.
                    break;
                }
                else
                {
                    // Create explosion effect in this cell.
                    CreateExplosion(pos);
                }
            }
        }

        // Finally, destroy the bomb object.
        Destroy(gameObject);
    }

    void CreateExplosion(Vector2 position)
    {
        // Instantiate the explosion effect at the given grid position.
        if (explosionPrefab != null)
        {
            // Set the z-position to -2 so explosion is rendered above walls if needed.
            GameObject explosionEffect = Instantiate(explosionPrefab, new Vector3(position.x, position.y, -2), Quaternion.identity);
            // Destroy explosion after 2 seconds (adjustable as needed)
            Destroy(explosionEffect, 2f);
        }
    }

    // Removed OnTriggerExit2D and related coroutine to allow the player to always pass through the bomb.

    // Decrement bomb counter when this bomb is destroyed.
    void OnDestroy()
    {
        if (currentBombCount > 0)
            currentBombCount--;
    }
}