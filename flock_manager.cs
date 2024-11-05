using UnityEngine;
using System.Collections.Generic;

public class FlockManager : MonoBehaviour
{
    public GameObject birdPrefab; // Assign your bird prefab in the inspector
    public int flockSize = 20;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float neighborDistance = 2f;

    private List<GameObject> birds = new List<GameObject>();
    private Vector3 targetPosition; // Target position for the birds
    private float movementDirection = 1f; // 1 for moving to the right, -1 for moving to the left

    void Start()
    {
        // Set initial target position
        targetPosition = new Vector3(500, 100, 0);

        for (int i = 0; i < flockSize; i++)
        {
            GameObject bird = Instantiate(birdPrefab, new Vector3(Random.Range(-500f, 500f), 100, Random.Range(-500f, 500f)), Quaternion.identity);
            birds.Add(bird);
            Debug.Log("Bird instantiated at position: " + bird.transform.position); // Log the position
        }
    }

    void Update()
    {
        // Update target position based on movement direction
        if (Vector3.Distance(birds[0].transform.position, targetPosition) < 1f)
        {
            // Switch target position
            movementDirection *= -1f; // Reverse direction
            targetPosition = new Vector3(movementDirection * 500, 100, 0); // New target position
        }

        foreach (GameObject bird in birds)
        {
            MoveBird(bird);
        }
    }

    void MoveBird(GameObject bird)
    {
        Vector3 velocity = Vector3.zero;
        int groupSize = 0;

        // Adjust the amplitude of the vertical oscillation
        float heightOffset = Mathf.Sin(Time.time + bird.GetInstanceID()) * 0.2f;

        // Get the current position of the bird
        Vector3 currentPosition = bird.transform.position;

        // Calculate the direction toward the target position
        Vector3 directionToTarget = (targetPosition - currentPosition).normalized;

        // Check neighboring birds for flocking behavior
        foreach (GameObject otherBird in birds)
        {
            if (otherBird != bird)
            {
                float distance = Vector3.Distance(currentPosition, otherBird.transform.position);

                if (distance < neighborDistance)
                {
                    // Align with neighbors
                    velocity += otherBird.transform.forward;
                    groupSize++;

                    // Separation behavior to avoid crowding
                    if (distance < 1f)
                    {
                        velocity += (currentPosition - otherBird.transform.position);
                    }
                }
            }
        }

        if (groupSize > 0)
        {
            velocity /= groupSize; // Average velocity
            velocity.Normalize();
            bird.transform.forward = Vector3.Slerp(bird.transform.forward, velocity, rotationSpeed * Time.deltaTime);
        }

        // Update the bird's position toward the target with flocking behavior
        Vector3 newPosition = currentPosition + directionToTarget * speed * Time.deltaTime;
        newPosition.y += heightOffset; // Apply adjusted height oscillation
        bird.transform.position = newPosition;

        // Change bird's rotation to look down and then up
        float lookAngle = Mathf.Sin(Time.time + bird.GetInstanceID()) * 15;
        bird.transform.rotation = Quaternion.Euler(lookAngle, bird.transform.eulerAngles.y, 0);
    }
}
