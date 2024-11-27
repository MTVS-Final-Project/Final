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
    public Text male;
    public Text age;
    public Text name;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catAI = GameObject.Find("Cat").GetComponent<CatAIFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        name.text = catAI.name;
        weight.text = catAI.weight.ToString();
        hunger.text = Mathf.Floor(catAI.hunger).ToString();
        friendly.text = catAI.friendly.ToString();
        mood.text = catAI.mood.ToString();
        sleep.text = Mathf.Floor(catAI.sleepy).ToString();
        age.text = catAI.age.ToString()+"개월";
        if (catAI.male)
        {
            male.text = "수";
        }
        else
        {
            male.text = "암";   

        }
    }
}
