using UnityEngine;

public class CatTemp : MonoBehaviour
{

    public bool hostile = false;


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

            GameObject.Find("CatRespondText").GetComponent<CatRespond>().Avoid();

        }

        else if (!hostile)
        {
            Debug.Log("고양이가 재미있어함.");

            GameObject.Find("CatRespondText").GetComponent<CatRespond>().Greet();


        }
    }
}
