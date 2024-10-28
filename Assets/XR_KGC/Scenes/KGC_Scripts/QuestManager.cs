using System.Collections;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //퀘스트 보관소, 나중에는 리스트써서 하거나 그냥 데이터베이스에서 받아오는식으로 해야할듯

    public bool quest1;

    public bool expiredQuest1;

    public GameObject QUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (quest1)
        {
           if (!expiredQuest1)
            {
                expiredQuest1 = true;

                QuestComplete();
            }
            

        }
    }

    void QuestComplete()
    {
        StartCoroutine(QuestSuccess());
    }

    private IEnumerator QuestSuccess()
    {
        QUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        QUI.SetActive(false);

    }
}
