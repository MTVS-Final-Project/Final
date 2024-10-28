using System.Collections;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //����Ʈ ������, ���߿��� ����Ʈ�Ἥ �ϰų� �׳� �����ͺ��̽����� �޾ƿ��½����� �ؾ��ҵ�

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
