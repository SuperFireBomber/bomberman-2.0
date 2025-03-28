using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionCollision : MonoBehaviour
{
    // 记录本帧内已被伤害的对象，防止重复伤害
    private static HashSet<GameObject> damagedObjects = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !damagedObjects.Contains(other.gameObject))
        {
            damagedObjects.Add(other.gameObject);
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableCollider();
            }
            if (PlayerHealthController.instance != null)
            {
                PlayerHealthController.instance.DealDamage();
                AudioManager.instance.PlaySFX("hurt");
            }
            StartCoroutine(RemoveDamageFlag(other.gameObject));
        }
        else if (other.CompareTag("Enemy") && !damagedObjects.Contains(other.gameObject))
        {
            damagedObjects.Add(other.gameObject);
            EnemyControl enemy = other.GetComponent<EnemyControl>();
            if (enemy != null)
            {
                enemy.ApplyExplosionDamage();
            }
            StartCoroutine(RemoveDamageFlag(other.gameObject));
        }
    }

    private IEnumerator RemoveDamageFlag(GameObject obj)
    {
        yield return new WaitForEndOfFrame();
        damagedObjects.Remove(obj);
    }
}
