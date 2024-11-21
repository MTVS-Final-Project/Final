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
        text.text = ("����̰� �Ⱦ��մϴ�.");
    }

    public void Greet()
    {
        text.text = ("����̰� �����մϴ�.");
        QM.quest1 = true;
    }
}
