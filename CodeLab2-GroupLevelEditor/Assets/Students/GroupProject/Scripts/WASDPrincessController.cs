using System;
using UnityEngine;

public class WASDPrincessController : MonoBehaviour
{
    public float baseSpeed = 5f; // Base speed of the princess
    public GridScript grid; // Reference to the GridScript to read start and goal positions

    private Vector2 currentGridPosition; // Current position of the princess on the grid
    private float currentSpeed; // Current speed of the princess based on material cost
    private Vector3 targetPosition; // Target position for lerping
    private float lerpDuration = 0.5f; // Initial duration for lerping
    private bool isLerping; // Flag to prevent input while lerping
    private Vector2 bufferedDirection; // Stores the input direction while lerping

    private float lerpStartTime; // Time when lerping started
    private float lerpThreshold = 0.01f; // Threshold to consider lerp complete

    // make the difference more noticeable
    public float speedMultiplier = 3f;  // Multiplier to exaggerate speed differences
    public float lerpDurationMultiplier = 2f;  // Multiplier to exaggerate lerp duration differences

    void Start()
    {
        // Initialize current position to the grid start position
        currentGridPosition = new Vector2(grid.start.x, grid.start.y);
        transform.position = GridToWorld(currentGridPosition); // Set the initial position
        currentSpeed = baseSpeed; // Set initial speed
        targetPosition = transform.position; // Set the initial target position
        isLerping = false; // Not lerping initially
    }

    void Update()
    {
        
        // Continuously check if the player has reached the goal position
        if (HasReachedGoal())
        {
            MarkPlayerAsFinished();
        }
        
        // If currently lerping, allow buffered input but do not move immediately
        if (isLerping)
        {
            float t = (Time.time - lerpStartTime) / lerpDuration;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            // Check if lerping is close to complete (within the threshold)
            if (Vector3.Distance(transform.position, targetPosition) <= lerpThreshold)
            {
                // Snap to the target to avoid float inaccuracies
                transform.position = targetPosition;
                isLerping = false; // Lerp is complete
                ProcessBufferedInput(); // Apply any buffered input
            }
        }
        else
        {
            // Capture the input when not lerping
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
    }

    private void Move(Vector2 direction)
    {
        // Calculate new position
        Vector2 newGridPosition = currentGridPosition + direction; // Move one grid cell at a time

        // Check if the new position is within bounds
        if (IsWithinBounds(newGridPosition))
        {
            // Read the material cost at the new position
            currentSpeed = GetSpeedBasedOnMaterial(newGridPosition);

            // Update lerp duration based on material cost
            lerpDuration = CalculateLerpDuration(newGridPosition);

            // Update the current position and target position
            currentGridPosition = newGridPosition; // Update the current position
            targetPosition = GridToWorld(currentGridPosition); // Set the target position for lerping
            lerpStartTime = Time.time; // Reset the lerp start time

            isLerping = true; // Set lerping flag
            bufferedDirection = Vector2.zero; // Clear any buffered input
        }
    }

    private void ProcessBufferedInput()
    {
        // If buffered input exists, apply it once lerp is complete
        if (bufferedDirection != Vector2.zero)
        {
            Move(bufferedDirection); // Move based on buffered direction
            bufferedDirection = Vector2.zero; // Clear the buffer
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
                           0); 
    }

    private float CalculateLerpDuration(Vector2 position)
    {
        // Get the material cost for the current position
        GameObject currentQuad = grid.GetGrid()[(int)position.x, (int)position.y];
        float cost = grid.GetMovementCost(currentQuad);

        // Exaggerate lerp duration based on cost (e.g., higher cost = much longer duration)
        return Mathf.Clamp(cost * lerpDurationMultiplier, 0.1f, 500f); 
    }

    private float GetSpeedBasedOnMaterial(Vector2 position)
    {
        // Get the material type at the new position
        GameObject currentQuad = grid.GetGrid()[(int)position.x, (int)position.y];
        float cost = grid.GetMovementCost(currentQuad);

        // Exaggerate speed difference based on cost (lower cost = much higher speed)
        return baseSpeed * (1f / cost) * speedMultiplier; // Apply speed multiplier for more noticeable difference
    }

    // Check if the player's current grid position matches the goal position
    private bool HasReachedGoal()
    {
        Vector2 goalPosition = new Vector2(grid.goal.x, grid.goal.y);
        return currentGridPosition == goalPosition;
    }
    
    // Mark the player into the queue once they reach the goal
    private void MarkPlayerAsFinished()
    {
        Debug.Log("Reached goal!");
        SingletonScript.instance.princessPlaces.Enqueue(gameObject.name);
        gameObject.SetActive(false); // Disable the player object after reaching the goal
    }
    
}
