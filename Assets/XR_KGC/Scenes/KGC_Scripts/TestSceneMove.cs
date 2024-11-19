using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown((KeyCode.Q)))
        {
            SceneManager.LoadScene("Test_KGC");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene("Second_KGC");
        }
    }
}
