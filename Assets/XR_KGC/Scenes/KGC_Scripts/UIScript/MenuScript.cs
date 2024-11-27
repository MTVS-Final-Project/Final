using System.Collections;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject ModifyCanvas;
    public GameObject modifyStartButton;
    public GameObject catStatusButton;
    public GameObject menuButton;
    public GameObject placeCanvas;
    public GameObject csCanvas;

    public ModifySetup ms;

    public bool open;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        csCanvas = GameObject.Find("CatStatusCanvas");
        modifyStartButton = GameObject.Find("ModifyStart");
        catStatusButton = transform.GetChild(1).gameObject;
        ModifyCanvas = GameObject.Find("ModifyCanvas");
        menuButton = GameObject.Find("MenuButton");
        placeCanvas = GameObject.Find("PlaceCanvas");
        ms = ModifyCanvas.GetComponent<ModifySetup>();
    }
    void Start()
    {
       
        csCanvas.SetActive(false);
        StartCoroutine(HideUIWhenStart());

    }

    public IEnumerator HideUIWhenStart()
    {
        yield return new WaitForSeconds(0.05f);
        catStatusButton.SetActive(false);
        modifyStartButton.SetActive(false);
        ModifyCanvas.SetActive(false);

    }
    public void OnOff()
    {
        if (!open) //꺼져있으면
        {
            open = !open;
            catStatusButton.SetActive(true);
            modifyStartButton.SetActive(true);
            ModifyCanvas.SetActive(true);
        }
        else //켜져있으면
        {
            open = !open;
            catStatusButton.SetActive(false);
            modifyStartButton.SetActive(false);
            ModifyCanvas.SetActive(false);
        }
    }

    public void CloseStatus()
    {
        csCanvas?.SetActive(false);
        menuButton?.SetActive(true);
    }
    public void ShowStatus()
    {
        csCanvas.SetActive(true);
        menuButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (placeCanvas.activeInHierarchy)
        {
            menuButton.SetActive(false);
        }
        if (!modifyStartButton.activeInHierarchy)
        {
            catStatusButton.SetActive(false);
           // menuButton.SetActive(false);
        }
        if (!menuButton.activeInHierarchy)
        {
            catStatusButton.SetActive(false);
            modifyStartButton.SetActive(false);
            open = false;
        }
        if (!catStatusButton.activeInHierarchy)
        {
            modifyStartButton.SetActive(false);
        }
    }
}
