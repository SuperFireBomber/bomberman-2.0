using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float cellSize = 0.25f;  // 移动距离
    public Vector2 startPosition = new Vector2(8, 4); // 初始位置
    public LayerMask wallLayer;  

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
            if (Input.GetKeyDown(KeyCode.W)) Move(Vector2.up);
            if (Input.GetKeyDown(KeyCode.S)) Move(Vector2.down);
            if (Input.GetKeyDown(KeyCode.A)) Move(Vector2.left);
            if (Input.GetKeyDown(KeyCode.D)) Move(Vector2.right);
        }

 
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1),
            2f * Time.deltaTime);


        if (Vector3.Distance(transform.position, new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1)) < 0.25f)
        {
            isMoving = false;
        }
    }

    void Move(Vector2 direction)
    {
        Vector2 newPosition = targetPosition + direction;


        Collider2D hit = Physics2D.OverlapPoint(new Vector2(newPosition.x * cellSize, newPosition.y * cellSize), wallLayer);

        if (hit == null)
        {
            targetPosition = newPosition;
            isMoving = true; 
        }
    }
}
