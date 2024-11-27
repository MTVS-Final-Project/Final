using System.Collections;
using TMPro;
using UnityEngine;

public class ChatItem : MonoBehaviour
{
    TMP_Text chatText;
    private void Awake()
    {
        chatText = GetComponent<TMP_Text>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetText(string s)
    {
        //�ؽ�Ʈ ����
        chatText.text = s;

        // ������ ���� �ڷ�ƾ ����
        StartCoroutine(UpdateSize());
        
    }

    IEnumerator UpdateSize()
    {
        yield return null;
        // �ؽ�Ʈ ���뿡 ���缭 ũ�⸦ ����
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);


        yield return null;
        // ChatView ���� ������Ʈ ã��
        GameObject go = GameObject.Find("ChatView");
        // ã�� ������Ʈ���� ChatManagaer ������Ʈ�� ��������
        ChatManager cm = go.GetComponent<ChatManager>();
        // ������ ������Ʈ AutoScollBottom �Լ� ȣ��
        cm.AutoScrollBottom();
    }
}
