using UnityEngine;
using TMPro;

/// <summary>
/// UIManager.cs - Manages HUD and UI displays
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI creaturesText;
    [SerializeField] private TextMeshProUGUI podsText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private float elapsedTime = 0f;
    private bool gameActive = true;

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameActive)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    /// <summary>
    /// Update creatures display
    /// </summary>
    public void UpdateCreaturesDisplay(int remaining, int total)
    {
        if (creaturesText != null)
        {
            creaturesText.text = $"Creatures: {remaining}/{total}";
        }
    }

    /// <summary>
    /// Update pods display
    /// </summary>
    public void UpdatePodsDisplay(int remaining)
    {
        if (podsText != null)
        {
            podsText.text = $"Pods: {remaining}";
        }
    }

    /// <summary>
    /// Update objective text
    /// </summary>
    public void SetObjective(string text)
    {
        if (objectiveText != null)
        {
            objectiveText.text = text;
        }
    }

    /// <summary>
    /// Update timer display
    /// </summary>
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    /// <summary>
    /// Show game over screen
    /// </summary>
    public void ShowGameOver(bool won)
    {
        gameActive = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.text = won ? "🎉 VICTORY!\nAll creatures contained!" : "💥 GAME OVER!\nCreatures escaped!";
        }
    }
}
