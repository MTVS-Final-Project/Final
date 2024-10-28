using UnityEngine;
using UnityEngine.UI;

public class TokenScript : MonoBehaviour
{
    public Text text;

    public int tokenCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = tokenCount.ToString();
    }
}
