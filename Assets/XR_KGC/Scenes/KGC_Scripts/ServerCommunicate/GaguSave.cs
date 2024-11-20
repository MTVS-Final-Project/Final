using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class GaguSave : MonoBehaviour
{
    public GameObject gaguParent;
    public ItemScript itemS;

    // 가구 기능 들어가있는 프리팹들
    public GameObject[] gagu = new GameObject[5];
    // 가구 외형 정하는 스프라이트
    public List<string> itemName = new List<string>(); // 가구이름 저장 불러오기만 하면 이건 필요없는거같음.
    public List<Sprite> sprites = new List<Sprite>();
   // public List<GaguData> gaguDataList = new List<GaguData>();

    // GaguDataList는 TempManager에서 관리합니다.
    public List<GaguData> gaguDataList => TempManager.Instance.gaguDataList;



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
        public List<GaguData> gaguDataList;

        public GaguDataListWrapper(List<GaguData> dataList)
        {
            gaguDataList = dataList;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gaguDataList!=null&& gaguParent.transform.childCount == 0)
        {
           StartCoroutine(LoadGagu());
        }
    }

    // Update is called once per frame
    void Update()
    {
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
            StartCoroutine(LoadGagu());
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartCoroutine(SendDataToServer());
        }
    }

    public IEnumerator LoadGagu()
    {
        for (int i = 0; i < gaguDataList.Count; i++)
        {
            GaguData data = gaguDataList[i];
            GameObject go = Instantiate(gagu[data.size], new Vector3(data.xpos, data.ypos, 0), Quaternion.Euler(0, data.rotY, 0));

            go.GetComponent<KeepOnGround>().enabled = false;
            go.GetComponent<DragObject>().enabled = false;
            go.transform.SetParent(gaguParent.transform, false);
            go.transform.position = new Vector3(data.xpos, data.ypos, 0);

            go.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprites[data.spriteNum];

            Debug.Log($"Object {go.name} instantiated at position {go.transform.position}");
        }
        yield return new WaitForSeconds(0.1f);

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
        gaguDataList.Clear();

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
                gaguDataList.Add(new GaguData(1, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
            else if (child.name.Contains("Square"))
            {
                gaguDataList.Add(new GaguData(0, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
            else if (child.name.Contains("Tower"))
            {
                gaguDataList.Add(new GaguData(2, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
            else if (child.name.Contains("Toilet"))
            {
                gaguDataList.Add(new GaguData(3, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
            else if (child.name.Contains("Dish"))
            {
                gaguDataList.Add(new GaguData(4, spriteIndex, child.transform.position.x, child.transform.position.y, child.transform.eulerAngles.y));
            }
        }

        Debug.Log("Number of items in gaguDataList: " + gaguDataList.Count);
    }

    public IEnumerator SendDataToServer()
    {
        GaguDataListWrapper dataWrapper = new GaguDataListWrapper(gaguDataList);
        string jsonData = JsonUtility.ToJson(dataWrapper);
        Debug.Log("Sending JSON Data: " + jsonData); // JSON 데이터 로그 출력
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest("https://yourserver.com/api/saveGaguData", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data successfully sent to server: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to send data: " + request.error);
        }
    }

    // 나중에 정보들 저장하고 불러오는 스크립트 넣어야됨
}
