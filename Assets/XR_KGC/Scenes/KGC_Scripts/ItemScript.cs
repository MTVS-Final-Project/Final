using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //컨탠츠바에 들어갈 물체들 리스트로 받아와서 따로넣어주기

    public GameObject gagu;

    public List<int> size = new List<int>(); //가구구분 0 = 1칸짜리 장식, 1 = 침대, 2 = 캣타워 ,3 = 고양이 변기, 4 = 고양이 밥그릇
    public List<string> itemName = new List<string>(); //가구이름
    public List<Sprite> sprites = new List<Sprite>(); //가구 스프라이트

   // public SelectItem si;
    void Start()
    {
        
        for (int i = 0; i < itemName.Count; i++)
        {
            GameObject go = Instantiate(gagu, transform);

            go.gameObject.GetComponent<SelectItem>().gaguSize = size[i];
            go.transform.GetChild(0).GetComponent<Text>().text = itemName[i];
            go.transform.GetChild(1).GetComponent<Image>().sprite = sprites[i];
            go.gameObject.GetComponent<SelectItem>().gaguImage = sprites[i];
            


            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void OnButtonClick(int buttonIndex)
    {
        Debug.Log("Button " + buttonIndex + " clicked! Name: " + itemName[buttonIndex]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
