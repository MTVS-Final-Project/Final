using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SnackScript : MonoBehaviour
{
    public CatAIFSM CatAI;

    public Sprite full;
    public Sprite empty;
    public Button button;
    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CatAI = GameObject.Find("Cat").GetComponent<CatAIFSM>();
        button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FeedSnack()
    {
        //CatAI.mood += 10;
        //CatAI.friendly += 20;
        //CatAI.hunger += 20;
        //CatAI.interaction.ShowFriendlyReaction();
        StartCoroutine(Feed());
    }

    public IEnumerator Feed()
    {
        image.sprite = empty;
        CatAI.mood += 10;
        CatAI.friendly += 20;
        CatAI.hunger += 20;

        CatAI.interaction.ShowFriendlyReaction();
        button.interactable = false;



        yield return new WaitForSeconds(1);
        image.sprite = full;
        button.interactable |= true;
    }

}
