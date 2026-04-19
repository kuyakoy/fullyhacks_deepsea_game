using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public Button add_button;
    public GameObject t;
    public GameObject parent;

    private Vector3 new_position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button add_bt = add_button.GetComponent<Button>();
        GameObject task_template = t.GetComponent<GameObject>();
        add_button.onClick.AddListener(AddTask);
        new_position = new Vector3(100,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void AddTask()
    {
        GameObject new_task = Instantiate(t);
        new_task.transform.SetParent(parent.transform);
        new_task.transform.localPosition = new_position;
        new_position -= new Vector3(0, 50, 0);

    }
    
}
