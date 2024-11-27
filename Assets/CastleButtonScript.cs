using UnityEngine;

public class CastleButtonScript : MonoBehaviour
{
    public GameObject feed;
    public GameObject rub;
    public GameObject hug;
    public GameObject kidnap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenAction()
    {
        feed.SetActive(true);
        rub.SetActive(true);
        hug.SetActive(true);
        kidnap.SetActive(true);
    }
}
