using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField taskNameInput;
    public TMP_InputField pomoCountInput;

    [Header("Task List UI")]
    public Transform taskListContainer;  // The content object inside your ScrollView
    public GameObject taskItemPrefab;    // A prefab for each task row

    [Header("Buttons")]
    public Button addTaskButton;
    public Button startPomoButton;

    [Header("Display")]
    public TMP_Text seaDollarsText;

    private List<GameObject> taskUIItems = new List<GameObject>();

    void OnEnable()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        if (seaDollarsText != null)
            seaDollarsText.text = $"💰 {GameManager.Instance.seaDollars} Sea Dollars";
    }

    public void AddTask()
    {
        // Validate inputs
        string name = taskNameInput.text.Trim();
        if (string.IsNullOrEmpty(name)) return;

        if (!int.TryParse(pomoCountInput.text, out int pomos) || pomos <= 0) return;

        // Create the task data
        Task newTask = new Task
        {
            taskName = name,
            pomosRequired = pomos,
            pomosCompleted = 0
        };
        GameManager.Instance.tasks.Add(newTask);

        // Spawn a UI row
        GameObject item = Instantiate(taskItemPrefab, taskListContainer);
        TMP_Text[] labels = item.GetComponentsInChildren<TMP_Text>();
        labels[0].text = name;
        labels[1].text = $"🍅 x{pomos}";
        taskUIItems.Add(item);

        // Clear inputs
        taskNameInput.text = "";
        pomoCountInput.text = "";
    }

    public void StartPomo()
    {
        // Need at least one incomplete task to start
        Task active = GameManager.Instance.tasks.Find(t => !t.isComplete);
        if (active == null) return;

        GameManager.Instance.SwitchState(GameState.Battle);
    }
}