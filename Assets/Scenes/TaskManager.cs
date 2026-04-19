using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [Header("Task List")]
    public GameObject add_button;
    public GameObject t;
    public GameObject parent;

    [Header("Input Prompt")]
    public GameObject prompt_ui;
    public TMP_InputField task_name;
    public TMP_InputField pomo_task;
    public Button done_button;

    [Header("Navigation")]
    public Button startPomoButton;

    private Vector3 new_position;
    private Vector3 DEF_POS = new Vector3(150, 300, 0);
    private Vector3 INCR_VEC = new Vector3(0, 60, 0);
    private Transform but_transf;
    private RectTransform view;
    private int button_count = 0;

    void Start()
    {
        but_transf = add_button.GetComponent<Transform>();
        view = parent.GetComponent<RectTransform>();

        add_button.GetComponent<Button>().onClick.AddListener(AddTask);
        done_button.onClick.AddListener(CompletePrompt);

        if (startPomoButton != null)
            startPomoButton.onClick.AddListener(StartPomo);

        new_position = DEF_POS;
    }

    void AddTask()
    {
        PromptTask();

        if (button_count >= 9)
            add_button.SetActive(false);
    }

    void PromptTask()
    {
        prompt_ui.SetActive(true);
    }

    void CompletePrompt()
    {
        string name = task_name.text;
        int pomos = 1;

        if (int.TryParse(pomo_task.text, out int res))
            pomos = res;

        task_name.text = "";
        pomo_task.text = "";

        CompleteCreate(name, pomos);
        button_count++;
        prompt_ui.SetActive(false);
    }

    void CompleteCreate(string name, int pomos)
    {
        // Save to GameManager so BattleManager can access it
        Task newTask = new Task
        {
            taskName = name,
            pomosRequired = pomos,
            pomosCompleted = 0
        };
        GameManager.Instance.tasks.Add(newTask);

        // Spawn the UI row
        GameObject new_task = Instantiate(t);
        new_task.transform.SetParent(parent.transform);
        new_task.transform.localPosition = new_position;

        new_position -= INCR_VEC;
        but_transf.localPosition -= INCR_VEC;

        new_task.transform.Find("TaskName").gameObject.GetComponent<TMP_Text>().text = name;
        new_task.transform.Find("Pomos").gameObject.GetComponent<TMP_Text>().text = "Pomos to complete: " + pomos.ToString();
        new_task.name = "task" + button_count;

        // Capture index for delete
        int capturedIndex = button_count;
        new_task.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(
            delegate { DeleteTask(capturedIndex); }
        );
    }

    void DeleteTask(int ind)
    {
        // Remove from GameManager tasks list
        if (ind - 1 < GameManager.Instance.tasks.Count)
            GameManager.Instance.tasks.RemoveAt(ind - 1);

        Destroy(parent.transform.GetChild(ind - 1).gameObject);
        button_count--;
        new_position = DEF_POS;

        int i = 0;
        foreach (Transform child in parent.transform)
        {
            child.localPosition = new_position;
            child.gameObject.name = "task" + (i + 1).ToString();
            new_position -= INCR_VEC;
            i++;
        }

        new_position += INCR_VEC;
        but_transf.localPosition = DEF_POS - ((i - 1) * INCR_VEC);
        add_button.SetActive(true);
    }

    void StartPomo()
    {
        Task active = GameManager.Instance.tasks.Find(t => !t.isComplete);
        if (active == null) return;

        GameManager.Instance.SwitchState(GameState.Battle);
    }
}