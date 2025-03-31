using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;   // 静态实例，方便访问

    public Vector2 startPosition = new Vector2(8, 4); // 初始位置
    public LayerMask wallLayer;

    public GameObject bombPrefab;
    public int maxBombs = 1;    // 最大炸弹数量

    private Vector2 targetPosition;
    private SpriteRenderer spriteRenderer;   // 使用 SpriteRenderer 替代 MeshRenderer
    private Color originalColor;
    public float moveSpeed = 5f;  // 控制移动速度，单位：单位/秒
    private Rigidbody2D rb;
    private Vector3 move;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        targetPosition = startPosition;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, -1);

        // 获取 SpriteRenderer 组件并保存原始颜色
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        // Trigger DisableCollider(无敌) at the beginning
        DisableCollider();
    }

    void Update()
    {

        move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            move += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            move += Vector3.down;
        if (Input.GetKey(KeyCode.A))
            move += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            move += Vector3.right;

        // 放置炸弹逻辑，判断是否达到最大炸弹数量
        if (Input.GetKeyDown(KeyCode.Space) && Bomb.currentBombCount < maxBombs)
        {
            PlaceBomb();
            AudioManager.instance.PlaySFX("ignite-bomb");
        }
    }
    void FixedUpdate()
    {

        Vector3 pos = rb.transform.position;
        pos += move.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(pos);
    }

    private void PlaceBomb()
    {
        Vector2 bombGridPos = new Vector2(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y)
        );

        Collider2D hit = Physics2D.OverlapPoint(new Vector2(bombGridPos.x, bombGridPos.y), wallLayer);
        if (hit == null)
        {
            Instantiate(bombPrefab, new Vector3(bombGridPos.x, bombGridPos.y, -1), Quaternion.identity);
        }
    }

    // 激活速度提升效果
    public void ActivateSpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoost(duration));
    }

    private IEnumerator SpeedBoost(float duration)
    {
        moveSpeed = 10f;
        yield return new WaitForSeconds(duration);

        moveSpeed = 5f;
    }


    // 激活最大炸弹数量增加效果
    public void ActivateMaxBombsBoost(float duration)
    {
        StartCoroutine(MaxBombsBoost(duration));
    }

    private IEnumerator MaxBombsBoost(float duration)
    {
        maxBombs = 2;

        yield return new WaitForSeconds(duration);

        maxBombs = 1;
    }
    // 修改后的 DisableCollider 方法，使用 SpriteRenderer 来实现透明效果
    public void DisableCollider()
    {
        StartCoroutine(DisableColliderCoroutine());
    }

    private IEnumerator DisableColliderCoroutine()
    {
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Invincible");

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0.5f;
            spriteRenderer.color = c;
        }

        yield return new WaitForSeconds(2f);

        gameObject.layer = originalLayer;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
