using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// GameManager.cs - Manages level progression, objectives, and win/lose conditions
/// Coordinates between Player, Creatures, and Pod system
/// </summary>
public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelObjective
    {
        public string objectiveName;
        public int creaturesToContain;
        public float timeLimit = 300f; // 5 minutes default
        public bool isTimeLimit = false;
        public bool isCreatureLimit = true;
    }

    [SerializeField] private LevelObjective levelObjective;
    [SerializeField] private int creaturesContained = 0;
    [SerializeField] private float timeRemaining;
    [SerializeField] private bool levelActive = true;
    [SerializeField] private bool levelWon = false;

    // References
    [SerializeField] private BubblePodSystem podSystem;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<CreatureAI> creaturesInLevel = new List<CreatureAI>();

    // Events
    public UnityEvent<int> OnCreaturesContained = new UnityEvent<int>();
    public UnityEvent<float> OnTimeUpdated = new UnityEvent<float>();
    public UnityEvent OnLevelWon = new UnityEvent();
    public UnityEvent OnLevelLost = new UnityEvent();
    public UnityEvent<string> OnObjectiveUpdated = new UnityEvent<string>();

    private static GameManager instance;

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        // Initialize
        timeRemaining = levelObjective.timeLimit;
        creaturesContained = 0;

        // Auto-find references if not assigned
        if (podSystem == null)
            podSystem = FindObjectOfType<BubblePodSystem>();
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        // Find all creatures in level
        FindAllCreatures();

        // Subscribe to pod system events
        if (podSystem != null)
        {
            podSystem.OnPodPlaced += OnPodPlaced;
        }

        UpdateObjectiveUI();
        Debug.Log($"[GameManager] Level started: {levelObjective.objectiveName}");
    }

    private void Update()
    {
        if (!levelActive) return;

        // Update timer if enabled
        if (levelObjective.isTimeLimit)
        {
            timeRemaining -= Time.deltaTime;
            OnTimeUpdated?.Invoke(timeRemaining);

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                LoseLevel("Time's up!");
            }
        }

        // Check win condition
        if (levelObjective.isCreatureLimit && creaturesContained >= levelObjective.creaturesToContain)
        {
            WinLevel();
        }
    }

    /// <summary>
    /// Called when a pod is placed - check if it contains a creature
    /// </summary>
    private void OnPodPlaced(Vector3 podPosition)
    {
        // Check all creatures to see if any are within the pod area
        foreach (CreatureAI creature in creaturesInLevel)
        {
            if (creature == null || creature.IsContained) continue;

            // Simple distance check (adjust radius as needed)
            float distanceToPod = Vector3.Distance(creature.transform.position, podPosition);
            if (distanceToPod < 1.5f) // Pod radius
            {
                ContainCreature(creature);
            }
        }
    }

    /// <summary>
    /// Mark a creature as contained
    /// </summary>
    private void ContainCreature(CreatureAI creature)
    {
        if (creature.IsContained) return;

        creature.IsContained = true;
        creature.gameObject.SetActive(false); // Remove from level
        creaturesContained++;

        OnCreaturesContained?.Invoke(creaturesContained);
        UpdateObjectiveUI();

        Debug.Log($"[GameManager] Creature contained! ({creaturesContained}/{levelObjective.creaturesToContain})");

        // Check win condition
        if (creaturesContained >= levelObjective.creaturesToContain)
        {
            WinLevel();
        }
    }

    /// <summary>
    /// Find all creatures currently in the level
    /// </summary>
    private void FindAllCreatures()
    {
        CreatureAI[] allCreatures = FindObjectsOfType<CreatureAI>();
        creaturesInLevel.AddRange(allCreatures);
        Debug.Log($"[GameManager] Found {creaturesInLevel.Count} creatures in level");
    }

    /// <summary>
    /// Spawn a creature at a specific location
    /// </summary>
    public void SpawnCreature(GameObject creaturePrefab, Vector3 spawnPosition)
    {
        GameObject newCreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        CreatureAI creatureAI = newCreature.GetComponent<CreatureAI>();
        if (creatureAI != null)
        {
            creaturesInLevel.Add(creatureAI);
        }
    }

    /// <summary>
    /// Win the level
    /// </summary>
    private void WinLevel()
    {
        if (levelWon) return; // Prevent multiple wins

        levelActive = false;
        levelWon = true;
        Time.timeScale = 0.5f; // Slow motion effect

        Debug.Log("[GameManager] LEVEL WON!");
        OnLevelWon?.Invoke();

        // You can add: load next level, show victory screen, etc.
        Invoke("CompleteLevel", 2f);
    }

    /// <summary>
    /// Lose the level
    /// </summary>
    public void LoseLevel(string reason)
    {
        if (!levelActive) return;

        levelActive = false;
        Time.timeScale = 0.5f; // Slow motion effect

        Debug.Log($"[GameManager] LEVEL LOST: {reason}");
        OnLevelLost?.Invoke();

        // You can add: restart level, show game over screen, etc.
        Invoke("RestartLevel", 2f);
    }

    /// <summary>
    /// Update objective UI text
    /// </summary>
    private void UpdateObjectiveUI()
    {
        string objectiveText = "";
        
        if (levelObjective.isCreatureLimit)
        {
            objectiveText += $"Creatures Contained: {creaturesContained}/{levelObjective.creaturesToContain}";
        }

        if (levelObjective.isTimeLimit)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            objectiveText += $" | Time: {minutes:00}:{seconds:00}";
        }

        OnObjectiveUpdated?.Invoke(objectiveText);
    }

    /// <summary>
    /// Complete level and move to next
    /// </summary>
    private void CompleteLevel()
    {
        Time.timeScale = 1f;
        Debug.Log("[GameManager] Loading next level...");
        // TODO: Load next scene/level
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Restart current level
    /// </summary>
    private void RestartLevel()
    {
        Time.timeScale = 1f;
        Debug.Log("[GameManager] Restarting level...");
        // TODO: Reload scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Getters
    public int GetCreaturesContained() => creaturesContained;
    public int GetCreaturesToContain() => levelObjective.creaturesToContain;
    public float GetTimeRemaining() => timeRemaining;
    public bool IsLevelActive() => levelActive;
    public bool IsLevelWon() => levelWon;
}
