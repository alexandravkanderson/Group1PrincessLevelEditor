using UnityEngine;

public class WASDPrincessController : MonoBehaviour
{
    public float speed = 5f; // Speed of the princess
    public GridScript grid; // Reference to the GridScript to read start and goal positions

    private Vector2 currentGridPosition; // Current position of the princess on the grid

    void Start()
    {
        // Initialize current position to the grid start position
        currentGridPosition = new Vector2(grid.start.x, grid.start.y);
        transform.position = GridToWorld(currentGridPosition); // Set the initial position
    }

    void Update()
    {
        // Capture the input
        Vector2 inputDirection = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W)) inputDirection += Vector2.down;  // Swap up with down
        if (Input.GetKeyDown(KeyCode.S)) inputDirection += Vector2.up;    // Swap down with up
        if (Input.GetKeyDown(KeyCode.A)) inputDirection += Vector2.left;
        if (Input.GetKeyDown(KeyCode.D)) inputDirection += Vector2.right;

        // Move if there's a direction input
        if (inputDirection != Vector2.zero)
        {
            Move(inputDirection);
        }
    }

    private void Move(Vector2 direction)
    {
        // Calculate new position
        Vector2 newGridPosition = currentGridPosition + direction; // Move one grid cell at a time

        // Check if the new position is within bounds
        if (IsWithinBounds(newGridPosition))
        {
            currentGridPosition = newGridPosition; // Update the current position
            transform.position = GridToWorld(currentGridPosition); // Convert grid position to world position
        }
    }

    private bool IsWithinBounds(Vector2 position)
    {
        // Check bounds based on grid dimensions
        int gridWidth = grid.gridWidth;
        int gridHeight = grid.gridHeight;

        // Ensure indices are within bounds
        return position.x >= 0 && position.x < gridWidth && position.y >= 0 && position.y < gridHeight;
    }

    private Vector3 GridToWorld(Vector2 gridPosition)
    {
        // Convert grid coordinates to world position
        float offsetX = (grid.gridWidth * -grid.spacing) / 2f;
        float offsetY = (grid.gridHeight * grid.spacing) / 2f;

        return new Vector3(offsetX + gridPosition.x * grid.spacing, 
                           offsetY - gridPosition.y * grid.spacing, 
                           0); // Assuming 2D game, z = 0
    }
}
