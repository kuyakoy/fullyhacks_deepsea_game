using UnityEngine;
using System.Collections.Generic;

public enum GameState { TaskList, Battle, Shop }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Panels")]
    public GameObject taskListPanel;
    public GameObject battlePanel;
    public GameObject shopPanel;

    [Header("Game Data")]
    public int seaDollars = 0;
    public List<Task> tasks = new List<Task>();
    public GameState currentState;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        SwitchState(GameState.TaskList);
    }

    public void SwitchState(GameState newState)
    {
        currentState = newState;

        taskListPanel.SetActive(false);
        battlePanel.SetActive(false);
        shopPanel.SetActive(false);

        switch (newState)
        {
            case GameState.TaskList: taskListPanel.SetActive(true); break;
            case GameState.Battle:   battlePanel.SetActive(true);   break;
            case GameState.Shop:     shopPanel.SetActive(true);     break;
        }
    }

    public void AddSeaDollars(int amount)
    {
        seaDollars += amount;
    }
}

[System.Serializable]
public class Task
{
    public string taskName;
    public int pomosRequired;
    public int pomosCompleted;
    public bool isComplete => pomosCompleted >= pomosRequired;
}