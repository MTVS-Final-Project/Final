using System.Collections;
using UnityEngine;

public class CatTemp : MonoBehaviour
{

    public bool hostile = false;
    public GameObject goodReaction;
    public GameObject badReaction;
    public GameObject negativeReaction;
    public GameObject veryGoodReaction;
    public QuestManager QM;

    public CatBehavior CB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CB = GetComponent<CatBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CB.catPersonality == CatBehavior.CatPersonality.Friendly)
        {
            hostile = false;
        }
        else if(CB.catPersonality == CatBehavior.CatPersonality.Picky)
        {
            hostile = true;
        }

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
