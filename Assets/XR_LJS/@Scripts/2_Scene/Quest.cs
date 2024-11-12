using UnityEngine;

public class Quest : MonoBehaviour
{
    public GameObject questPop;
    public GameObject userQuestPop;
    public GameObject suju;
    public void QuestOpen()
    {
        questPop.SetActive(true);
    }

    public void QuestClose()
    {
        questPop.SetActive(false);
    }

    public void GetQuest()
    {
        userQuestPop.SetActive(true);
    }

    public void LetsQuest()
    {
        // 퀘스트를 수락하는 로직
        userQuestPop.SetActive(false);
    }

    public void NopeQuest()
    {
        // 퀘스트를 거절하는 로직
        userQuestPop.SetActive(false);
    }

    public void QuestSuju()
    {
        // 퀘스트 수주 로직
        suju.SetActive(true);
    }

    public void QuestSujuClose()
    {
        suju.SetActive(false);
    }


    public void GetQuest_Close()
    {
        userQuestPop.SetActive(false);
    }

}
