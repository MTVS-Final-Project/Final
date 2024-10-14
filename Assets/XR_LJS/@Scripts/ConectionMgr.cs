using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class ConectionMgr : MonoBehaviourPunCallbacks
{
    public Text loadingText;
    private const string FIRST_SCENE_NAME = "FirstScene_LJS";
    private const string SECOND_SCENE_NAME = "SecondScene_LJS";

    // DontDestroyOnLoad 객체들을 추적하기 위한 리스트
    private static List<GameObject> dontDestroyObjects = new List<GameObject>();

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        loadingText.text = "마스터 서버 접속...";

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // DontDestroyOnLoad로 설정된 객체를 추적 리스트에 추가하는 메서드
    public static void AddDontDestroyOnLoadObject(GameObject obj)
    {
        if (!dontDestroyObjects.Contains(obj))
        {
            dontDestroyObjects.Add(obj);
            DontDestroyOnLoad(obj);
        }
    }

    public void LoadFirstScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DestroyDontDestroyOnLoadObjects();
            PhotonNetwork.LoadLevel(FIRST_SCENE_NAME);
        }
    }

    public void LoadSecondScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DestroyDontDestroyOnLoadObjects();
            PhotonNetwork.LoadLevel(SECOND_SCENE_NAME);
        }
    }


    private void DestroyDontDestroyOnLoadObjects()
    {
        Debug.Log($"Found {dontDestroyObjects.Count} DontDestroyOnLoad objects.");

        for (int i = dontDestroyObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = dontDestroyObjects[i];
            if (obj != null && obj != this.gameObject)
            {
                string objName = obj.name;
                PhotonView photonView = PhotonView.Get(obj);
                if (photonView != null && photonView.IsMine)
                {
                    PhotonNetwork.Destroy(obj);
                    Debug.Log($"Destroyed Photon object: {objName}");
                }
                else if (photonView == null)
                {
                    Destroy(obj);
                    Debug.Log($"Destroyed regular object: {objName}");
                }
                else
                {
                    Debug.Log($"Cannot destroy non-owned Photon object: {objName}");
                }
                dontDestroyObjects.RemoveAt(i);
            }
        }
    }
}