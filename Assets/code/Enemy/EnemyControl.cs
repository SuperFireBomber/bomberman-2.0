using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float cellSize = 1f;  // �ƶ�����
    public Vector2 startPosition = new Vector2(5, 5); // ����Ը����������õ��˵ĳ�ʼλ��
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

            // �������˳�򣨿�ѡ����ֱ�����ѡ����
            // �������з���ֱ���ҵ�һ����ײǽ�ķ���
            bool foundValidDirection = false;
            for (int i = 0; i < directions.Length; i++)
            {
                // ���ѡ��һ������
                int randIndex = Random.Range(0, directions.Length);
                Vector2 direction = directions[randIndex];

                Vector2 newPosition = targetPosition + direction;

                // �����λ���Ƿ�Ϊǽ��
                Collider2D hit = Physics2D.OverlapPoint(new Vector2(newPosition.x * cellSize, newPosition.y * cellSize), wallLayer);

                if (hit == null)
                {
                    targetPosition = newPosition;
                    isMoving = true;
                    foundValidDirection = true;
                    break;
                }
            }
            // ����ĸ����򶼲��У�����˱���ԭ�ز���
        }

        // ƽ���ƶ���Ŀ��λ��
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1),
            2f * Time.deltaTime);

        // ���ӽ�Ŀ��λ��ʱ��ֹͣ�ƶ�
        if (Vector3.Distance(transform.position, new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1)) < 0.01f)
        {
            isMoving = false;
        }
    }
}
