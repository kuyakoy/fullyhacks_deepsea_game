using UnityEngine;
using System.Collections.Generic;
using TMPro;

public enum GameState { TaskList, Battle, Shop }

// ============================================================
//  GAME MANAGER
// ============================================================
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ── Panels ───────────────────────────────────────────────
    [Header("Panels")]
    public GameObject taskListPanel;
    public GameObject battlePanel;
    public GameObject shopPanel;

    // ── Game Data ────────────────────────────────────────────
    [Header("Game Data")]
    public int seaDollars = 0;
    public List<Task> tasks = new List<Task>();
    public GameState currentState;

    // ── TMP References ───────────────────────────────────────
    private TMP_Text TimerText;
    private TMP_Text CurrentTaskText;
    private TMP_Text SeaDollarsText;

    // ============================================================
    //  UNITY LIFECYCLE
    // ============================================================
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else                  { Destroy(gameObject); }
    }

    void Start()
    {
        FindTMPs();
        TestTask();
        SwitchState(GameState.TaskList);
    }

    // ============================================================
    //  SETUP
    // ============================================================
    void FindTMPs()
    {
        TimerText       = battlePanel.transform.Find("TimerText").GetComponent<TMP_Text>();
        CurrentTaskText = battlePanel.transform.Find("CurrentTaskText").GetComponent<TMP_Text>();
        SeaDollarsText  = battlePanel.transform.Find("SeaDollarsText").GetComponent<TMP_Text>();
    }

    void TestTask()
    {
        // TODO: remove this block when UI is working
        AddTask("Test Task", 3);
        Debug.Log("Tasks in list: " + tasks.Count);
        Debug.Log("Task name: "     + tasks[0].taskName + " | Pomos required: " + tasks[0].pomosRequired);

        if (TimerText != null)
            TimerText.text = tasks[0].taskName;
    }

    // ============================================================
    //  STATE
    // ============================================================
    public void SwitchState(GameState newState)
    {
        currentState = newState;

        taskListPanel.SetActive(false);
        battlePanel  .SetActive(false);
        shopPanel    .SetActive(false);

        switch (newState)
        {
            case GameState.TaskList: taskListPanel.SetActive(true); break;
            case GameState.Battle:   battlePanel  .SetActive(true); break;
            case GameState.Shop:     shopPanel    .SetActive(true); break;
        }
    }

    // ============================================================
    //  DISPLAY
    // ============================================================
    void UpdateDisplay()
    {
        if (CurrentTaskText != null)
            CurrentTaskText.text = "Current Task: " + GetActiveTask()?.taskName;

        if (SeaDollarsText != null)
            SeaDollarsText.text = "Sea Dollars: " + seaDollars;
    }

    // ============================================================
    //  TASKS
    // ============================================================
    public void AddTask(string taskName, int pomosRequired)
    {
        tasks.Add(new Task {
            taskName       = taskName,
            pomosRequired  = pomosRequired,
            pomosCompleted = 0
        });
        Debug.Log("Task added: " + taskName + " | Pomos: " + pomosRequired);
    }

    public Task GetActiveTask()
    {
        return tasks.Find(t => !t.isComplete);
    }

    // ============================================================
    //  CURRENCY
    // ============================================================
    public void AddSeaDollars(int amount)
    {
        seaDollars += amount;
        UpdateDisplay();
    }
}

// ============================================================
//  TASK DATA CLASS
// ============================================================
[System.Serializable]
public class Task
{
    public string taskName;
    public int    pomosRequired;
    public int    pomosCompleted;
    public bool   isComplete => pomosCompleted >= pomosRequired;
}