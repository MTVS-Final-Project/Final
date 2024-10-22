using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CharacterCustomizationManager : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class CustomizationButtons
    {
        public Button nextButton;     // 다음 버튼
        public Button prevButton;     // 이전 버튼
        public TextMeshProUGUI countText;  // 현재 선택된 아이템 카운트 텍스트
    }

    [Header("캐릭터 스프라이트 렌더러")]
    [SerializeField] private SpriteRenderer characterBody;    // 캐릭터 몸통
    [SerializeField] private SpriteRenderer hairSprite;       // 머리카락
    [SerializeField] private SpriteRenderer eyesSprite;       // 눈
    [SerializeField] private SpriteRenderer pantSprite;       // 바지
    [SerializeField] private SpriteRenderer rightArmSprite;   // 오른쪽 팔
    [SerializeField] private SpriteRenderer leftArmSprite;    // 왼쪽 팔
    [SerializeField] private SpriteRenderer rightLegSprite;   // 오른쪽 다리
    [SerializeField] private SpriteRenderer leftLegSprite;    // 왼쪽 다리
    [SerializeField] private SpriteRenderer rightShoesSprite; // 오른쪽 신발
    [SerializeField] private SpriteRenderer leftShoesSprite;  // 왼쪽 신발
    [SerializeField] private SpriteRenderer hatSprite;        // 모자

    [Header("커스터마이징 스프라이트 목록")]
    [SerializeField] private Sprite[] hairSprites;    // 헤어스타일 목록
    [SerializeField] private Sprite[] eyesSprites;    // 눈 스타일 목록
    [SerializeField] private Sprite[] bodySprites;    // 상의 스타일 목록
    [SerializeField] private Sprite[] pantSprites;    // 하의 스타일 목록
    [SerializeField] private Sprite[] armSprites;     // 팔 스타일 목록
    [SerializeField] private Sprite[] legSprites;     // 다리 스타일 목록
    [SerializeField] private Sprite[] shoesSprites;   // 신발 스타일 목록
    [SerializeField] private Sprite[] hatSprites;     // 모자 스타일 목록
    [SerializeField] private Color[] skinColors;      // 피부색 목록

    [Header("UI 컴포넌트")]
    [SerializeField] private CustomizationButtons hairButtons;    // 헤어스타일 버튼
    [SerializeField] private CustomizationButtons eyesButtons;    // 눈 버튼
    [SerializeField] private CustomizationButtons bodyButtons;    // 상의 버튼
    [SerializeField] private CustomizationButtons pantButtons;    // 하의 버튼
    [SerializeField] private CustomizationButtons armButtons;     // 팔 버튼
    [SerializeField] private CustomizationButtons legButtons;     // 다리 버튼
    [SerializeField] private CustomizationButtons shoesButtons;   // 신발 버튼
    [SerializeField] private CustomizationButtons hatButtons;     // 모자 버튼
    [SerializeField] private CustomizationButtons skinButtons;    // 피부색 버튼

    [SerializeField] private Button finishButton;                 // 완료 버튼
    [SerializeField] private TextMeshProUGUI customizationStatus; // 상태 텍스트

    private PhotonView photonView;    // 포톤 뷰 컴포넌트
    private Data currentData;         // 현재 커스터마이징 데이터

    // 초기화
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        currentData = new Data();
        LoadSavedCustomization();
    }

    void Start()
    {
        InitializeButtons();          // 버튼 초기화
        UpdateCharacterAppearance();  // 캐릭터 외형 업데이트
        UpdateAllCountTexts();        // 모든 카운트 텍스트 업데이트
        customizationStatus.text = "캐릭터를 커스터마이징하세요";
    }

    // 버튼 초기화
    private void InitializeButtons()
    {
        // 헤어스타일 버튼
        hairButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Hair, 1));
        hairButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Hair, -1));

        // 눈 버튼
        eyesButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Eyes, 1));
        eyesButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Eyes, -1));

        // 상의 버튼
        bodyButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Body, 1));
        bodyButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Body, -1));

        // 하의 버튼
        pantButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Pant, 1));
        pantButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Pant, -1));

        // 팔 버튼
        armButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Arms, 1));
        armButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Arms, -1));

        // 다리 버튼
        legButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Legs, 1));
        legButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Legs, -1));

        // 신발 버튼
        shoesButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Shoes, 1));
        shoesButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Shoes, -1));

        // 모자 버튼
        hatButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Hat, 1));
        hatButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Hat, -1));

        // 피부색 버튼
        skinButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Skin, 1));
        skinButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Skin, -1));

        // 완료 버튼
        finishButton.onClick.AddListener(FinishCustomization);
    }

    // 커스터마이징 타입 열거형
    private enum CustomizationType
    {
        Hair,   // 헤어스타일
        Eyes,   // 눈
        Body,   // 상의
        Pant,   // 하의
        Arms,   // 팔
        Legs,   // 다리
        Shoes,  // 신발
        Hat,    // 모자
        Skin    // 피부색
    }

    // 모든 카운트 텍스트 업데이트
    private void UpdateAllCountTexts()
    {
        UpdateCountText(hairButtons, currentData.hair, hairSprites.Length, "헤어");
        UpdateCountText(eyesButtons, currentData.eyes, eyesSprites.Length, "눈");
        UpdateCountText(bodyButtons, currentData.body, bodySprites.Length, "상의");
        UpdateCountText(pantButtons, currentData.pant, pantSprites.Length, "하의");
        UpdateCountText(armButtons, currentData.rightArm, armSprites.Length, "팔");
        UpdateCountText(legButtons, currentData.rightLeg, legSprites.Length, "다리");
        UpdateCountText(shoesButtons, currentData.rightShoes, shoesSprites.Length, "신발");
        UpdateCountText(hatButtons, currentData.hat, hatSprites.Length, "모자");
        UpdateCountText(skinButtons, currentData.skin, skinColors.Length, "피부색");
    }

    // 개별 카운트 텍스트 업데이트
    private void UpdateCountText(CustomizationButtons buttons, int currentIndex, int maxCount, string partName)
    {
        if (buttons.countText != null)
        {
            buttons.countText.text = $"{partName}: {currentIndex + 1}/{maxCount}";
        }
    }

    // 커스터마이징 변경
    private void ChangeCustomization(CustomizationType type, int direction)
    {
        if (!photonView.IsMine) return;

        switch (type)
        {
            case CustomizationType.Hair:
                currentData.hair = (currentData.hair + direction + hairSprites.Length) % hairSprites.Length;
                break;
            case CustomizationType.Eyes:
                currentData.eyes = (currentData.eyes + direction + eyesSprites.Length) % eyesSprites.Length;
                break;
            case CustomizationType.Body:
                currentData.body = (currentData.body + direction + bodySprites.Length) % bodySprites.Length;
                break;
            case CustomizationType.Pant:
                currentData.pant = (currentData.pant + direction + pantSprites.Length) % pantSprites.Length;
                break;
            case CustomizationType.Arms:
                // 왼쪽, 오른쪽 팔 동시 변경
                currentData.rightArm = currentData.leftArm =
                    (currentData.rightArm + direction + armSprites.Length) % armSprites.Length;
                break;
            case CustomizationType.Legs:
                // 왼쪽, 오른쪽 다리 동시 변경
                currentData.rightLeg = currentData.leftLeg =
                    (currentData.rightLeg + direction + legSprites.Length) % legSprites.Length;
                break;
            case CustomizationType.Shoes:
                // 왼쪽, 오른쪽 신발 동시 변경
                currentData.rightShoes = currentData.leftShoes =
                    (currentData.rightShoes + direction + shoesSprites.Length) % shoesSprites.Length;
                break;
            case CustomizationType.Hat:
                currentData.hat = (currentData.hat + direction + hatSprites.Length) % hatSprites.Length;
                break;
            case CustomizationType.Skin:
                currentData.skin = (currentData.skin + direction + skinColors.Length) % skinColors.Length;
                currentData.skinColor = skinColors[currentData.skin];
                break;
        }

        UpdateCharacterAppearance();
        UpdateAllCountTexts();
    }

    // 캐릭터 외형 업데이트
    private void UpdateCharacterAppearance()
    {
        if (photonView.IsMine)
        {
            // 모든 클라이언트에 동기화
            photonView.RPC("SyncCharacterAppearance", RpcTarget.All,
                JsonUtility.ToJson(currentData));
        }
    }

    // 캐릭터 외형 동기화 (RPC)
    [PunRPC]
    private void SyncCharacterAppearance(string jsonData)
    {
        currentData = JsonUtility.FromJson<Data>(jsonData);

        // 모든 스프라이트 업데이트
        hairSprite.sprite = hairSprites[Mathf.Clamp(currentData.hair, 0, hairSprites.Length - 1)];
        eyesSprite.sprite = eyesSprites[Mathf.Clamp(currentData.eyes, 0, eyesSprites.Length - 1)];
        characterBody.sprite = bodySprites[Mathf.Clamp(currentData.body, 0, bodySprites.Length - 1)];
        pantSprite.sprite = pantSprites[Mathf.Clamp(currentData.pant, 0, pantSprites.Length - 1)];

        // 팔 업데이트
        rightArmSprite.sprite = armSprites[Mathf.Clamp(currentData.rightArm, 0, armSprites.Length - 1)];
        leftArmSprite.sprite = armSprites[Mathf.Clamp(currentData.leftArm, 0, armSprites.Length - 1)];

        // 다리 업데이트
        rightLegSprite.sprite = legSprites[Mathf.Clamp(currentData.rightLeg, 0, legSprites.Length - 1)];
        leftLegSprite.sprite = legSprites[Mathf.Clamp(currentData.leftLeg, 0, legSprites.Length - 1)];

        // 신발 업데이트
        rightShoesSprite.sprite = shoesSprites[Mathf.Clamp(currentData.rightShoes, 0, shoesSprites.Length - 1)];
        leftShoesSprite.sprite = shoesSprites[Mathf.Clamp(currentData.leftShoes, 0, shoesSprites.Length - 1)];

        // 모자 업데이트 (선택 사항)
        if (hatSprite != null && currentData.hat >= 0)
        {
            hatSprite.sprite = hatSprites[Mathf.Clamp(currentData.hat, 0, hatSprites.Length - 1)];
            hatSprite.enabled = currentData.hat > 0;  // 모자 선택되지 않았으면 숨김
        }

        // 피부색 업데이트
        characterBody.color = currentData.skinColor;
    }

    // 저장된 커스터마이징 불러오기
    private void LoadSavedCustomization()
    {
        if (PlayerPrefs.HasKey("CharacterData"))
        {
            string jsonData = PlayerPrefs.GetString("CharacterData");
            currentData = JsonUtility.FromJson<Data>(jsonData);
        }
    }

    // 커스터마이징 저장
    private void SaveCustomization()
    {
        string jsonData = JsonUtility.ToJson(currentData);
        PlayerPrefs.SetString("CharacterData", jsonData);
        PlayerPrefs.Save();

        // PlayerData 싱글톤에도 저장
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.data = currentData;
        }
    }

    // 커스터마이징 완료
    public void FinishCustomization()
    {
        if (!photonView.IsMine) return;

        SaveCustomization();
        customizationStatus.text = "커스터마이징 완료! 게임 씬으로 이동합니다...";

        // 마스터 클라이언트만 씬 전환 가능
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(JsonUtility.ToJson(currentData));
        }
        else
        {
            currentData = JsonUtility.FromJson<Data>((string)stream.ReceiveNext());
            UpdateCharacterAppearance();
        }

    }

}