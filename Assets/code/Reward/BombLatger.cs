using UnityEngine;

public class BombLarger : MonoBehaviour
{
    public float autoDestroyTime = 8f;
    public float effectDuration = 7f;

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
                BombEffectManager.instance.ActivateBombLarger(effectDuration);
                RewardUIManager.instance.ShowBombRangeUI(effectDuration);
            }

            Destroy(gameObject);
        }
    }
}
