using System.Collections;
using UnityEngine;

public class CatTemp : MonoBehaviour
{

    public bool hostile = false;
    public GameObject goodReaction;
    public GameObject badReaction;
    public QuestManager QM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CatAction()
    {
        if (hostile)
        {
            Debug.Log("고양이가 도망감.");

           // GameObject.Find("CatRespondText").GetComponent<CatRespond>().Avoid();
           badReaction.SetActive(true);

            StartCoroutine(ClearMotion());
        }

        else if (!hostile)
        {
            Debug.Log("고양이가 재미있어함.");

            QM.quest1 = true;
            //GameObject.Find("CatRespondText").GetComponent<CatRespond>().Greet();
            goodReaction.SetActive(true);
            StartCoroutine(ClearMotion());

        }
    }

    private IEnumerator ClearMotion()
    {
        yield return new WaitForSeconds(2f);
        if (badReaction.activeInHierarchy)
        {
            badReaction.SetActive(false);
        }
        if (goodReaction.activeInHierarchy)
        {
            goodReaction.SetActive(false);
        }
    }
}
