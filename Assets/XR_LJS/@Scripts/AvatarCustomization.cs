using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // Photon 네트워킹을 사용하는 경우 추가

public class AvatarCustomization : MonoBehaviour
{
    public GameObject panel; // UI 패널
    public SpriteRenderer body; // 캐릭터의 스프라이트 렌더러
    public Image squareHeadDisplay; // 색상 미리보기 이미지
    public Color[] colors; // 색상 배열
    public int WhatColor = 1; // 선택된 색상 인덱스

    // 추가 UI 요소
    public SpriteRenderer eyes; // 눈의 스프라이트 렌더러
    public SpriteRenderer mouth; // 입의 스프라이트 렌더러
    public SpriteRenderer leftArm; // 왼팔의 스프라이트 렌더러
    public SpriteRenderer leftHand; // 왼손의 스프라이트 렌더러

    // 해당 요소의 스프라이트 옵션 배열
    public Sprite[] eyesOptions;
    public Sprite[] mouthOptions;
    public Sprite[] leftArmOptions;
    public Sprite[] leftHandOptions;

    // 색상 업데이트 및 스프라이트 설정
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

        // 눈, 입, 팔, 손의 스프라이트 업데이트
        eyes.sprite = eyesOptions[0]; // 첫 번째 옵션으로 설정 (인덱스 조정 필요)
        mouth.sprite = mouthOptions[0]; // 첫 번째 옵션으로 설정 (인덱스 조정 필요)
        leftArm.sprite = leftArmOptions[0]; // 첫 번째 옵션으로 설정 (인덱스 조정 필요)
        leftHand.sprite = leftHandOptions[0]; // 첫 번째 옵션으로 설정 (인덱스 조정 필요)
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

    // 추가 메서드: 눈, 입, 팔, 손 변경 메서드
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
