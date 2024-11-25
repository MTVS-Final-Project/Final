using UnityEngine;

public class ToyCanvasScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject toySelectBar; 


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toySelectBar.activeInHierarchy)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

        }
    }
}
