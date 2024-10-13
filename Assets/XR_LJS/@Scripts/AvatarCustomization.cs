using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // Photon ��Ʈ��ŷ�� ����ϴ� ��� �߰�

public class AvatarCustomization : MonoBehaviour
{
    public GameObject panel; // UI �г�
    public SpriteRenderer body; // ĳ������ ��������Ʈ ������
    public Image squareHeadDisplay; // ���� �̸����� �̹���
    public Color[] colors; // ���� �迭
    public int WhatColor = 1; // ���õ� ���� �ε���

    // �߰� UI ���
    public SpriteRenderer eyes; // ���� ��������Ʈ ������
    public SpriteRenderer mouth; // ���� ��������Ʈ ������
    public SpriteRenderer leftArm; // ������ ��������Ʈ ������
    public SpriteRenderer leftHand; // �޼��� ��������Ʈ ������

    // �ش� ����� ��������Ʈ �ɼ� �迭
    public Sprite[] eyesOptions;
    public Sprite[] mouthOptions;
    public Sprite[] leftArmOptions;
    public Sprite[] leftHandOptions;

    // ���� ������Ʈ �� ��������Ʈ ����
    void Update()
    {
        squareHeadDisplay.color = body.color;

        for (int i = 0; i < colors.Length; i++)
        {
            if (i == WhatColor)
            {
                body.color = colors[i];
            }
        }

        // ��, ��, ��, ���� ��������Ʈ ������Ʈ
        eyes.sprite = eyesOptions[0]; // ù ��° �ɼ����� ���� (�ε��� ���� �ʿ�)
        mouth.sprite = mouthOptions[0]; // ù ��° �ɼ����� ���� (�ε��� ���� �ʿ�)
        leftArm.sprite = leftArmOptions[0]; // ù ��° �ɼ����� ���� (�ε��� ���� �ʿ�)
        leftHand.sprite = leftHandOptions[0]; // ù ��° �ɼ����� ���� (�ε��� ���� �ʿ�)
    }

    public void ChangePanelState(bool state)
    {
        panel.SetActive(state);
    }

    public void ChangeHeadColor(int index)
    {
        WhatColor = index;
    }

    public void LoadSecondScene()
    {
        PhotonNetwork.LoadLevel("SecondScene_LJS");
    }

    // �߰� �޼���: ��, ��, ��, �� ���� �޼���
    public void ChangeEyes(int index)
    {
        if (index >= 0 && index < eyesOptions.Length)
        {
            eyes.sprite = eyesOptions[index];
        }
    }

    public void ChangeMouth(int index)
    {
        if (index >= 0 && index < mouthOptions.Length)
        {
            mouth.sprite = mouthOptions[index];
        }
    }

    public void ChangeLeftArm(int index)
    {
        if (index >= 0 && index < leftArmOptions.Length)
        {
            leftArm.sprite = leftArmOptions[index];
        }
    }

    public void ChangeLeftHand(int index)
    {
        if (index >= 0 && index < leftHandOptions.Length)
        {
            leftHand.sprite = leftHandOptions[index];
        }
    }
}
