using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// CreatureAI.cs - Manages creature behavior, movement, and species-specific traits
/// Each Bolimcain species has unique movement patterns and difficulty
/// </summary>
public class CreatureAI : MonoBehaviour
{
    public enum SpeciesType
    {
        Artyn,      // Fast, aggressive
        Falk,       // Slow, predictable
        Valvule,    // Balanced
        Marsshowt   // Tricky, evasive
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
            case SpeciesType.Artyn:
                BehaviorArtyn();
                break;
            case SpeciesType.Falk:
                BehaviorFalk();
                break;
            case SpeciesType.Valvule:
                BehaviorValvule();
                break;
            case SpeciesType.Marsshowt:
                BehaviorMarsshowt();
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
            case SpeciesType.Artyn:
                movementSpeed = baseSpeed * 1.8f; // 80% faster
                changeInterval = directionChangeInterval * 0.7f; // Change direction more often
                evasionChance = 0.2f; // 20% chance to evade
                break;

            case SpeciesType.Falk:
                movementSpeed = baseSpeed * 0.5f; // 50% slower
                changeInterval = directionChangeInterval * 1.5f; // Change direction less often
                evasionChance = 0f; // No evasion
                break;

            case SpeciesType.Valvule:
                movementSpeed = baseSpeed; // Normal speed
                changeInterval = directionChangeInterval; // Normal change rate
                evasionChance = 0.1f; // 10% chance to evade
                break;

            case SpeciesType.Marsshowt:
                movementSpeed = baseSpeed * 1.3f; // 30% faster
                changeInterval = directionChangeInterval * 0.5f; // Very unpredictable
                evasionChance = 0.4f; // 40% chance to evade!
                break;
        }
    }

    /// <summary>
    /// Artyn: Fast, aggressive. Quick directional changes, active movement
    /// </summary>
    private void BehaviorArtyn()
    {
        // Change direction frequently and erratically
        if (directionChangeTimer <= 0)
        {
            PickNewDirection();
            directionChangeTimer = changeInterval + Random.Range(-0.3f, 0.3f); // Add variance
        }

        // Check for obstacles
        if (IsPathBlocked())
        {
            PickNewDirection();
        }

        MoveCreature();
    }

    /// <summary>
    /// Falk: Slow, predictable. Easy to predict movement
    /// </summary>
    private void BehaviorFalk()
    {
        // Change direction slowly and predictably
        if (directionChangeTimer <= 0)
        {
            PickNewDirection();
            directionChangeTimer = changeInterval;
        }

        // Check for obstacles
        if (IsPathBlocked())
        {
            PickNewDirection();
        }

        MoveCreature();
    }

    /// <summary>
    /// Valvule: Balanced. Standard patrol behavior
    /// </summary>
    private void BehaviorValvule()
    {
        // Standard behavior
        if (directionChangeTimer <= 0)
        {
            PickNewDirection();
            directionChangeTimer = changeInterval;
        }

        // Check for obstacles
        if (IsPathBlocked())
        {
            PickNewDirection();
        }

        MoveCreature();
    }

    /// <summary>
    /// Marsshowt: Tricky, evasive. Unpredictable movement, dodges randomly
    /// </summary>
    private void BehaviorMarsshowt()
    {
        // Very frequent direction changes
        if (directionChangeTimer <= 0)
        {
            // Sometimes dodge dramatically
            if (Random.value < evasionChance)
            {
                // Sudden direction change (dodge)
                currentDirection = Random.insideUnitCircle.normalized;
            }
            else
            {
                PickNewDirection();
            }
            directionChangeTimer = changeInterval;
        }

        // Check for obstacles
        if (IsPathBlocked())
        {
            // Dodge instead of just turning
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
