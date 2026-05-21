using UnityEngine;

/// <summary>
/// PlayerController.cs - Handles player movement and pod placement
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject podPrefab;
    [SerializeField] private int maxPods = 5;
    [SerializeField] private float podPlacementCooldown = 0.3f;

    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.zero;
    private int podsRemaining;
    private float podPlacementTimer = 0f;
    private GameManager gameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        podsRemaining = maxPods;
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            gameManager.UpdatePodsDisplay(podsRemaining);
        }
    }

    private void Update()
    {
        HandleInput();

        if (podPlacementTimer > 0)
        {
            podPlacementTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    /// <summary>
    /// Handle player input
    /// </summary>
    private void HandleInput()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        // Pod placement
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlacePod();
        }
    }

    /// <summary>
    /// Place a pod at player position
    /// </summary>
    private void PlacePod()
    {
        if (podsRemaining <= 0 || podPlacementTimer > 0)
        {
            return;
        }

        if (podPrefab == null)
        {
            Debug.LogError("Pod prefab not assigned!");
            return;
        }

        Instantiate(podPrefab, transform.position, Quaternion.identity);
        podsRemaining--;
        podPlacementTimer = podPlacementCooldown;

        if (gameManager != null)
        {
            gameManager.UpdatePodsDisplay(podsRemaining);
        }

        Debug.Log($"Pod placed! Remaining: {podsRemaining}");
    }
}
