using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //컨탠츠바에 들어갈 물체들 리스트로 받아와서 따로넣어주기
    public GameObject TC; //장난감 들어있는 캔버스
    public GameObject Bar; //선택 버튼있는 바

    public GameObject item;

    public List<string> itemName = new List<string>();
    public List<Sprite> sprites = new List<Sprite>();

    void Start()
    {
        for (int i = 0; i < itemName.Count; i++)
        {
            GameObject go =  Instantiate(item,transform);

            go.transform.GetChild(0).GetComponent<Text>().text = itemName[i];
            go.transform.GetChild(1).GetComponent<Image>().sprite = sprites[i];


            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void OnButtonClick(int buttonIndex)
    {
        Debug.Log("Button " + buttonIndex + " clicked! Name: " + itemName[buttonIndex]);
        TC.transform.GetChild(buttonIndex).gameObject.SetActive(true);
        Bar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
