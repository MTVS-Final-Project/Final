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
        if (Input.GetKeyDown((KeyCode.Alpha1)))
        {
            Debug.Log("1����");
            SceneManager.LoadScene("residential_KGC");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2����");

            SceneManager.LoadScene("Second_KGC");
        } 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3����");

            SceneManager.LoadScene("Test2_KGC");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("TestResidential_KGC");

        }
    }
    public void MoveToStreet()
    {
        SceneManager.LoadScene("Room_KGC");
    }
}
