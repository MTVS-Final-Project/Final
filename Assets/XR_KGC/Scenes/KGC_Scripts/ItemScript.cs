using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //�������ٿ� �� ��ü�� ����Ʈ�� �޾ƿͼ� ���γ־��ֱ�

    public GameObject gagu;

    public List<string> itemName = new List<string>();
    public List<Sprite> sprites = new List<Sprite>();

    void Start()
    {
        for (int i = 0; i < itemName.Count; i++)
        {
            GameObject go = Instantiate(gagu, transform);

            go.transform.GetChild(0).GetComponent<Text>().text = itemName[i];
            go.transform.GetChild(1).GetComponent<Image>().sprite = sprites[i];


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
