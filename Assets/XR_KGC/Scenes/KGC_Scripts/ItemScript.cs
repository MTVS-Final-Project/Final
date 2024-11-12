using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //�������ٿ� �� ��ü�� ����Ʈ�� �޾ƿͼ� ���γ־��ֱ�

    public GameObject gagu;

    public List<int> size = new List<int>(); //����ũ��, 0�� 1ĭ 1�� 2ĭ¥����.
    public List<string> itemName = new List<string>(); //�����̸�
    public List<Sprite> sprites = new List<Sprite>(); //���� ��������Ʈ

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
