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
            Debug.Log("BombEffectManager initialized and set to DontDestroyOnLoad.");
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if they exist
            Debug.Log("Duplicate BombEffectManager destroyed.");
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

        Debug.Log("Bomb explosion radius increased to 2. Power-up activated!");
        Debug.Log("Power-up effect started. Duration: " + duration + " seconds.");

        yield return new WaitForSeconds(duration);

        BombRadiusManager.explosionRadius = 1;
        isEffectActive = false;

        Debug.Log("Bomb explosion radius reverted to 1. Power-up effect ended.");
        Debug.Log("Power-up duration ended after " + duration + " seconds.");
    }
}
