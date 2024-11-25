using UnityEngine;
using TMPro;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun
{
    public TMP_InputField inputChat;

    // ChatItem prefabs
    public GameObject chatItemFactory;

    // Content�� Transform
    public RectTransform trContent;

    // ChatView�� Transform
    public RectTransform trChatView;

    // ä���� �߰��Ǳ� ���� Content�� ����(h)���� ������ �ִ� ����
    float prevContentH;


    private TouchScreenKeyboard keyboard; // �ȵ���̵� Ű����
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
        // Ű���� ���¿� ���� �ڵ����� �����
        if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            HideKeyboard();
        }
    }

    

    void OnSummit(string s)
    {
        //������ s�� ���̰� 0�̸� �Լ��� ������
        if (s.Length == 0) return;
        
        photonView.RPC(nameof(AddChat), RpcTarget.All, s);  

        //������ inputChat�� Ȱ��ȭ����
        inputChat.ActivateInputField();
    }

    [PunRPC]
    void AddChat(string chat)
    {
        //���ο� ä���� �߰��Ǳ� ���� Content �� h ���� ����
        prevContentH = trContent.sizeDelta.y;

        //ChatItem �ϳ� ������(�θ� ChatView�� Content�� ����
        GameObject go = Instantiate(chatItemFactory, trContent);
        //ChatItem ������Ʈ�� ��������
        ChatItem chatItem = go.GetComponent<ChatItem>();
        // ������ ������Ʈ�� SetText �Լ� ����
        chatItem.SetText(chat);
        // inputChat�� �ִ� ������ �ʱ�ȭ
        inputChat.text = "";
    }

    void OnValueChanged(string s)
    {
        print("���� �� : " + s);
    }
    void OnEndEdit(string s)
    {
        print("�ۼ� �� : " + s);
    }

    public void AutoScrollBottom()
    {
        if (trContent.sizeDelta.y > trChatView.sizeDelta.y)
        {
            // ���� �ٴڿ� ��Ҵٸ�
            if (prevContentH - trChatView.sizeDelta.y <= trContent.anchoredPosition.y)
            {
                //content�� y���� �缳���Ѵ�.
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trChatView.sizeDelta.y);
            }
        }
    }

    // �ȵ���̵� Ű���� ǥ��
    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "Enter Message");
    }

    // �ȵ���̵� Ű���� �����
    public void HideKeyboard()
    {
        if (keyboard != null)
        {
            keyboard.active = false;
        }
    }
}
