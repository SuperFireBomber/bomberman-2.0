using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float cellSize = 1f;  // 移动距离
    public Vector2 startPosition = new Vector2(8, 4); // 初始位置
    public LayerMask wallLayer;  

    private Vector2 targetPosition;  
    private bool isMoving = false;  

    void Start()
    {

        targetPosition = startPosition;
        transform.position = new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1);
    }

    // Add this at the top of the class (along with other public variables)
    public GameObject bombPrefab;
    void Update()
    {
        if (!isMoving) 
        {
            if (Input.GetKeyDown(KeyCode.W)) Move(Vector2.up);
            if (Input.GetKeyDown(KeyCode.S)) Move(Vector2.down);
            if (Input.GetKeyDown(KeyCode.A)) Move(Vector2.left);
            if (Input.GetKeyDown(KeyCode.D)) Move(Vector2.right);
        }
        // Bomb placement logic: place bomb when space key is pressed.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Compute bomb grid position (divide by cellSize, round to nearest integer)
            Vector2 bombGridPos = new Vector2(
                Mathf.Round(transform.position.x / cellSize),
                Mathf.Round(transform.position.y / cellSize)
            );

            // Check if the grid cell already has a wall (using the same wallLayer)
            Collider2D hit = Physics2D.OverlapPoint(new Vector2(bombGridPos.x * cellSize, bombGridPos.y * cellSize), wallLayer);
            if (hit == null)
            {
                // Instantiate bomb at the computed grid position (z值设为-1)
                Instantiate(bombPrefab, new Vector3(bombGridPos.x * cellSize, bombGridPos.y * cellSize, -1), Quaternion.identity);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1),
            5f * Time.deltaTime);


        if (Vector3.Distance(transform.position, new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1)) < 0.01f)
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
