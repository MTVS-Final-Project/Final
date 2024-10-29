using UnityEngine;
using UnityEngine.UI;

public class CatRespond : MonoBehaviour
{
    public Text text;
    public QuestManager QM;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GameObject.Find("CatRespondText").GetComponent<Text>();
        QM = GameObject.Find("RoomQuestManager").GetComponent<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Avoid()
    {
        text.text = ("고양이가 싫어합니다.");
    }

    public void Greet()
    {
        text.text = ("고양이가 좋아합니다.");
        QM.quest1 = true;
    }
}
