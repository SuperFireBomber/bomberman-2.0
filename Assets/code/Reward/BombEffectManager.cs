using UnityEngine;
using System.Collections;

public class BombEffectManager : MonoBehaviour
{
    public static BombEffectManager instance;
    private bool isEffectActive = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents destruction on scene load
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if they exist
        }
    }

    public void ActivateBombLarger(float duration)
    {
        if (!isEffectActive)
        {
            StartCoroutine(BombRadiusBoost(duration));
        }
    }

    private IEnumerator BombRadiusBoost(float duration)
    {
        isEffectActive = true;
        BombRadiusManager.explosionRadius = 2;

        yield return new WaitForSeconds(duration);

        BombRadiusManager.explosionRadius = 1;
        isEffectActive = false;
    }
}
