using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;          // MM:SS countdown
    public TMP_Text elapsedText;        // shows time survived
    public Slider battleProgressBar;    // goes from 0 --> 1

    [Header("Settings")]
    public float pomoDuration = 25f * 60f;   // seconds
    public int seaDollarsReward = 100;

    private float timeRemaining;
    private float timeElapsed;
    private bool isRunning = false;

    void OnEnable()
    {
        // Called every time BattlePanel becomes active
        StartBattle();
    }

    public void StartBattle()
    {
        timeRemaining = pomoDuration;
        timeElapsed = 0f;
        isRunning = true;
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

        // Optional elapsed label — e.g. "Survived: 04:32"
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
            battleProgressBar.value = timeElapsed / pomoDuration; // 0 → 1
    }

    // How far through the pomo (0.0 to 1.0) — use this to scale enemy damage etc.
    public float BattleProgress => timeElapsed / pomoDuration;

    void OnPomoComplete()
    {
        // Update the active task's pomo count
        var tasks = GameManager.Instance.tasks;
        Task activeTask = tasks.Find(t => !t.isComplete);
        if (activeTask != null)
            activeTask.pomosCompleted++;

        GameManager.Instance.AddSeaDollars(seaDollarsReward);
        GameManager.Instance.SwitchState(GameState.Shop);
    }
}