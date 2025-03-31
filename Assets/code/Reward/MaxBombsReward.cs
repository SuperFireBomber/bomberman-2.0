using UnityEngine;

public class MaxBombsReward : MonoBehaviour
{
    public float autoDestroyTime = 8f;
    public float duration = 7f;

    private void Start()
    {
        Destroy(gameObject, autoDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (PlayerController.instance != null)
            {
                PlayerController.instance.ActivateMaxBombsBoost(duration);
                RewardUIManager.instance.ShowMaxBombsUI(duration);
            }

            Destroy(gameObject);
        }
    }
}
