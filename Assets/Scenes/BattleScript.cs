using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text elapsedText;

    [Header("Fish Descent")]
    public RectTransform fishContainer;
    public float startY      = 50f;
    public float maxDescentY = -300f;

    [Header("Settings")]
    public float pomoMinutes   = 25f;
    public float breakMinutes  = 5f;
    public int   totalPomos    = 4;
    public int   seaDollarsReward = 100;

    public void setPomos(int numPomos)
    {
        totalPomos = numPomos;
    }

    // Calculated from minutes fields
    private float pomoDuration  => pomoMinutes  * 60f;
    private float breakDuration => breakMinutes * 60f;

    public GameManager manager;

    private float timeRemaining;
    private float timeElapsed;
    private float totalSessionElapsed;
    private float totalSessionDuration;
    private bool  isRunning      = false;
    private bool  isOnBreak      = false;
    private int   pomosCompleted = 0;

    void OnEnable()
    {
        StartSession();
    }

    // ── Called once at the very start to reset everything ────
    public void StartSession()
    {
        pomosCompleted        = 0;
        totalSessionElapsed   = 0f;
        totalSessionDuration  = totalPomos * pomoDuration;
        isOnBreak             = false;

        // Reset fish to top
        if (fishContainer != null)
            fishContainer.anchoredPosition = new Vector2(fishContainer.anchoredPosition.x, startY);

        StartPomo();
    }

    // ── Starts a single pomo ─────────────────────────────────
    void StartPomo()
    {
        timeRemaining = pomoDuration;
        timeElapsed   = 0f;
        isRunning     = true;
        isOnBreak     = false;

        UpdateTimerDisplay();
    }

    // ── Starts a break ───────────────────────────────────────
    void StartBreak()
    {
        timeRemaining = breakDuration;
        timeElapsed   = 0f;
        isOnBreak     = true;
        isRunning     = true;
    }

    void Update()
    {
        if (!isRunning) return;

        float delta = Time.deltaTime;
        timeRemaining       -= delta;
        timeElapsed         += delta;
        totalSessionElapsed += delta;

        UpdateTimerDisplay();
        UpdateFishPosition();

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning     = false;

            if (!isOnBreak)
                OnPomoComplete();
            else
                OnBreakComplete();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = isOnBreak
            ? $"Break: {minutes:00}:{seconds:00}"
            : $"Pomo {pomosCompleted + 1}/{totalPomos}  {minutes:00}:{seconds:00}";

        if (elapsedText != null)
        {
            int em = Mathf.FloorToInt(timeElapsed / 60f);
            int es = Mathf.FloorToInt(timeElapsed % 60f);
            elapsedText.text = isOnBreak
                ? $"Break time! ({pomosCompleted}/{totalPomos} done)"
                : $"Survived: {em:00}:{es:00}";
        }
    }

    void UpdateFishPosition()
    {
        if (fishContainer == null) return;
        if (isOnBreak) return;                  // don't move on break

        float totalDistance = startY - maxDescentY;
        float speed = totalDistance / totalSessionDuration; // pixels per second

        float currentY = fishContainer.anchoredPosition.y;
        float newY = Mathf.Max(currentY - speed * Time.deltaTime, maxDescentY);

        fishContainer.anchoredPosition = new Vector2(fishContainer.anchoredPosition.x, newY);
    }
    void OnPomoComplete()
    {
        pomosCompleted++;
        GameManager.Instance.AddSeaDollars(seaDollarsReward);

        if (pomosCompleted >= totalPomos)
        {
            Debug.Log("All pomos complete!");
            GameManager.Instance.SwitchState(GameState.TaskList);
        }
        else
        {
            StartBreak();
        }
    }

    void OnBreakComplete()
    {
        //GameManager.Instance.SwitchState(GameState.TaskList);
        StartPomo();
    }

    public float BattleProgress => totalSessionElapsed / totalSessionDuration;
}