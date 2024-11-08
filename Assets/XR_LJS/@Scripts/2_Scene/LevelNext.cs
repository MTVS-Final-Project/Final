using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNext : MonoBehaviourPunCallbacks
{
    private int selectedSprite = 0;  // �⺻���� ���õ� ��������Ʈ �ε���
    public List<Sprite> playerSprites = new List<Sprite>();  // �÷��̾� ��������Ʈ ����Ʈ
    public Button saveButton;  // ���̺� ��ư
    public Button loadButton;  // �ε� ��ư

    // Start �޼���: ��ư Ŭ�� �̺�Ʈ ����
    void Start()
    {
        // ���̺� ��ư Ŭ�� �� Save �޼��� ȣ��
        saveButton.onClick.AddListener(SaveAndPrepareForSceneChange);

        // �ε� ��ư Ŭ�� �� Load �޼��� ȣ��
        loadButton.onClick.AddListener(LoadAndApplySprite);

        // ����� �α�: ���� ���� ��
        Debug.Log("LevelNext ��ũ��Ʈ�� ���۵Ǿ����ϴ�. �ʱ� ���õ� ��������Ʈ �ε���: " + selectedSprite);
    }

    // ���̺� ��ư�� Ŭ���ϸ� ȣ��Ǵ� �޼���
    public void SaveAndPrepareForSceneChange()
    {
        // ����� �α�: ���̺� ��ư Ŭ�� ��
        Debug.Log("���̺� ��ư�� Ŭ���Ǿ����ϴ�. ���õ� ��������Ʈ �ε���: " + selectedSprite);

        // ���̺� ���
        string filePath = Application.persistentDataPath + "/PlayerData.txt";  // ���� ��� (���� ������ ��η� ����)
        Debug.Log("���õ� ��������Ʈ �ε���(" + selectedSprite + ")�� ���Ͽ� �����մϴ�. ���: " + filePath);

        // ���õ� ��������Ʈ �ε����� ���Ͽ� ����
        File.WriteAllText(filePath, selectedSprite.ToString());

        // ���õ� ��������Ʈ�� ���� ���� ������Ʈ�� ���� (SpriteRenderer�� ����)
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && playerSprites.Count > selectedSprite)
        {
            spriteRenderer.sprite = playerSprites[selectedSprite];
            Debug.Log("���õ� ��������Ʈ�� ����Ǿ����ϴ�: " + playerSprites[selectedSprite].name);
        }
        else
        {
            Debug.LogWarning("SpriteRenderer�� ���ų� ���õ� ��������Ʈ�� ��ȿ���� �ʽ��ϴ�.");
        }

        // ���̺� �Ϸ� �� �� ��ȯ �غ�
        Debug.Log("���̺갡 �Ϸ�Ǿ����ϴ�. '���� ��' ��ư�� Ŭ���Ͽ� ���� ��ȯ�ϼ���.");
    }

    // �ε� ��ư�� Ŭ���ϸ� ȣ��Ǵ� �޼���
    public void LoadAndApplySprite()
    {
        // �ε� �� �� ��ȯ
        Debug.Log("'Room_KGC' ������ �̵��մϴ�.");
        SceneManager.LoadScene("Room_KGC");  // "Room_KGC" ������ �̵�
    }
}
