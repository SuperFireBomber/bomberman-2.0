using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float cellSize = 1f;  // �ƶ�����
    public Vector2 startPosition = new Vector2(5, 5); // ��ʼλ��
    public LayerMask wallLayer;  // ���ڼ��ǽ�ڵ�ͼ��

    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = startPosition;
        transform.position = new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1);
    }

    void Update()
    {
        if (!isMoving)
        {
            // �����ĸ����������ϡ��¡�����
            Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            for (int i = 0; i < directions.Length; i++)
            {
                int randIndex = Random.Range(0, directions.Length);
                Vector2 direction = directions[randIndex];
                Vector2 newPosition = targetPosition + direction;
                Collider2D hit = Physics2D.OverlapPoint(new Vector2(newPosition.x * cellSize, newPosition.y * cellSize), wallLayer);

                if (hit == null)
                {
                    targetPosition = newPosition;
                    isMoving = true;
                    break;
                }
            }
        }

        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1),
            2f * Time.deltaTime);

        if (Vector3.Distance(transform.position, new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1)) < 0.01f)
        {
            isMoving = false;
        }
    }

    // ������ײ��ⷽ��
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ��ȡPlayerController��������ý�����ײ��ķ���
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableCollider();
            }
        }
    }
}
