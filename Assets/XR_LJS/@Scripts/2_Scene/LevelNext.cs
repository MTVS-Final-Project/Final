using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNext : MonoBehaviourPunCallbacks
{
    private int selectedSprite = 0;  // �⺻���� ���õ� ��������Ʈ �ε���
    public Button saveButton;  // ���̺� ��ư
    public Button loadButton;  // �ε� ��ư

    // Start �޼���: ��ư Ŭ�� �̺�Ʈ ����
    void Start()
    {

        // �ε� ��ư Ŭ�� �� Load �޼��� ȣ��
        loadButton.onClick.AddListener(LoadAndApplySprite);

        // ����� �α�: ���� ���� ��
        Debug.Log("LevelNext ��ũ��Ʈ�� ���۵Ǿ����ϴ�. �ʱ� ���õ� ��������Ʈ �ε���: " + selectedSprite);
    }

   

    // �ε� ��ư�� Ŭ���ϸ� ȣ��Ǵ� �޼���
    public void LoadAndApplySprite()
    {
        // �ε� �� �� ��ȯ
        Debug.Log("'Room_KGC' ������ �̵��մϴ�.");
        SceneManager.LoadScene("Room_KGC");  // "Room_KGC" ������ �̵�
    }
}
