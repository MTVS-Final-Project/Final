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
using System.Reflection.Emit;
using UnityEngine.EventSystems;

public class MarketplaceUI : MonoBehaviour
{
    private static readonly string baseUrl = "http://13.124.6.53:8080/api/v1/market";

    public Image showUpImage;
    public Image showImage;

    public TMP_InputField nameInputField;
    public Button uploadButton;
    public Button purchaseButton;
    public Transform content;
    public GameObject itemPrefab;

    private int selectedCategory;
    private string selectedFilePath;
    private List<Item> displayedItems;
    private List<Room1> displayedRooms;
    private Item selectedItemForPurchase;
    private Room1 selectedRoomForPurchase;

    public GameObject menuBtn;
    public GameObject downPop;
    public GameObject downPop2;
    public GameObject upPop;

    public TextMeshProUGUI money;

    public GameObject chu;
    public GameObject chu2;
    public GameObject gumae;
    public GameObject gumae2;

    public TextMeshProUGUI itemName;
    public GameObject whatPop;
    public GameObject whatGumae;

    private void Start()
    {
        Debug.Log("Marketplace UI Start Initialized");

        uploadButton.onClick.AddListener(OpenFilePicker);
        purchaseButton.onClick.AddListener(PurchaseSelectedItem);
        whatPop.SetActive(false);
        whatGumae.SetActive(false);
        menuBtn.SetActive(true);
        downPop.SetActive(false);
        downPop2.SetActive(false);
        upPop.SetActive(false);
        chu.SetActive(false);
        chu2.SetActive(false);
        gumae.SetActive(false);
        gumae2.SetActive(false);
    }

    public void gumaehagi()
    {
        gumae.SetActive(true);
    }

    public void WhatsPopup()
    {
        whatPop.SetActive(true);
    }

    public void WhatsPopupClose()
    {
        whatPop.SetActive(false);
    }

    public void WhatsGumae()
    {
        whatGumae.SetActive(true);
    }
    public void WhatsGumaeClose()
    {
        whatGumae.SetActive(false);
    }


    public void OnCategoryButtonClicked()
    {
        StartCoroutine(GetAllItems(DisplayItems));
    }

    public void OpenDownloadPopUp()
    {
        Debug.Log("Opening Download Popup");
        whatGumae.SetActive(false);
        menuBtn.SetActive(false);
        downPop.SetActive(true);
        downPop2.SetActive(false);
    }
    public void OpenDownloadPopUp2()
    {
        Debug.Log("Opening Download Popup");
        whatGumae.SetActive(false);
        menuBtn.SetActive(false);
        downPop.SetActive(false);
        downPop2.SetActive(true);
    }

    public void OpenUploadPopUp()
    {
        Debug.Log("Opening Upload Popup");
        whatPop.SetActive(false);
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
        chu2.SetActive(false );
        gumae.SetActive(false);
        gumae2.SetActive(false);
    }

