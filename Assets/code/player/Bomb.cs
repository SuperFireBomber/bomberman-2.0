using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static int currentBombCount = 0;

    public float explosionDelay = 5f;
    public GameObject explosionPrefab;
    public LayerMask obstacleLayer;

    private Collider2D bombCollider;
    public bool disableExplode = false;



    void Start()
    {
        currentBombCount++;
        transform.localScale = new Vector3(0.9f, 0.9f, 1f);
        bombCollider = GetComponent<Collider2D>();
        bombCollider.isTrigger = true;

        Vector3 snappedPos = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            transform.position.z
        );
        transform.position = snappedPos;

        StartCoroutine(ExplosionCountdown());
    }

    IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
if (disableExplode)
    return;
// Determine the bomb's grid position (rounding to nearest integer).

        Vector2 bombGridPosition = new Vector2(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y)
        );


        CreateExplosion(bombGridPosition);
        AudioManager.instance.PlaySFX("explosion");

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in directions)
        {
            for (int i = 1; i <= BombRadiusManager.explosionRadius; i++)
            {
                Vector2 pos = bombGridPosition + dir * i;
                Collider2D hit = Physics2D.OverlapPoint(pos, obstacleLayer);
                if (hit != null)
                {
                    if (hit.gameObject.CompareTag("obstacle"))
                    {
                        Destroy(hit.gameObject);
                        CreateExplosion(pos);
                    }
                    break;
                }
                else
                {
                    CreateExplosion(pos);
                }
            }
        }

        Destroy(gameObject);
    }

    void CreateExplosion(Vector2 position)
    {
        if (explosionPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionPrefab, new Vector3(position.x, position.y, -2), Quaternion.identity);
            Destroy(explosionEffect, 2f);
        }
    }

    void OnDestroy()
    {
        if (currentBombCount > 0)
            currentBombCount--;
    }
}
