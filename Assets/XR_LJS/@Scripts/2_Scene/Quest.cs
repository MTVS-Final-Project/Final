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
        // ����Ʈ�� �����ϴ� ����
        userQuestPop.SetActive(false);
    }

    public void NopeQuest()
    {
        // ����Ʈ�� �����ϴ� ����
        userQuestPop.SetActive(false);
    }

    public void QuestSuju()
    {
        // ����Ʈ ���� ����
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
