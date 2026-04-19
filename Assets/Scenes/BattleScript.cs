using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text elapsedText;
    public Slider battleProgressBar;

    [Header("Fish Descent")]
    public RectTransform fishContainer;
    public float startY = 50f;
    public float maxDescentY = -300f;

    [Header("Settings")]
    public float pomoDuration = 25f * 60f;
    public int seaDollarsReward = 100;

    private float timeRemaining;
    private float timeElapsed;
    private bool isRunning = false;

    void OnEnable()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        timeRemaining = pomoDuration;
        timeElapsed = 0f;
        isRunning = true;

        // Reset fish position to top
        if (fishContainer != null)
            fishContainer.anchoredPosition = new Vector2(fishContainer.anchoredPosition.x, startY);

        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!isRunning) return;

        float delta = Time.deltaTime;
        timeRemaining -= delta;
        timeElapsed   += delta;

        UpdateTimerDisplay();
        UpdateProgressBar();
        UpdateFishPosition();

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            OnPomoComplete();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (elapsedText != null)
        {
            int em = Mathf.FloorToInt(timeElapsed / 60f);
            int es = Mathf.FloorToInt(timeElapsed % 60f);
            elapsedText.text = $"Survived: {em:00}:{es:00}";
        }
    }

    void UpdateProgressBar()
    {
        if (battleProgressBar != null)
            battleProgressBar.value = timeElapsed / pomoDuration;
    }

    void UpdateFishPosition()
    {
        if (fishContainer == null) return;

        float progress = timeElapsed / pomoDuration; // 0 to 1
        float newY = Mathf.Lerp(startY, maxDescentY, progress);
        fishContainer.anchoredPosition = new Vector2(fishContainer.anchoredPosition.x, newY);
    }

    public float BattleProgress => timeElapsed / pomoDuration;

    void OnPomoComplete()
    {
        var tasks = GameManager.Instance.tasks;
        Task activeTask = tasks.Find(t => !t.isComplete);
        if (activeTask != null)
            activeTask.pomosCompleted++;

        GameManager.Instance.AddSeaDollars(seaDollarsReward);
        GameManager.Instance.SwitchState(GameState.Shop);
    }
}