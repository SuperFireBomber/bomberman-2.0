using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RewardUIManager : MonoBehaviour
{
    public static RewardUIManager instance;

    [Header("UI Elements")]
    public Image speedBoostImage;
    public TextMeshProUGUI speedBoostText;

    public Image maxBombsImage;
    public TextMeshProUGUI maxBombsText;

    public Image bombRangeImage;
    public TextMeshProUGUI bombRangeText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ShowSpeedBoostUI(float duration)
    {
        StartCoroutine(DisplayPowerUpUI(speedBoostImage, speedBoostText, duration));
    }

    public void ShowMaxBombsUI(float duration)
    {
        StartCoroutine(DisplayPowerUpUI(maxBombsImage, maxBombsText, duration));
    }

    public void ShowBombRangeUI(float duration)
    {
        StartCoroutine(DisplayPowerUpUI(bombRangeImage, bombRangeText, duration));
    }


    private IEnumerator DisplayPowerUpUI(Image image, TextMeshProUGUI text, float duration)
    {
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);

        float remainingTime = duration;
        while (remainingTime > 0)
        {
            text.text = Mathf.Ceil(remainingTime).ToString();
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
