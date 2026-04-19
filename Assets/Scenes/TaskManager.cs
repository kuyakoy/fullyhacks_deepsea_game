using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public GameObject add_button;
    public GameObject t;
    public GameObject parent;

    public GameManager manager;

    private Vector3 new_position;
    private Vector3 DEF_POS = new Vector3(120, 0, 0);
    private Vector3 INCR_VEC = new Vector3(0, 60, 0);
    private Transform but_transf;
    private RectTransform view;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        but_transf = add_button.GetComponent<Transform>();
        Button add_bt = add_button.GetComponent<Button>();
        GameObject task_template = t.GetComponent<GameObject>();
        view = parent.GetComponent<RectTransform>();
        
        add_bt.onClick.AddListener(AddTask);
        done_button.onClick.AddListener(CompletePrompt);
        new_position = DEF_POS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private int button_count = 0;
    void AddTask()
    {
        PromptTask();
        button_count++;

        if (button_count >= 9) {
            add_button.SetActive(false);
        }
    }

    void CompleteCreate(string name, int pomos)
    {
        GameObject new_task = Instantiate(t);
        new_task.transform.SetParent(parent.transform);
        new_task.transform.localPosition = new_position;

        new_position -= INCR_VEC;
        //but_transf.localPosition -= INCR_VEC;
        new_task.transform.Find("TaskName").gameObject.GetComponent<TMP_Text>().text = name;
        new_task.transform.Find("Pomos").gameObject.GetComponent<TMP_Text>().text = "Pomo's to complete: " + pomos.ToString();
        new_task.name = "task" + button_count;

        new_task.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(delegate {DeleteTask(new_task.name[4] - '0');});
        new_task.transform.Find("Start").gameObject.GetComponent<Button>().onClick.AddListener(delegate {Activate(new_task.name[4] - '0');});
    }

    [Header("Input Items")]
    public GameObject prompt_ui;
    public TMP_InputField task_name;
    public TMP_InputField pomo_task;
    public Button done_button;
    
    void PromptTask()
    {
        prompt_ui.SetActive(true);
    }

    void CompletePrompt()
    {
        string name = task_name.text;
        int pomos = 1;
        if (int.TryParse(pomo_task.text, out int res))
        {
            pomos = res;
        } else
        {
            pomos = 1;
        }

        task_name.text = "";
        pomo_task.text = "";
        CompleteCreate(name, pomos);
        prompt_ui.SetActive(false);
    }

    void DeleteTask(int ind)
    {
        Destroy(parent.transform.GetChild(ind - 1).gameObject);
        button_count--;
        new_position = DEF_POS;
        int i = 0;
        bool cont = false;
        foreach (Transform child in parent.transform)
        {
            if (!cont && i == ind - 1)
            {
                cont = true;
                continue;
            }
            child.localPosition = new_position;
            child.gameObject.name = "task" + (i + 1).ToString();
            new_position -= INCR_VEC;
            Debug.Log(i);
            i++;
        }
        if (button_count < 9)
        {
            add_button.SetActive(true);
        }
        //but_transf.localPosition = DEF_POS - (i * INCR_VEC);
    }

    void Activate(int ind)
    {
        GameObject task = parent.transform.GetChild(ind - 1).gameObject;
        string name_task = task.transform.Find("TaskName").gameObject.GetComponent<TMP_Text>().text;
        int pomos = task.transform.Find("Pomos").gameObject.GetComponent<TMP_Text>().text[20] - '0';
        manager.ActivateTask(name_task, pomos);
    }
    
}
