using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public TMP_InputField inputChat;

    // ChatItem prefabs
    public GameObject chatItemFactory;

    // Content�� Transform
    public Transform trContent;

    public TMP_Text text;
    void Start()
    {
        //input Chat�� ������ ����ɶ� ȣ��Ǵ� �Լ� ���
        inputChat.onValueChanged.AddListener(OnValueChanged);
        //input Chat ���͸� ���� �� ȣ��Ǵ� �Լ� ���
        inputChat.onSubmit.AddListener(OnSummit);
        //input Chat ��Ŀ���� ���� �� ȣ��Ǵ� �Լ� ���
        inputChat.onEndEdit.AddListener(OnEndEdit);
    }

    void Update()
    {
        
    }

    void OnValueChanged(string s)
    {
        print("���� �� : " + s);
    }

    void OnSummit(string s)
    {
        //ChatItem �ϳ� ������(�θ� ChatView�� Content�� ����
        GameObject go = Instantiate(chatItemFactory, trContent);
        //ChatItem ������Ʈ�� ��������
        ChatItem chatItem = go.GetComponent<ChatItem>();
        // ������ ������Ʈ�� SetText �Լ� ����
        chatItem.SetText(s);
        // inputChat�� �ִ� ������ �ʱ�ȭ
        inputChat.text = "";
    }

    void OnEndEdit(string s)
    {
        print("�ۼ� �� : " + s);
    }
}
