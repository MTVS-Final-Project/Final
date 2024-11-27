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
        //텍스트 갱신
        chatText.text = s;

        // 사이즈 조절 코루틴 실행
        StartCoroutine(UpdateSize());
        
    }

    IEnumerator UpdateSize()
    {
        yield return null;
        // 텍스트 내용에 맞춰서 크기를 조절
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);


        yield return null;
        // ChatView 게임 오브젝트 찾자
        GameObject go = GameObject.Find("ChatView");
        // 찾은 오브젝트에서 ChatManagaer 컴포넌트를 가져오자
        ChatManager cm = go.GetComponent<ChatManager>();
        // 가져온 컴포넌트 AutoScollBottom 함수 호출
        cm.AutoScrollBottom();
    }
}
