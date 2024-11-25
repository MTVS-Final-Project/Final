using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public TMP_InputField inputChat;

    // ChatItem prefabs
    public GameObject chatItemFactory;

    // Content의 Transform
    public Transform trContent;

    public TMP_Text text;
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
        
    }

    void OnValueChanged(string s)
    {
        print("변경 중 : " + s);
    }

    void OnSummit(string s)
    {
        //ChatItem 하나 만들자(부모를 ChatView의 Content로 하자
        GameObject go = Instantiate(chatItemFactory, trContent);
        //ChatItem 컴포넌트를 가져오자
        ChatItem chatItem = go.GetComponent<ChatItem>();
        // 가져온 컴포넌트의 SetText 함수 실행
        chatItem.SetText(s);
        // inputChat에 있는 내용을 초기화
        inputChat.text = "";
    }

    void OnEndEdit(string s)
    {
        print("작성 끝 : " + s);
    }
}
