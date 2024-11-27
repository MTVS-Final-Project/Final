using UnityEngine;
using UnityEngine.UI;

public class CatStatusScript : MonoBehaviour
{
    public CatAIFSM catAI;
    public Text weight;
    public Text hunger;
    public Text friendly;
    public Text mood;
    public Text sleep;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catAI = GameObject.Find("Cat").GetComponent<CatAIFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        weight.text = catAI.weight.ToString();
        hunger.text = Mathf.Floor(catAI.hunger).ToString();
        friendly.text = catAI.friendly.ToString();
        mood.text = catAI.mood.ToString();
        sleep.text = Mathf.Floor(catAI.sleepy).ToString();
    }
}
