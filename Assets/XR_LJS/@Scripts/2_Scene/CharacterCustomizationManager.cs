using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CharacterCustomizationManager : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class CustomizationButtons
    {
        public Button nextButton;     // ���� ��ư
        public Button prevButton;     // ���� ��ư
        public TextMeshProUGUI countText;  // ���� ���õ� ������ ī��Ʈ �ؽ�Ʈ
    }

    [Header("ĳ���� ��������Ʈ ������")]
    [SerializeField] private SpriteRenderer characterBody;    // ĳ���� ����
    [SerializeField] private SpriteRenderer hairSprite;       // �Ӹ�ī��
    [SerializeField] private SpriteRenderer eyesSprite;       // ��
    [SerializeField] private SpriteRenderer pantSprite;       // ����
    [SerializeField] private SpriteRenderer rightArmSprite;   // ������ ��
    [SerializeField] private SpriteRenderer leftArmSprite;    // ���� ��
    [SerializeField] private SpriteRenderer rightLegSprite;   // ������ �ٸ�
    [SerializeField] private SpriteRenderer leftLegSprite;    // ���� �ٸ�
    [SerializeField] private SpriteRenderer rightShoesSprite; // ������ �Ź�
    [SerializeField] private SpriteRenderer leftShoesSprite;  // ���� �Ź�
    [SerializeField] private SpriteRenderer hatSprite;        // ����

    [Header("Ŀ���͸���¡ ��������Ʈ ���")]
    [SerializeField] private Sprite[] hairSprites;    // ��Ÿ�� ���
    [SerializeField] private Sprite[] eyesSprites;    // �� ��Ÿ�� ���
    [SerializeField] private Sprite[] bodySprites;    // ���� ��Ÿ�� ���
    [SerializeField] private Sprite[] pantSprites;    // ���� ��Ÿ�� ���
    [SerializeField] private Sprite[] armSprites;     // �� ��Ÿ�� ���
    [SerializeField] private Sprite[] legSprites;     // �ٸ� ��Ÿ�� ���
    [SerializeField] private Sprite[] shoesSprites;   // �Ź� ��Ÿ�� ���
    [SerializeField] private Sprite[] hatSprites;     // ���� ��Ÿ�� ���
    [SerializeField] private Color[] skinColors;      // �Ǻλ� ���

    [Header("UI ������Ʈ")]
    [SerializeField] private CustomizationButtons hairButtons;    // ��Ÿ�� ��ư
    [SerializeField] private CustomizationButtons eyesButtons;    // �� ��ư
    [SerializeField] private CustomizationButtons bodyButtons;    // ���� ��ư
    [SerializeField] private CustomizationButtons pantButtons;    // ���� ��ư
    [SerializeField] private CustomizationButtons armButtons;     // �� ��ư
    [SerializeField] private CustomizationButtons legButtons;     // �ٸ� ��ư
    [SerializeField] private CustomizationButtons shoesButtons;   // �Ź� ��ư
    [SerializeField] private CustomizationButtons hatButtons;     // ���� ��ư
    [SerializeField] private CustomizationButtons skinButtons;    // �Ǻλ� ��ư

    [SerializeField] private Button finishButton;                 // �Ϸ� ��ư
    [SerializeField] private TextMeshProUGUI customizationStatus; // ���� �ؽ�Ʈ

    private PhotonView photonView;    // ���� �� ������Ʈ
    private Data currentData;         // ���� Ŀ���͸���¡ ������

    // �ʱ�ȭ
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        currentData = new Data();
        LoadSavedCustomization();
    }

    void Start()
    {
        InitializeButtons();          // ��ư �ʱ�ȭ
        UpdateCharacterAppearance();  // ĳ���� ���� ������Ʈ
        UpdateAllCountTexts();        // ��� ī��Ʈ �ؽ�Ʈ ������Ʈ
        customizationStatus.text = "ĳ���͸� Ŀ���͸���¡�ϼ���";
    }

    // ��ư �ʱ�ȭ
    private void InitializeButtons()
    {
        // ��Ÿ�� ��ư
        hairButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Hair, 1));
        hairButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Hair, -1));

        // �� ��ư
        eyesButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Eyes, 1));
        eyesButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Eyes, -1));

        // ���� ��ư
        bodyButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Body, 1));
        bodyButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Body, -1));

        // ���� ��ư
        pantButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Pant, 1));
        pantButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Pant, -1));

        // �� ��ư
        armButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Arms, 1));
        armButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Arms, -1));

        // �ٸ� ��ư
        legButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Legs, 1));
        legButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Legs, -1));

        // �Ź� ��ư
        shoesButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Shoes, 1));
        shoesButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Shoes, -1));

        // ���� ��ư
        hatButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Hat, 1));
        hatButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Hat, -1));

        // �Ǻλ� ��ư
        skinButtons.nextButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Skin, 1));
        skinButtons.prevButton.onClick.AddListener(() => ChangeCustomization(CustomizationType.Skin, -1));

        // �Ϸ� ��ư
        finishButton.onClick.AddListener(FinishCustomization);
    }

    // Ŀ���͸���¡ Ÿ�� ������
    private enum CustomizationType
    {
        Hair,   // ��Ÿ��
        Eyes,   // ��
        Body,   // ����
        Pant,   // ����
        Arms,   // ��
        Legs,   // �ٸ�
        Shoes,  // �Ź�
        Hat,    // ����
        Skin    // �Ǻλ�
    }

    // ��� ī��Ʈ �ؽ�Ʈ ������Ʈ
    private void UpdateAllCountTexts()
    {
        UpdateCountText(hairButtons, currentData.hair, hairSprites.Length, "���");
        UpdateCountText(eyesButtons, currentData.eyes, eyesSprites.Length, "��");
        UpdateCountText(bodyButtons, currentData.body, bodySprites.Length, "����");
        UpdateCountText(pantButtons, currentData.pant, pantSprites.Length, "����");
        UpdateCountText(armButtons, currentData.rightArm, armSprites.Length, "��");
        UpdateCountText(legButtons, currentData.rightLeg, legSprites.Length, "�ٸ�");
        UpdateCountText(shoesButtons, currentData.rightShoes, shoesSprites.Length, "�Ź�");
        UpdateCountText(hatButtons, currentData.hat, hatSprites.Length, "����");
        UpdateCountText(skinButtons, currentData.skin, skinColors.Length, "�Ǻλ�");
    }

    // ���� ī��Ʈ �ؽ�Ʈ ������Ʈ
    private void UpdateCountText(CustomizationButtons buttons, int currentIndex, int maxCount, string partName)
    {
        if (buttons.countText != null)
        {
            buttons.countText.text = $"{partName}: {currentIndex + 1}/{maxCount}";
        }
    }

    // Ŀ���͸���¡ ����
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
                // ����, ������ �� ���� ����
                currentData.rightArm = currentData.leftArm =
                    (currentData.rightArm + direction + armSprites.Length) % armSprites.Length;
                break;
            case CustomizationType.Legs:
                // ����, ������ �ٸ� ���� ����
                currentData.rightLeg = currentData.leftLeg =
                    (currentData.rightLeg + direction + legSprites.Length) % legSprites.Length;
                break;
            case CustomizationType.Shoes:
                // ����, ������ �Ź� ���� ����
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

    // ĳ���� ���� ������Ʈ
    private void UpdateCharacterAppearance()
    {
        if (photonView.IsMine)
        {
            // ��� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("SyncCharacterAppearance", RpcTarget.All,
                JsonUtility.ToJson(currentData));
        }
    }

    // ĳ���� ���� ����ȭ (RPC)
    [PunRPC]
    private void SyncCharacterAppearance(string jsonData)
    {
        currentData = JsonUtility.FromJson<Data>(jsonData);

        // ��� ��������Ʈ ������Ʈ
        hairSprite.sprite = hairSprites[Mathf.Clamp(currentData.hair, 0, hairSprites.Length - 1)];
        eyesSprite.sprite = eyesSprites[Mathf.Clamp(currentData.eyes, 0, eyesSprites.Length - 1)];
        characterBody.sprite = bodySprites[Mathf.Clamp(currentData.body, 0, bodySprites.Length - 1)];
        pantSprite.sprite = pantSprites[Mathf.Clamp(currentData.pant, 0, pantSprites.Length - 1)];

        // �� ������Ʈ
        rightArmSprite.sprite = armSprites[Mathf.Clamp(currentData.rightArm, 0, armSprites.Length - 1)];
        leftArmSprite.sprite = armSprites[Mathf.Clamp(currentData.leftArm, 0, armSprites.Length - 1)];

        // �ٸ� ������Ʈ
        rightLegSprite.sprite = legSprites[Mathf.Clamp(currentData.rightLeg, 0, legSprites.Length - 1)];
        leftLegSprite.sprite = legSprites[Mathf.Clamp(currentData.leftLeg, 0, legSprites.Length - 1)];

        // �Ź� ������Ʈ
        rightShoesSprite.sprite = shoesSprites[Mathf.Clamp(currentData.rightShoes, 0, shoesSprites.Length - 1)];
        leftShoesSprite.sprite = shoesSprites[Mathf.Clamp(currentData.leftShoes, 0, shoesSprites.Length - 1)];

        // ���� ������Ʈ (���� ����)
        if (hatSprite != null && currentData.hat >= 0)
        {
            hatSprite.sprite = hatSprites[Mathf.Clamp(currentData.hat, 0, hatSprites.Length - 1)];
            hatSprite.enabled = currentData.hat > 0;  // ���� ���õ��� �ʾ����� ����
        }

        // �Ǻλ� ������Ʈ
        characterBody.color = currentData.skinColor;
    }

    // ����� Ŀ���͸���¡ �ҷ�����
    private void LoadSavedCustomization()
    {
        if (PlayerPrefs.HasKey("CharacterData"))
        {
            string jsonData = PlayerPrefs.GetString("CharacterData");
            currentData = JsonUtility.FromJson<Data>(jsonData);
        }
    }

    // Ŀ���͸���¡ ����
    private void SaveCustomization()
    {
        string jsonData = JsonUtility.ToJson(currentData);
        PlayerPrefs.SetString("CharacterData", jsonData);
        PlayerPrefs.Save();

        // PlayerData �̱��濡�� ����
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.data = currentData;
        }
    }

    // Ŀ���͸���¡ �Ϸ�
    public void FinishCustomization()
    {
        if (!photonView.IsMine) return;

        SaveCustomization();
        customizationStatus.text = "Ŀ���͸���¡ �Ϸ�! ���� ������ �̵��մϴ�...";

        // ������ Ŭ���̾�Ʈ�� �� ��ȯ ����
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