    public void Gumaebulga()
    {
        gumae2.SetActive(true);
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
            downPop2.SetActive(false);
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
        string description = "����";
        float price = float.Parse("20"); // ���� �Է� �ʵ� (���ڿ� -> ���� ��ȯ)
        int makerId = 0; // ���� ����� ID�� ������
        string category = "ī�װ�1"; // ���õ� ī�װ�
        string filePath = selectedFilePath;


        if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(category))
        {
            Debug.Log($"Uploading item: {itemName} in Category {category}");
            StartCoroutine(UploadItem(itemName, description, price, makerId, category, filePath));
            chu.SetActive(true); // ���ε� ���� UI Ȱ��ȭ
        }
        else
        {
            Debug.LogError("Upload failed: Please provide all required information.");
        }
    }
    public void OnUploadButtonClickRoom()
    {
        string roomId = "0";
        int price = int.Parse("20");
        int makerId = 0;

        if (!string.IsNullOrEmpty(roomId))
        {
            Debug.Log($"Uploading item: {itemName}");
            StartCoroutine(UploadRoom(roomId, price));
            whatPop.SetActive(false);
            chu2.SetActive(true); // ���ε� ���� UI Ȱ��ȭ
        }
        else
        {
            Debug.LogError("Upload failed: Please provide all required information.");
        }
    }
    private IEnumerator GetAllItems(System.Action<List<Item>> callback)
    {
        string url = $"{baseUrl}/item"; // ��ü �������� �������� API URL

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
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json); // ������ ��� �Ľ�
                callback(items); // ������ ����� �ݹ����� ����
            }
        }
    }
    private IEnumerator GetAllRooms(System.Action<List<Room1>> callback)
    {
        string url = $"{baseUrl}/room"; // ��ü �������� �������� API URL

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
                List<Room1> rooms = JsonConvert.DeserializeObject<List<Room1>>(json); // ������ ��� �Ľ�
                callback(rooms); // ������ ����� �ݹ����� ����
            }
        }
    }
    private void DisplayItems(List<Item> items)
    {
        Debug.Log($"Displaying {items.Count} items");

        // 1. ������ ����Ʈ�� null���� Ȯ��
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("No items to display.");
            return;
        }

        displayedItems = items;

        // 2. ������ ������ ���� �������� ��� ����
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 3. ������ ���� �� UI�� �߰�
        foreach (Item item in items)
        {
            if (item == null) continue;  // �������� null�� ��� �ǳʶٱ�

            GameObject itemObj = Instantiate(itemPrefab, content);
            if (itemObj == null) continue; // ������ ��ü ������ ������ ��� �ǳʶٱ�


            if (itemName != null)
            {
                itemName.text = item.name;
            }
            else
            {
                Debug.LogError("Item name UI element not found.");
            }

            // 5. ������ Ŭ�� �� �����ϵ��� �̺�Ʈ �߰�
            Button itemButton = itemObj.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => SelectItemForPurchase(item));
            }
            else
            {
                Debug.LogError("Button component not found in item prefab.");
            }

            // 6. �̹��� �ε� (UI ��Ұ� null���� üũ)
            Image itemImage = itemObj.transform.Find("ItemImage")?.GetComponent<Image>();
            if (itemImage != null)
            {
                // StartCoroutine(LoadImage(item.imageUrl, itemImage));
            }
            else
            {
                Debug.Log("Image component not found in item prefab.");
            }

            Debug.Log($"Item Added: {item.name}");
        }
    }

    private void DisplayRooms(List<Room1> rooms)
    {
        Debug.Log($"Displaying {rooms.Count} items");

        // 1. ������ ����Ʈ�� null���� Ȯ��
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogWarning("No items to display.");
            return;
        }

        displayedRooms = rooms;

        // 2. ������ ������ ���� �������� ��� ����
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 3. ������ ���� �� UI�� �߰�
        foreach (Room1 room in rooms)
        {
            if (room == null) continue;  // �������� null�� ��� �ǳʶٱ�

            GameObject itemObj = Instantiate(itemPrefab, content);
            if (itemObj == null) continue; // ������ ��ü ������ ������ ��� �ǳʶٱ�

            // 5. ������ Ŭ�� �� �����ϵ��� �̺�Ʈ �߰�
            Button itemButton = itemObj.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => SelectRoomForPurchase(room));
            }
            else
            {
                Debug.LogError("Button component not found in item prefab.");
            }

            // 6. �̹��� �ε� (UI ��Ұ� null���� üũ)
            Image itemImage = itemObj.transform.Find("ItemImage")?.GetComponent<Image>();
            if (itemImage != null)
            {
                // StartCoroutine(LoadImage(item.imageUrl, itemImage));
            }
            else
            {
                Debug.Log("Image component not found in item prefab.");
            }

            Debug.Log($"Item Added: {room.roomId}");
        }
    }

    public void SelectItemForPurchase(Item item)
    {
        Debug.Log($"Item Selected for Purchase: {item.name} (ID: {item.itemId})");
        selectedItemForPurchase = item;
        itemName.text = item.name;
    }
    public void SelectRoomForPurchase(Room1 room)
    {
        Debug.Log($"Item Selected for Purchase: {room.roomId} (ID: {room.roomId})");
        selectedRoomForPurchase = room;
    }

    public void PurchaseSelectedItem()
    {
        if (selectedItemForPurchase != null)
        {
            Debug.Log($"Attempting to purchase: {selectedItemForPurchase.name} (ID: {selectedItemForPurchase.itemId})");
            StartCoroutine(PurchaseItem(0, 0, success =>
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

    public void PurchaseSelectedRoom()
    {
        if (selectedRoomForPurchase != null)
        {
            Debug.Log($"Attempting to purchase: {selectedRoomForPurchase.roomId} (ID: {selectedRoomForPurchase.roomId})");
            StartCoroutine(PurchaseRoom(selectedRoomForPurchase.roomId, 0, success =>
            {
                if (success)
                {
                    Debug.Log($"Purchase successful for {selectedRoomForPurchase.roomId}");
                    StartCoroutine(GetRoomsByCategory(DisplayRooms));
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
    private IEnumerator UploadRoom(string id, int price)
    {
        Debug.Log($"Starting Upload for Room ID {id} with Price {price}");
        string url = $"{baseUrl}/room";

        // JSON ������ ����
        UploadRoomRequest jsonData = new UploadRoomRequest
        {
            id = id,
            price = price,
            furnitureList = new List<Furniture>
        {
            new Furniture
            {
                size = 10,
                spriteNumber = 1,
                positionX = 0,
                positionY = 0,
                rotationY = 0
            }
        },
            uploadedAt = System.DateTime.UtcNow.ToString("o") // ISO 8601 ����
        };

        // JSON ����ȭ
        string jsonString = JsonUtility.ToJson(jsonData);
        Debug.Log($"Request JSON: {jsonString}");

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            // JSON ���ڿ��� UTF8�� ����Ʈ �迭�� ��ȯ�Ͽ� ����
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // ��û ������
            Debug.Log($"Request JSON: {jsonString}");
            yield return www.SendWebRequest();

            // ���� Ȯ��
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

    private IEnumerator UploadItem(string itemName, string description, float price, int makerId, string category, string filePath)
    {
        Debug.Log($"Starting Upload for {itemName} in Category {category}");
        string url = $"{baseUrl}/item";

        // JSON ������ ����
        UploadItemRequest jsonData = new UploadItemRequest
        {
            makerId = makerId,
            name = itemName,
            description = description,
            price = price,
            category = category
        };

        // JSON ����ȭ
        string jsonString = JsonUtility.ToJson(jsonData);
        Debug.Log($"Request JSON: {jsonString}");

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            // JSON ���ڿ��� UTF8�� ����Ʈ �迭�� ��ȯ�Ͽ� ����
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // ��û ������
            Debug.Log($"Request JSON: {jsonString}");
            yield return www.SendWebRequest();

            // ���� Ȯ��
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

    // ��Ƽ��Ʈ �� ������ ���� �Լ�
    private byte[] CreateMultipartFormData(string json, byte[] fileData, string filePath, string boundary)
    {
        var formData = new List<byte>();

        // JSON ������ �߰�
        string jsonPart = $"--{boundary}\r\n" +
                          "Content-Disposition: form-data; name=\"metadata\"\r\n\r\n" +
                          $"{json}\r\n";
        formData.AddRange(Encoding.UTF8.GetBytes(jsonPart));

        // ���� ������ �߰�
        string fileHeader = $"--{boundary}\r\n" +
                            $"Content-Disposition: form-data; name=\"file\"; filename=\"{Path.GetFileName(filePath)}\"\r\n" +
                            "Content-Type: image/png\r\n\r\n";
        formData.AddRange(Encoding.UTF8.GetBytes(fileHeader));
        formData.AddRange(fileData);
        formData.AddRange(Encoding.UTF8.GetBytes("\r\n"));

        // �� ��� �߰�
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
    private IEnumerator GetRoomsByCategory(System.Action<List<Room1>> callback)
    {
        string url = $"{baseUrl}/room";

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
                List<Room1> rooms = JsonConvert.DeserializeObject<List<Room1>>(json);
                callback(rooms);
            }
        }
    }
    private IEnumerator PurchaseItem(int itemId, int userId, System.Action<bool> callback)
    {
        Debug.Log($"Attempting purchase for Item ID: {0}");
        string url = $"{baseUrl}/trade/item/{0}"; // ���� API URL

        // ��û �����͸� JSON���� ����ȭ
        string jsonData = JsonConvert.SerializeObject(new { userId = 0 });

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Purchase failed: {www.error}");
                callback(false);
            }
            else
            {
                try
                {
                    // �������� ��ȯ�� JSON
                    string json = www.downloadHandler.text;

                    // JSON�� ���� ��ü�� ������ȭ
                    Item purchasedItem = JsonConvert.DeserializeObject<Item>(json);

                    if (purchasedItem != null)
                    {
                        // DontDestroyOnLoad ������Ʈ�� �߰�
                        ItemManager.Instance.AddItems(new List<Item> { purchasedItem });

                        Debug.Log($"Purchase successful: {purchasedItem.name}");
                        callback(true);
                    }
                    else
                    {
                        Debug.LogError("Purchase response is invalid.");
                        callback(false);
                    }
                }
                catch (JsonSerializationException ex)
                {
                    Debug.LogError($"JSON Deserialization Error: {ex.Message}");
                    callback(false);
                }
            }
        }
    }
    private IEnumerator PurchaseRoom(string roomId, int userId, System.Action<bool> callback)
    {
        Debug.Log($"Attempting purchase for Room ID: {roomId}");
        string url = $"{baseUrl}/trade/room/{roomId}"; // ���� API URL
        string jsonData = JsonConvert.SerializeObject(new { userId = userId });

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Purchase failed: {www.error}");
                callback(false);
            }
            else
            {
                try
                {
                    // �������� ��ȯ�� JSON
                    string json = www.downloadHandler.text;

                    // JSON�� ���� ��ü�� ������ȭ
                    Room1 purchasedRoom = JsonConvert.DeserializeObject<Room1>(json);

                    if (purchasedRoom != null)
                    {
                        // DontDestroyOnLoad ������Ʈ�� �߰�
                        RoomManager.Instance.AddRooms(new List<Room1> { purchasedRoom });

                        Debug.Log($"Purchase successful: {purchasedRoom.roomId}");
                        callback(true);
                    }
                    else
                    {
                        Debug.LogError("Purchase response is invalid.");
                        callback(false);
                    }
                }
                catch (JsonSerializationException ex)
                {
                    Debug.LogError($"JSON Deserialization Error: {ex.Message}");
                    callback(false);
                }
            }
        }
    }

}

public class Item
{
    public string itemId { get; set; }
    public int makerId { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string category { get; set; }
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

[System.Serializable]
public class UploadRoomRequest
{
    public string id; // roomId -> id�� ����
    public int makerId; // �߰�
    public int price;
    public List<Furniture> furnitureList; // �߰�
    public string uploadedAt; // �߰�
}
public class Room1
{
    public string roomId { get; set; }
    public int price { get; set; }
}
