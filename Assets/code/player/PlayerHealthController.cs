using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth, maxHealth;

    [Header("Health Bar UI")]
    public Sprite healthBarSprite;
    public Transform healthBarContainer;
    private List<GameObject> healthBars = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;

        // 创建初始血条
        GenerateHealthBars();
    }

    void Update()
    {
        // 可以在这里加入其他玩家健康相关逻辑
    }

    public void DealDamage()
    {
        currentHealth--;
        AudioManager.instance.PlaySFX("hurt");

        // 更新血条：移除最后一个血条
        if (healthBars.Count > 0)
        {
            GameObject bar = healthBars[healthBars.Count - 1];
            healthBars.RemoveAt(healthBars.Count - 1);
            Destroy(bar);
            UpdateHealthBarPositions();
        }

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX("steel-pipe");
            gameObject.SetActive(false);
            GameOverManager.instance.ShowGameOver();
        }
    }

    public void IncreaseHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            AudioManager.instance.PlaySFX("heal");

            // 更新血条：增加一个血条
            AddHealthBar();
            UpdateHealthBarPositions();
        }
        else
        {
        }
    }

    private void GenerateHealthBars()
    {
        if (healthBarSprite != null && healthBarContainer != null)
        {
            float spacing = 0.3f;
            float totalWidth = (maxHealth - 1) * spacing;
            float startX = -totalWidth / 2f;

            for (int i = 0; i < maxHealth; i++)
            {
                CreateHealthBar(i, startX, spacing);
            }
        }
    }

    private void CreateHealthBar(int index, float startX, float spacing)
    {
        GameObject bar = new GameObject("HealthBar_" + index);
        bar.transform.SetParent(healthBarContainer);
        bar.transform.localPosition = new Vector3(startX + index * spacing, 0, 0);
        SpriteRenderer sr = bar.AddComponent<SpriteRenderer>();
        sr.sprite = healthBarSprite;
        sr.sortingOrder = 10;
        healthBars.Add(bar);
    }

    private void AddHealthBar()
    {
        if (healthBarSprite != null && healthBarContainer != null)
        {
            float spacing = 0.3f;
            float startX = -((healthBars.Count) * spacing) / 2f;
            CreateHealthBar(healthBars.Count, startX, spacing);
        }
    }

    private void UpdateHealthBarPositions()
    {
        float spacing = 0.3f;
        int count = healthBars.Count;
        float totalWidth = (count - 1) * spacing;
        float startX = -totalWidth / 2f;
        for (int i = 0; i < count; i++)
        {
            healthBars[i].transform.localPosition = new Vector3(startX + i * spacing, 0, 0);
        }
    }
}
