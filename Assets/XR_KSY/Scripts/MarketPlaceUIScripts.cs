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
using Photon.Pun;
using System.Text;

public class MarketplaceUI : MonoBehaviour
{
    private static readonly string baseUrl = "http://13.124.6.53:8080/api/v1/market";

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

    public TextMeshProUGUI money;

    public GameObject chu;
    public GameObject gumae;

    private void Start()
    {
        Debug.Log("Marketplace UI Start Initialized");

        uploadButton.onClick.AddListener(OpenFilePicker);
        purchaseButton.onClick.AddListener(PurchaseSelectedItem);
        menuBtn.SetActive(true);
        downPop.SetActive(false);
        upPop.SetActive(false);
        chu.SetActive(false);
        gumae.SetActive(false);
    }

    public void OnCategoryButtonClicked()
    {
        StartCoroutine(GetAllItems(DisplayItems));
    }

    public void OpenDownloadPopUp()
    {
        Debug.Log("Opening Download Popup");
        menuBtn.SetActive(false);
        downPop.SetActive(true);
    }

    public void OpenUploadPopUp()
    {
        Debug.Log("Opening Upload Popup");
        menuBtn.SetActive(false);
        upPop.SetActive(true);
    }

    public void SetPopBtn()
    {
        Debug.Log("Resetting Popups");
        menuBtn.SetActive(true);
        downPop.SetActive(false);
        upPop.SetActive(false);
    }

    public void ChuGu()
    {
        Debug.Log("Closing Purchase Feedback Popups");
        chu.SetActive(false);
        gumae.SetActive(false);
    }

    public void Backback()
    {
        if (menuBtn.activeSelf)
        {
            Debug.Log("Returning to Room Scene");
            PhotonNetwork.LoadLevel("Room_KGC");
        }
        else
        {
            Debug.Log("Returning to Main Menu");
            menuBtn.SetActive(true);
            downPop.SetActive(false);
            upPop.SetActive(false);
        }
    }

