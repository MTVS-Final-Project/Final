using UnityEngine;
using TMPro;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun
{
    public TMP_InputField inputChat;

    // ChatItem prefabs
    public GameObject chatItemFactory;

    // Content의 Transform
    public RectTransform trContent;

    // ChatView의 Transform
    public RectTransform trChatView;

    // 채팅이 추가되기 전에 Content의 높이(h)값을 가지고 있는 변수
    float prevContentH;


    private TouchScreenKeyboard keyboard; // 안드로이드 키보드
    void Start()
    {
        //input Chat의 내용이 변경될때 호출되는 함수 등록
        inputChat.onValueChanged.AddListener(OnValueChanged);
        //input Chat 엔터를 쳤을 때 호출되는 함수 등록
        inputChat.onSubmit.AddListener(OnSummit);
        //input Chat 포커싱을 잃을 때 호출되는 함수 등록
        inputChat.onEndEdit.AddListener(OnEndEdit);
    }

    void Update()
    {
        // 키보드 상태에 따라 자동으로 숨기기
        if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            HideKeyboard();
        }
    }

    

    void OnSummit(string s)
    {
        //만약의 s의 길이가 0이면 함수를 나가자
        if (s.Length == 0) return;
        
        photonView.RPC(nameof(AddChat), RpcTarget.All, s);  

        //강제로 inputChat을 활성화하자
        inputChat.ActivateInputField();
    }

    [PunRPC]
    void AddChat(string chat)
    {
        //새로운 채팅이 추가되기 전의 Content 의 h 값을 저장
        prevContentH = trContent.sizeDelta.y;

        //ChatItem 하나 만들자(부모를 ChatView의 Content로 하자
        GameObject go = Instantiate(chatItemFactory, trContent);
        //ChatItem 컴포넌트를 가져오자
        ChatItem chatItem = go.GetComponent<ChatItem>();
        // 가져온 컴포넌트의 SetText 함수 실행
        chatItem.SetText(chat);
        // inputChat에 있는 내용을 초기화
        inputChat.text = "";
    }

    void OnValueChanged(string s)
    {
        print("변경 중 : " + s);
    }
    void OnEndEdit(string s)
    {
        print("작성 끝 : " + s);
    }

    public void AutoScrollBottom()
    {
        if (trContent.sizeDelta.y > trChatView.sizeDelta.y)
        {
            // 이전 바닥에 닿았다면
            if (prevContentH - trChatView.sizeDelta.y <= trContent.anchoredPosition.y)
            {
                //content의 y값을 재설정한다.
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trChatView.sizeDelta.y);
            }
        }
    }

    // 안드로이드 키보드 표시
    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "Enter Message");
    }

    // 안드로이드 키보드 숨기기
    public void HideKeyboard()
    {
        if (keyboard != null)
        {
            keyboard.active = false;
        }
    }
}
