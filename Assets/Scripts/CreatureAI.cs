using UnityEngine;

/// <summary>
/// CreatureAI.cs - Manages creature behavior, movement, and species-specific traits
/// 12 Bolimcain species with unique movement patterns and difficulty
/// </summary>
public class CreatureAI : MonoBehaviour
{
    public enum SpeciesType
    {
        Zarnk,      // Fire - Fast
        Falk,       // Earth - Slow
        Valvule,    // Plant - Balanced
        Marsshowt,  // Dark - Tricky
        Opini,      // Rock - Fast
        Carchem,    // Fighter - Slow
        Bloblly,    // Water - Fast
        Closset,    // Ice - Slow
        Fatter,     // Ghost - Fast
        Vark,       // Normal - Balanced
        Flapoy,     // Fly - Fast
        Tirent      // Electric - Fast
    }

    [SerializeField] private SpeciesType species = SpeciesType.Valvule;
    [SerializeField] private float baseSpeed = 2.5f;
    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float directionChangeInterval = 2f;
    [SerializeField] private float obstacleDetectionDistance = 2f;

    private Rigidbody2D rb;
    private Vector2 currentDirection = Vector2.right;
    private Vector2 targetPosition;
    private float directionChangeTimer = 0f;
    private bool isContained = false;
    private Animator animator;

    // Species-specific parameters
    private float movementSpeed;
    private float changeInterval;
    private float evasionChance = 0f;

    public bool IsContained { get => isContained; set => isContained = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Initialize based on species
        InitializeSpecies();

        // Assign random start direction
        currentDirection = Random.insideUnitCircle.normalized;
        directionChangeTimer = changeInterval;
    }

    private void Update()
    {
        if (isContained || rb == null) return;

        directionChangeTimer -= Time.deltaTime;

        // Species-specific behavior
        switch (species)
        {
            case SpeciesType.Zarnk:
            case SpeciesType.Opini:
            case SpeciesType.Bloblly:
            case SpeciesType.Fatter:
            case SpeciesType.Flapoy:
            case SpeciesType.Tirent:
                BehaviorFast();
                break;

            case SpeciesType.Falk:
            case SpeciesType.Carchem:
            case SpeciesType.Closset:
                BehaviorSlow();
                break;

            case SpeciesType.Valvule:
            case SpeciesType.Vark:
                BehaviorBalanced();
                break;

            case SpeciesType.Marsshowt:
                BehaviorTricky();
                break;
        }

        UpdateAnimator();
    }

    /// <summary>
    /// Initialize species-specific traits
    /// </summary>
    private void InitializeSpecies()
    {
        switch (species)
        {
            case SpeciesType.Zarnk:
            case SpeciesType.Opini:
            case SpeciesType.Bloblly:
            case SpeciesType.Fatter:
            case SpeciesType.Flapoy:
            case SpeciesType.Tirent:
                movementSpeed = baseSpeed * 1.8f;
                changeInterval = directionChangeInterval * 0.7f;
                evasionChance = 0.2f;
                break;

            case SpeciesType.Falk:
            case SpeciesType.Carchem:
            case SpeciesType.Closset:
                movementSpeed = baseSpeed * 0.5f;
                changeInterval = directionChangeInterval * 1.5f;
                evasionChance = 0f;
                break;

            case SpeciesType.Valvule:
            case SpeciesType.Vark:
                movementSpeed = baseSpeed;
                changeInterval = directionChangeInterval;
                evasionChance = 0.1f;
                break;

            case SpeciesType.Marsshowt:
                movementSpeed = baseSpeed * 1.3f;
                changeInterval = directionChangeInterval * 0.5f;
                evasionChance = 0.4f;
                break;
        }
    }

    /// <summary>
    /// FAST Species: Zarnk, Opini, Bloblly, Fatter, Flapoy, Tirent
    /// Quick directional changes, active movement
    /// </summary>
    private void BehaviorFast()
    {
        if (directionChangeTimer <= 0)
        {
            PickNewDirection();
            directionChangeTimer = changeInterval + Random.Range(-0.3f, 0.3f);
        }

        if (IsPathBlocked())
        {
            PickNewDirection();
        }

        MoveCreature();
    }

    /// <summary>
    /// SLOW Species: Falk, Carchem, Closset
    /// Slow, predictable movement
    /// </summary>
    private void BehaviorSlow()
    {
        if (directionChangeTimer <= 0)
        {
            PickNewDirection();
            directionChangeTimer = changeInterval;
        }

        if (IsPathBlocked())
        {
            PickNewDirection();
        }

        MoveCreature();
    }

    /// <summary>
    /// BALANCED Species: Valvule, Vark
    /// Standard patrol behavior
    /// </summary>
    private void BehaviorBalanced()
    {
        if (directionChangeTimer <= 0)
        {
            PickNewDirection();
            directionChangeTimer = changeInterval;
        }

        if (IsPathBlocked())
        {
            PickNewDirection();
        }

        MoveCreature();
    }

    /// <summary>
    /// TRICKY Species: Marsshowt
    /// Very unpredictable movement, dodges randomly (40% chance)
    /// </summary>
    private void BehaviorTricky()
    {
        if (directionChangeTimer <= 0)
        {
            if (Random.value < evasionChance)
            {
                currentDirection = Random.insideUnitCircle.normalized;
            }
            else
            {
                PickNewDirection();
            }
            directionChangeTimer = changeInterval;
        }

        if (IsPathBlocked())
        {
            currentDirection = Random.insideUnitCircle.normalized;
        }

        MoveCreature();
    }

    /// <summary>
    /// Pick a new random direction
    /// </summary>
    private void PickNewDirection()
    {
        currentDirection = Random.insideUnitCircle.normalized;
    }

    /// <summary>
    /// Check if path ahead is blocked
    /// </summary>
    private bool IsPathBlocked()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, obstacleDetectionDistance);
        return hit.collider != null && hit.collider.CompareTag("Obstacle");
    }

    /// <summary>
    /// Move the creature
    /// </summary>
    private void MoveCreature()
    {
        if (rb != null)
        {
            rb.velocity = currentDirection * movementSpeed;
        }
    }

    /// <summary>
    /// Update animator with movement direction
    /// </summary>
    private void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("DirectionX", currentDirection.x);
        animator.SetFloat("DirectionY", currentDirection.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    // Getters
    public SpeciesType GetSpecies() => species;
    public float GetMovementSpeed() => movementSpeed;
    public string GetSpeciesName() => species.ToString();
}
