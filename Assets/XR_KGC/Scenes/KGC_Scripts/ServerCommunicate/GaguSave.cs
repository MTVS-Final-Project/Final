using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class GaguSave : MonoBehaviour
{
    public GameObject gaguParent;
    public ItemScript itemS;

    public GameObject roomModi;

    // 가구 기능 들어가있는 프리팹들
    public GameObject[] gagu = new GameObject[5];
    // 가구 외형 정하는 스프라이트
    public List<string> itemName = new List<string>(); // 가구이름 저장 불러오기만 하면 이건 필요없는거같음.
    public List<Sprite> sprites = new List<Sprite>();
   // public List<GaguData> gaguDataList = new List<GaguData>();

    // GaguDataList는 TempManager에서 관리합니다.
    public List<GaguData> furnitureList => TempManager.Instance.furnitureList;

    private string receiveDataUrl = "http://13.124.6.53:8080/api/v1/room"; // 데이터를 받아올 서버의 URL



    [System.Serializable]
    public class GaguData
    {
        public int size;
        public int spriteNum;
        public float xpos;
        public float ypos;
        public float rotY;

        public GaguData(int size, int spriteNum, float xpos, float ypos, float rotY)
        {
            this.size = size;
            this.spriteNum = spriteNum;
            this.xpos = xpos;
            this.ypos = ypos;
            this.rotY = rotY;
        }
    }

    [System.Serializable]
    public class GaguDataListWrapper
    {
        public int makerId;
        public List<GaguData> furnitureList;

        public GaguDataListWrapper(int makerId, List<GaguData> dataList)
        {
            this.makerId = makerId;
            furnitureList = dataList;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(LoadGagu());
        if (furnitureList != null && gaguParent.transform.childCount == 0)
        {
            StartCoroutine(LoadGagu());
            //StartCoroutine(ReceiveDataFromServer()); // 서버에서 데이터를 받아옵니다.
        }
    }

    // Update is called once per frame
    void Update()
    {
        print("작동중");
        if (itemS == null)
        {
            itemS = GameObject.Find("GaguCanvas").GetComponentInChildren<ItemScript>();
        }
        if (gaguParent == null)
        {
            gaguParent = GameObject.Find("GaguParent");
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            FindChildrenWithKeyword(gaguParent);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            print("9는 눌렸다");
            StartCoroutine(LoadGagu());
        }
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    StartCoroutine(SendDataToServer());
        //}
    }

    public IEnumerator ReceiveDataFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(receiveDataUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data successfully received from server: " + request.downloadHandler.text);

            // 받은 JSON 데이터를 파싱하여 furnitureList에 저장합니다.
            GaguDataListWrapper receivedData = JsonUtility.FromJson<GaguDataListWrapper>(request.downloadHandler.text);
            furnitureList.Clear();
            furnitureList.AddRange(receivedData.furnitureList);

            // 데이터를 로드하여 가구들을 배치합니다.
            StartCoroutine(LoadGagu());
        }
        else
        {
            Debug.LogError("Failed to receive data: " + request.error);
        }
    }

    public IEnumerator LoadGagu()
    {
        roomModi.SetActive(true);
        for (int i = 0; i < furnitureList.Count; i++)
        {
            GaguData data = furnitureList[i];
            GameObject go = Instantiate(gagu[data.size], new Vector3(data.xpos, data.ypos, 0), Quaternion.Euler(0, data.rotY, 0));

            go.GetComponent<KeepOnGround>().enabled = false;
            go.GetComponent<DragObject>().enabled = false;
            go.transform.SetParent(gaguParent.transform, false);
            go.transform.position = new Vector3(data.xpos, data.ypos, 0);

            go.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprites[data.spriteNum];

            Debug.Log($"Object {go.name} instantiated at position {go.transform.position}");
        }
        yield return new WaitForSeconds(0.1f);
        yield return null;
        roomModi.SetActive(false);

        foreach (Transform child in gaguParent.transform)
        {
            child.GetComponent<KeepOnGround>().enabled = true;
        }
    }

    public void SaveGagu()
    {
        FindChildrenWithKeyword(gaguParent);

    }

    void FindChildrenWithKeyword(GameObject parent)
    {
        furnitureList.Clear();

        foreach (Transform child in parent.transform)
        {
            int spriteIndex = -1;
            for (int i = 0; i < itemS.sprites.Count; i++)
            {
                if (child.transform.GetChild(1).GetComponentInChildren<SpriteRenderer>().sprite.name == itemS.sprites[i].name)
                {
                    spriteIndex = i;
                    break;
                }
            }

            if (spriteIndex == -1)
            {
                continue;
            }

            if (child.name.Contains("Bed"))
            {
                furnitureList.Add(new GaguData(1, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
            else if (child.name.Contains("Square"))
            {
                furnitureList.Add(new GaguData(0, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
            else if (child.name.Contains("Tower"))
            {
                furnitureList.Add(new GaguData(2, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
            else if (child.name.Contains("Toilet"))
            {
                furnitureList.Add(new GaguData(3, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
            else if (child.name.Contains("Dish"))
            {
                furnitureList.Add(new GaguData(4, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
        }

        Debug.Log("Number of items in gaguDataList: " + furnitureList.Count);
    }

  
}
