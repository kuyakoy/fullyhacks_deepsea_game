using UnityEngine;
using System.Collections.Generic;
using TMPro;

public enum GameState { TaskList, Battle, Shop }

// ============================================================
//  GAME MANAGER
// ============================================================
public class GameManager : MonoBehaviour
{
    public GameState StateOnStart = GameState.Battle;
    public static GameManager Instance;

    // ── Panels ───────────────────────────────────────────────
    [Header("Panels")]
    public GameObject taskListPanel;
    public GameObject battlePanel;
    public GameObject shopPanel;

    // ── Game Data ────────────────────────────────────────────
    [Header("Game Data")]
    public int seaDollars = 0;
    public GameState currentState;

    public BattleManager battleScript;

    // ── TMP References ───────────────────────────────────────
    private TMP_Text TimerText;
    private TMP_Text CurrentTaskText;
    public TMP_Text SeaDollarsText;

    // Internal Data
    private string currentTask = null;

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
        UpdateDisplay();
        SwitchState(StateOnStart);
    }

    // ============================================================
    //  SETUP
    // ============================================================
    void FindTMPs()
    {
        //  TimerText       = battlePanel.transform.Find("TimerText").GetComponent<TMP_Text>();
        //  CurrentTaskText = battlePanel.transform.Find("CurrentTaskText").GetComponent<TMP_Text>();
        //  SeaDollarsText  = battlePanel.transform.Find("SeaDollarsText").GetComponent<TMP_Text>();
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
            case GameState.TaskList: 
                taskListPanel.SetActive(true); 
                break;

            case GameState.Battle:   
                battlePanel.SetActive(true);
                break;

            case GameState.Shop:     
                shopPanel.SetActive(true); 
                break;
        }
    }

    // ============================================================
    //  DISPLAY
    // ============================================================
    public void UpdateDisplay()
    {
        if (CurrentTaskText != null)
            CurrentTaskText.text = "Current Task: " + currentTask;

        if (SeaDollarsText != null)
            SeaDollarsText.text = "Sea Dollars: " + seaDollars;
    }

    // ============================================================
    //  TASKS
    // ============================================================
    public void ActivateTask(string name, int pomos)
    {
        battleScript.setPomos(pomos);
        SwitchState(GameState.Battle);
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
