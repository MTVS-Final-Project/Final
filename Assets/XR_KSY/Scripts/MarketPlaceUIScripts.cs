using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SFB;
using UnityEngine.SceneManagement;
//using Unity.Android.Gradle.Manifest;
using UnityEngine.InputSystem;
using Photon.Pun;

public class MarketplaceUI : MonoBehaviour
{
    private static readonly string baseUrl = "https://your-swagger-server.com/api";

    public Image showUpImage;
    public Image showImage;

    public TMP_InputField nameInputField;
    public TMP_Dropdown categoryDropdown;
    public Button uploadButton;
    public Button purchaseButton;
    public Transform content;
    public GameObject itemPrefab;

    private int selectedCategory;
    private string selectedFilePath;
    private List<Item> displayedItems;
    private Item selectedItemForPurchase;

    public GameObject menuBtn;
    public GameObject downPop;
    public GameObject upPop;

    public Button[] categoryBtns;

    public Sprite gagu;

    private bool uploadTest;

    public Button gagutest;
    public GameObject gT;

    public TextMeshProUGUI money;

    private bool uploadTest2;

    public GameObject chu;
    public GameObject gumae;

    private void Start()
    {
        uploadButton.onClick.AddListener(OpenFilePicker);
        purchaseButton.onClick.AddListener(PurchaseSelectedItem);
        menuBtn.SetActive(true);
        downPop.SetActive(false);
        upPop.SetActive(false);
        uploadTest = false;
        gT.SetActive(false);
        chu.SetActive(false);
        gumae.SetActive(false);

        for (int i = 0; i < categoryBtns.Length; i++)
        {
            int categoryId = i + 1;
            categoryBtns[i].onClick.AddListener(() => OnCategoryButtonClicked(categoryId));
        }
    }

    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            uploadTest = true;
            Debug.Log("uploadTest = true");
        }
    }



    private void OnCategoryButtonClicked(int categoryId)
    {
        selectedCategory = categoryId;
        StartCoroutine(GetItemsByCategory(selectedCategory, DisplayItems));
    }

    public void OpenDownloadPopUp()
    {
        menuBtn.SetActive(false);
        downPop.SetActive(true);
    }

    public void OpenUploadPopUp()
    {
        menuBtn.SetActive(false);
        upPop.SetActive(true);
    }

    public void SetPopBtn()
    {
        menuBtn.SetActive(true) ;
        downPop.SetActive(false) ;
        upPop.SetActive(false) ;
    }

    public void ChuGu()
    {
        chu.SetActive(false);
        gumae.SetActive(false);
    }

    public void Backback()
    {
        if(menuBtn.activeSelf)
        {
            //SceneManager.LoadScene(5);
            PhotonNetwork.LoadLevel("Room_KGC");
        }
        else
        {
            menuBtn.SetActive(true);
            downPop.SetActive(false);
            upPop.SetActive(false);
        }
    }

    // 파일 탐색기에서 PNG 파일 선택
    private void OpenFilePicker()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Select a PNG File", "", "png", false);
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            selectedFilePath = paths[0];
            Debug.Log("Selected file: " + selectedFilePath);
        }
        showUpImage.sprite = gagu;
        nameInputField.text = null;
        categoryDropdown.value = (1);
    }

    // 아이템 업로드
    public void OnUploadButtonClick()
    {
        string itemName = nameInputField.text;
        selectedCategory = categoryDropdown.value;

        if (/*!string.IsNullOrEmpty(selectedFilePath) &&*/ !string.IsNullOrEmpty(itemName))
        {
            StartCoroutine(UploadItem(itemName, selectedCategory/*, selectedFilePath*/));
            chu.SetActive(true);
        }
        else
        {
            Debug.LogError("Please select a file and enter a name.");
        }
    }

    // 선택된 카테고리의 아이템 목록을 ScrollView에 표시
    private void DisplayItems(List<Item> items)
    {
        displayedItems = items;

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in items)
        {
            GameObject itemObj = Instantiate(itemPrefab, content);
            itemObj.transform.Find("ItemName").GetComponent<Text>().text = item.name;
            itemObj.GetComponent<Button>().onClick.AddListener(() => SelectItemForPurchase(item));

            // 이미지 로드
            StartCoroutine(LoadImage(item.imageUrl, itemObj.transform.Find("ItemImage").GetComponent<Image>()));
        }
    }

    public void TestCode()
    {
        if(uploadTest == true)
        {
            gT.SetActive(true);
        }
        else
        {
            gT.SetActive(false);
        }
    }

    public void TestShow()
    {
        showImage.sprite = gagu;
        uploadTest2 = true;
    }

    public void TestBuy()
    {
        if(uploadTest2 == true)
        {
            money.text = "0";
            gumae.SetActive(true);
        }
        
    }

    // 선택한 아이템을 구매 대상으로 설정
    private void SelectItemForPurchase(Item item)
    {
        selectedItemForPurchase = item;
        showImage.sprite = gagu;
        Debug.Log("Selected item for purchase: " + item.name);
    }

    // 아이템 구매 요청
    private void PurchaseSelectedItem()
    {
        if (selectedItemForPurchase != null)
        {
            StartCoroutine(PurchaseItem(selectedItemForPurchase.id, success =>
            {
                if (success)
                {
                    Debug.Log("Purchase successful: " + selectedItemForPurchase.name);
                    StartCoroutine(GetItemsByCategory(selectedCategory, DisplayItems));
                }
                else
                {
                    Debug.LogError("Purchase failed.");
                }
            }));
        }
        else
        {
            Debug.LogError("No item selected for purchase.");
        }
    }

    // 이미지 로드
    private IEnumerator LoadImage(string url, Image image)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                // Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                // image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                image.sprite = gagu;
                
            }
            else
            {
                image.sprite = gagu;
                Debug.LogError("Error loading image: " + www.error);
            }
        }
    }

    // 서버로 아이템 업로드
    private IEnumerator UploadItem(string itemName, int category/*, string filePath*/)
    {
        string url = $"{baseUrl}/upload";
        //byte[] fileData = File.ReadAllBytes(filePath);

        WWWForm form = new WWWForm();
        form.AddField("name", itemName);
        form.AddField("category", category);
        //form.AddBinaryData("file", fileData, Path.GetFileName(filePath), "image/png");

        uploadTest = true;

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error uploading file: " + www.error);
            }
            else
            {
                Debug.Log("File uploaded successfully: " + www.downloadHandler.text);
            }
        }
    }

    // 서버로부터 카테고리별 아이템 목록 요청
    private IEnumerator GetItemsByCategory(int category, System.Action<List<Item>> callback)
    {
        string url = $"{baseUrl}/items?category=" + category;

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching items: " + www.error);

            }
            else
            {
                string json = www.downloadHandler.text;
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                callback(items);
            }
        }
    }

    // 서버에 구매 요청
    private IEnumerator PurchaseItem(string itemId, System.Action<bool> callback)
    {
        string url = $"{baseUrl}/purchase";
        WWWForm form = new WWWForm();
        form.AddField("itemId", itemId);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error purchasing item: " + www.error);
                callback(false);
            }
            else
            {
                Debug.Log("Item purchased successfully: " + www.downloadHandler.text);
                callback(true);
            }
        }
    }
}

public class Item
{
    public string id;
    public string name;
    public string imageUrl;
    public int category;
}
