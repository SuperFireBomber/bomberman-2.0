using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControl : MonoBehaviour
{
    [Header("移动相关")]
    public float cellSize = 1f;               // 格子大小
    public Vector2 startPosition = new Vector2(5, 5); // 初始位置（由 EnemyManager 设置）
    public LayerMask wallLayer;               // 检测墙壁的图层
    public EnemyManager enemyManager;         // 引用 EnemyManager

    public float baseSpeed = 2f;              // 基础速度（在 Inspector 中可以统一设置）
    public float speedMultiplier = 1f;        // 速度乘数（由 EnemyManager 传入，不同类型不同）
    private float moveSpeed;                  // 实际速度 = baseSpeed * speedMultiplier

    private Vector2 targetPosition;
    private bool isMoving = false;

    [Header("生命与无敌")]
    public int maxHealth = 1;                 // 敌人最大血量（不同类型不同）
    private int currentHealth;
    private bool isInvulnerable = false;      // 短暂无敌状态

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Health Bar UI")]
    public GameObject healthBarPrefab;      // 单格生命条预制体
    public Transform healthBarContainer;    // 存放生命条的容器（通常挂在 enemy 头顶的空物体）
    private List<GameObject> healthBars = new List<GameObject>();  // 用于记录每一格健康条

    void Start()
    {
        currentHealth = maxHealth;
        moveSpeed = baseSpeed * speedMultiplier;
        targetPosition = startPosition;
        transform.position = new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1);

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        // 生成健康条（如果预制体和容器都有设置）
        if (healthBarPrefab != null && healthBarContainer != null)
        {
            for (int i = 0; i < maxHealth; i++)
            {
                GameObject bar = Instantiate(healthBarPrefab, healthBarContainer);
                // 可根据需要调整 localPosition 使生命条横向排列，例如每个条之间间隔 0.3 个单位
                bar.transform.localPosition = new Vector3(i * 0.3f, 0, 0);
                healthBars.Add(bar);
            }
        }

    }

    void Update()
    {
        // 如果 Game Over 面板已显示，则不继续执行移动逻辑
        if (GameOverManager.instance != null && GameOverManager.instance.gameOverPanel.activeSelf)
            return;

        if (!isMoving)
        {
            Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            for (int i = 0; i < directions.Length; i++)
            {
                int randIndex = Random.Range(0, directions.Length);
                Vector2 direction = directions[randIndex];
                Vector2 newPosition = targetPosition + direction;
                Collider2D hit = Physics2D.OverlapPoint(new Vector2(newPosition.x * cellSize, newPosition.y * cellSize), wallLayer);
                if (hit != null)
                    continue;
                // 新增：检查新坐标是否已经被其他敌人占用（包括未移动的敌人）
                if (IsGridOccupied(newPosition))
                    continue;
                if (enemyManager != null && enemyManager.ReserveNextPosition(newPosition))
                {
                    targetPosition = newPosition;
                    isMoving = true;
                    break;
                }
            }
        }

        Vector3 targetWorldPos = new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1);
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            isMoving = false;
            if (enemyManager != null)
                enemyManager.ReleaseReservedPosition(targetPosition);
            transform.position = targetWorldPos;
        }
    }

    // 提供公开方法，由 ExplosionCollision 调用
    public void ApplyExplosionDamage()
    {
        if (!isInvulnerable)
        {
            StartCoroutine(DisableColliderCoroutine());
            TakeDamage();
        }
    }
    void TakeDamage()
    {
        currentHealth--;

        // 更新健康条：如果还有健康条，则移除最后一格
        if (healthBars.Count > 0)
        {
            GameObject bar = healthBars[healthBars.Count - 1];
            healthBars.RemoveAt(healthBars.Count - 1);
            Destroy(bar);
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX("enemy-die");
        }
        else
        {
            AudioManager.instance.PlaySFX("enemy-hurt");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableCollider();
            }

            // 扣血
            if (PlayerHealthController.instance != null)
            {
                PlayerHealthController.instance.DealDamage();
            }
        }
    }

    // 短暂无敌与透明效果（例如2秒），防止同一个炸弹重复伤害
    private IEnumerator DisableColliderCoroutine()
    {
        isInvulnerable = true;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0.5f; // 半透明
            spriteRenderer.color = c;
        }
        yield return new WaitForSeconds(2f);
        if (col != null)
        {
            col.enabled = true;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        isInvulnerable = false;
    }
    private bool IsGridOccupied(Vector2 pos)
    {
        EnemyControl[] enemies = FindObjectsOfType<EnemyControl>();
        foreach (EnemyControl enemy in enemies)
        {
            if (enemy == this)
                continue;
            // 将其他敌人的位置换算到网格坐标
            Vector2 enemyGridPos = new Vector2(
                Mathf.Round(enemy.transform.position.x / cellSize),
                Mathf.Round(enemy.transform.position.y / cellSize)
            );
            if (enemyGridPos == pos)
                return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        EnemyManager.currentEnemyCount--;
        // 保证在所有敌人销毁且VictoryManager存在的情况下才显示胜利界面
        if (EnemyManager.currentEnemyCount <= 0)
        {
            if (VictoryManager.instance != null)
            {
                {
                    // 禁用所有爆炸的碰撞体，防止后续爆炸伤害其他对象
                    DisableAllExplosionColliders();
                    VictoryManager.instance.ShowVictory();  // 此处调用 VictoryManager 的方法显示胜利界面
                }
            }
        }
    }
    private void DisableAllExplosionColliders()
    {
        // 找到所有标签为 "Explosion" 的游戏对象
        GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        foreach (GameObject exp in explosions)
        {
            Collider2D col = exp.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

}
