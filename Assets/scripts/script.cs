using UnityEngine;

public class script : MonoBehaviour
{

    
    public Transform circle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        circle.localScale += new Vector3(1, 1, 1);
    }
}
