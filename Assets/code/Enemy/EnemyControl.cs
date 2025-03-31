using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyControl : MonoBehaviour
{
    [Header("Movement")]
    public float cellSize = 1f;               
    public Vector2 startPosition = new Vector2(5, 5); 
    public LayerMask wallLayer;               // Detect Wall Layer
    public EnemyManager enemyManager;         // Reference EnemyManager

    public float baseSpeed = 2f;             
    public float speedMultiplier = 1f;        
    private float moveSpeed;                  // moveSpeed = baseSpeed * speedMultiplier

    private Vector2 targetPosition;
    private bool isMoving = false;

    [Header("Life and Invincibility")]
    public int maxHealth = 1;                 // maxHealth(differ for different enemy type
    private int currentHealth;
    private bool isInvulnerable = false;      // Temporary invincibility

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Health Bar UI")]
    public Sprite healthBarSprite;          
    public Transform healthBarContainer;    //Empty Object of Enemy to contain Health Bar
    private List<GameObject> healthBars = new List<GameObject>();  

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
        if (healthBarSprite != null && healthBarContainer != null)
        {
            for (int i = 0; i < maxHealth; i++)
            {
                GameObject bar = new GameObject("HealthBar_" + i);
                bar.transform.SetParent(healthBarContainer);
                // 添加 SpriteRenderer 并设置 sprite
                SpriteRenderer sr = bar.AddComponent<SpriteRenderer>();
                sr.sprite = healthBarSprite;
                sr.sortingOrder = 10;
                healthBars.Add(bar);
            }
            UpdateHealthBarPositions();
        }
    }

    void Update()
    {
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
    private void UpdateHealthBarPositions()
    {
        // 间隔可调（例如 0.3f）
        float spacing = 0.3f;
        int count = healthBars.Count;
        // 计算总宽度（以每个 bar 的中心间距计算）
        float totalWidth = (count - 1) * spacing;
        // 第一个 bar 的起始 x 坐标，使整个排列居中
        float startX = -totalWidth / 2f;
        for (int i = 0; i < count; i++)
        {
            // 更新每个血条的 localPosition
            healthBars[i].transform.localPosition = new Vector3(startX + i * spacing, 0, 0);
        }
    }

    void TakeDamage()
    {
        currentHealth--;

        // 如果还有血条，则移除最后一格
        if (healthBars.Count > 0)
        {
            GameObject bar = healthBars[healthBars.Count - 1];
            healthBars.RemoveAt(healthBars.Count - 1);
            Destroy(bar);
            // 更新剩余血条的位置，使其居中排列
            UpdateHealthBarPositions();
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
        EnemyControl[] enemies = FindObjectsByType<EnemyControl>(FindObjectsSortMode.None);
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
        if (EnemyManager.currentEnemyCount <= 0 && !EnemyManager.isSceneReloading)
        {
            if (VictoryManager.instance != null)
            {
                {
                    VictoryManager.instance.ShowVictory();  // 此处调用 VictoryManager 的方法显示胜利界面
                }
            }
        }
    }
}
