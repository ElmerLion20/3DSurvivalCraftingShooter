using UnityEngine;

public class Chicken : MonoBehaviour
{
    private Animator animator;
    private float timer;
    private float threshold = 3f;
    [SerializeField] private LayerMask playerLayer;
    private float speed = 3;
    private float fleeDistance = 5f;
    private float fleeRotationSpeed = 5f;
    private float randomRotationSpeed = 1f;
    [SerializeField] Transform playerTransform;
    private Vector3 targetPosition;
    private bool isWalking = false;
    private float distanceToMove = 0.5f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;


        // Check if player is too close
        if (Vector3.Distance(transform.position, playerTransform.position) < fleeDistance)
        {
            FleeFromPlayer();
            Run();
        }
        else if (timer > threshold)
        {
            timer = 0;
            RandomAction();
            animator.SetBool("Run", false);
        }

        if (isWalking)
        {
            float step = 0.5f * Time.deltaTime; // Calculate the step based on speed

            // Move chicken towards the target position using Lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, step);

            // Check if the chicken has reached close to the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Ensure it reaches the exact target position
                isWalking = false; // Stop walking
            }
        }
    }

    void RandomAction()
    {
        int random = Random.Range(1, 4);

        switch (random)
        {
            case 1:
                WalkForward();
                break;
            case 2:
                TurnHead();
                break;
            case 3:
                Eat();
                break;
        }

    }

    void WalkForward()
    {
        animator.SetBool("Turn Head", false);
        animator.SetBool("Eat", false);
        animator.SetBool("Walk", true);

        targetPosition = transform.position + transform.forward * distanceToMove; // Calculate the target position

        isWalking = true;
    }

    void TurnHead()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Turn Head", true);
        animator.SetBool("Eat", false);
        RotateRandomly();
    }

    void Eat()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Turn Head", false);
        animator.SetBool("Eat", true);
    }

    void Run()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Turn Head", false);
        animator.SetBool("Eat", false);
        animator.SetBool("Run", true);
    }

    void FleeFromPlayer()
    {
        // Calculate direction from player to chicken
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Move the chicken away from the player
        transform.Translate(-directionToPlayer * speed * Time.deltaTime, Space.World);

        // Face away from the player (optional)
        Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, fleeRotationSpeed * Time.deltaTime);
    }


    void RotateRandomly()
    {
        // Generate a random rotation around the Y-axis
        float randomRotation = Random.Range(0f, 360f);

        // Apply the random rotation
        transform.Rotate(Vector3.up, randomRotation * randomRotationSpeed * Time.deltaTime);
    }
}