    private void OpenFilePicker()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Select a PNG File", "", "png", false);
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            selectedFilePath = paths[0];
            Debug.Log($"File Selected: {selectedFilePath}");
            ReplaceSprite(selectedFilePath);
        }
        else
        {
            Debug.LogWarning("No file selected.");
        }
        nameInputField.text = null;
        categoryDropdown.value = 0;
    }

    private void ReplaceSprite(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found at path: {filePath}");
            return;
        }

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            showUpImage.sprite = newSprite;
            Debug.Log("Sprite successfully replaced.");
        }
        else
        {
            Debug.LogError("Failed to load image data.");
        }
    }

    public void OnUploadButtonClick()
    {
        string itemName = nameInputField.text;
        string description = "설명";
        float price = float.Parse("20"); // 가격 입력 필드 (문자열 -> 숫자 변환)
        int makerId = 0; // 현재 사용자 ID를 가져옴
        string category = "카테고리1"; // 선택된 카테고리
        string filePath = selectedFilePath;

        if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(category))
        {
            Debug.Log($"Uploading item: {itemName} in Category {category}");
            StartCoroutine(UploadItem(itemName, description, price, makerId, category, filePath));
            chu.SetActive(true); // 업로드 상태 UI 활성화
        }
        else
        {
            Debug.LogError("Upload failed: Please provide all required information.");
        }
    }
    private IEnumerator GetAllItems(System.Action<List<Item>> callback)
    {
        string url = $"{baseUrl}/item"; // 전체 아이템을 가져오는 API URL

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error fetching items: {www.error}");
            }
            else
            {
                string json = www.downloadHandler.text;
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json); // 아이템 목록 파싱
                callback(items); // 아이템 목록을 콜백으로 전달
            }
        }
    }
    private void DisplayItems(List<Item> items)
    {
        Debug.Log($"Displaying {items.Count} items");

        // 1. 아이템 리스트가 null인지 확인
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("No items to display.");
            return;
        }

        displayedItems = items;

        // 2. 콘텐츠 영역에 기존 아이템을 모두 삭제
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 3. 아이템 생성 및 UI에 추가
        foreach (Item item in items)
        {
            if (item == null) continue;  // 아이템이 null일 경우 건너뛰기

            GameObject itemObj = Instantiate(itemPrefab, content);
            if (itemObj == null) continue; // 아이템 객체 생성에 실패할 경우 건너뛰기

            // 4. UI 요소에 아이템 이름 설정
            //Text itemNameText = itemObj.transform.Find("ItemName")?.GetComponent<Text>();
            //if (itemNameText != null)
            //{
            //    itemNameText.text = item.name;
            //}
            //else
            //{
            //    Debug.LogError("Item name UI element not found.");
            //}

            // 5. 아이템 클릭 시 선택하도록 이벤트 추가
            Button itemButton = itemObj.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => SelectItemForPurchase(item));
            }
            else
            {
                Debug.LogError("Button component not found in item prefab.");
            }

            // 6. 이미지 로딩 (UI 요소가 null인지 체크)
            Image itemImage = itemObj.transform.Find("ItemImage")?.GetComponent<Image>();
            if (itemImage != null)
            {
                StartCoroutine(LoadImage(item.imageUrl, itemImage));
            }
            else
            {
                Debug.Log("Image component not found in item prefab.");
            }

            Debug.Log($"Item Added: {item.name}");
        }
    }

    public void SelectItemForPurchase(Item item)
    {
        Debug.Log($"Item Selected for Purchase: {item.name} (ID: {item.id})");
        selectedItemForPurchase = item;
    }

    public void PurchaseSelectedItem()
    {
        if (selectedItemForPurchase != null)
        {
            Debug.Log($"Attempting to purchase: {selectedItemForPurchase.name} (ID: {selectedItemForPurchase.id})");
            StartCoroutine(PurchaseItem(selectedItemForPurchase.id, success =>
            {
                if (success)
                {
                    Debug.Log($"Purchase successful for {selectedItemForPurchase.name}");
                    StartCoroutine(GetItemsByCategory(DisplayItems));
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

    private IEnumerator LoadImage(string url, Image image)
    {
        Debug.Log($"Loading image from URL: {url}");
        url = $"{baseUrl}/item";
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                Debug.Log("Image loaded successfully.");
            }
            else
            {
                Debug.LogError($"Error loading image: {www.error}");
            }
        }
    }

    private IEnumerator UploadItem(string itemName, string description, float price, int makerId, string category, string filePath)
    {
        Debug.Log($"Starting Upload for {itemName} in Category {category}");
        string url = $"{baseUrl}/item";

        // JSON 데이터 생성
        UploadItemRequest jsonData = new UploadItemRequest
        {
            makerId = makerId,
            name = itemName,
            description = description,
            price = price,
            category = category
        };

        // JSON 직렬화
        string jsonString = JsonUtility.ToJson(jsonData);
        Debug.Log($"Request JSON: {jsonString}");

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            // JSON 문자열을 UTF8로 바이트 배열로 변환하여 전송
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // 요청 보내기
            Debug.Log($"Request JSON: {jsonString}");
            yield return www.SendWebRequest();

            // 응답 확인
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Upload failed: {www.error}\nResponse: {www.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"Upload successful: {www.downloadHandler.text}");
            }
        }
    }

    // 멀티파트 폼 데이터 생성 함수
    private byte[] CreateMultipartFormData(string json, byte[] fileData, string filePath, string boundary)
    {
        var formData = new List<byte>();

        // JSON 데이터 추가
        string jsonPart = $"--{boundary}\r\n" +
                          "Content-Disposition: form-data; name=\"metadata\"\r\n\r\n" +
                          $"{json}\r\n";
        formData.AddRange(Encoding.UTF8.GetBytes(jsonPart));

        // 파일 데이터 추가
        string fileHeader = $"--{boundary}\r\n" +
                            $"Content-Disposition: form-data; name=\"file\"; filename=\"{Path.GetFileName(filePath)}\"\r\n" +
                            "Content-Type: image/png\r\n\r\n";
        formData.AddRange(Encoding.UTF8.GetBytes(fileHeader));
        formData.AddRange(fileData);
        formData.AddRange(Encoding.UTF8.GetBytes("\r\n"));

        // 끝 경계 추가
        string endBoundary = $"--{boundary}--\r\n";
        formData.AddRange(Encoding.UTF8.GetBytes(endBoundary));

        return formData.ToArray();
    }

    private IEnumerator GetItemsByCategory(System.Action<List<Item>> callback)
    {
        string url = $"{baseUrl}/item";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error fetching items: {www.error}");
            }
            else
            {
                string json = www.downloadHandler.text;
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                callback(items);
            }
        }
    }

    private IEnumerator PurchaseItem(string itemId, System.Action<bool> callback)
    {
        Debug.Log($"Attempting purchase for Item ID: {itemId}");
        string url = $"{baseUrl}/item?itemId={UnityWebRequest.EscapeURL(itemId)}"; // 쿼리 문자열로 itemId 추가

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Purchase failed: {www.error}");
                callback(false);
            }
            else
            {
                Debug.Log($"Purchase successful: {www.downloadHandler.text}");
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
    public string category;
}

[System.Serializable]
public class UploadItemRequest
{
    public int makerId;
    public string name;
    public string description;
    public float price;
    public string category;
}
