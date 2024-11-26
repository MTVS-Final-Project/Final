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
    public CatAIFSM catAI;

    // public CatBehavior CB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catAI = GetComponent<CatAIFSM>();
        // CB = GetComponent<CatBehavior>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void CatAction()
    {
        if (catAI.friendly > 80)
        {
            catAI.interaction.SuperH();
        }
        else if (catAI.friendly > 60)
        {
            catAI.interaction.ShowFriendlyReaction();
        }
        else //if (catAI.friendly > 40)
        {
            catAI.interaction.Negative();
        }
    }


}
