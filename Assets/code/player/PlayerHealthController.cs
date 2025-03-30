using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;   // 声明静态变量

public int currentHealth, maxHealth;             // 当前生命和最大生命

[Header("Health Bar UI")]
public Sprite healthBarSprite;          // 用于显示每格生命的 Sprite
public Transform healthBarContainer;    // 血条容器（建议作为玩家子物体放在头顶）
private List<GameObject> healthBars = new List<GameObject>();  // 存储生成的血条对象
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
// 生成血条，如果 healthBarSprite 和 healthBarContainer 均已设置
if (healthBarSprite != null && healthBarContainer != null)
{
    float spacing = 0.3f; // 每个血条之间的间隔
    // 计算总宽度（基于 maxHealth）
    float totalWidth = (maxHealth - 1) * spacing;
    // 让第一个血条居中，即起始 x 坐标为 -totalWidth/2
    float startX = -totalWidth / 2f;
    for (int i = 0; i < maxHealth; i++)
    {
        GameObject bar = new GameObject("HealthBar_" + i);
        // 将 bar 设为 healthBarContainer 的子物体
        bar.transform.SetParent(healthBarContainer);
        // 设置局部位置，横向排列
        bar.transform.localPosition = new Vector3(startX + i * spacing, 0, 0);
        // 添加 SpriteRenderer，并设置为 healthBarSprite
        SpriteRenderer sr = bar.AddComponent<SpriteRenderer>();
        sr.sprite = healthBarSprite;
        // 设置排序层级，确保血条显示在玩家上层（可根据需要调整）
        sr.sortingOrder = 10;
        healthBars.Add(bar);
    }
}
    }

    void Update()
    {
        // 此处可以加入其他玩家健康相关逻辑
    }

    public void DealDamage()
    {
        currentHealth--;
        AudioManager.instance.PlaySFX("hurt");

// 更新血条：移除最后一个血条，并重新排列剩余的血条
if (healthBars.Count > 0)
{
    GameObject bar = healthBars[healthBars.Count - 1];
    healthBars.RemoveAt(healthBars.Count - 1);
    Destroy(bar);
    UpdateHealthBarPositions();
}
        if (currentHealth <= 0)
        {
            // 禁用所有爆炸的碰撞体，防止后续爆炸伤害其他对象
            DisableAllExplosionColliders();

            // 遍历场上所有 Bomb，并禁用它们的爆炸功能
            GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
            foreach (GameObject bombObj in bombs)
            {
                Bomb bomb = bombObj.GetComponent<Bomb>();
                if (bomb != null)
                {
                    bomb.disableExplode = true;
                }
            }

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
        Debug.Log("Health increased by 1. Current Health: " + currentHealth);
    }
    else
    {
        Debug.Log("Health is already at maximum.");
    }
}

// 更新血条排列，使剩余血条以 healthBarContainer 为中心左右排列
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
    }

    private void DisableAllExplosionColliders()
    {
        GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        foreach (GameObject exp in explosions)
        {
            Collider2D col = exp.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }
}
