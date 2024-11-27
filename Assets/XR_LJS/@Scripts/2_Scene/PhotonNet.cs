// PhotonNet.cs
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Net;
using ExitGames.Client.Photon;
using Spine.Unity;

public class PhotonNet : MonoBehaviourPunCallbacks
{
    public Transform catTransform;
    public Transform cicleTransform;
    void Start()
    {
        if (catTransform != null)
        {
            Vector3 spawnPosition = GetRandomPositionNearCat();
            string myPrefabName = PlayerPrefs.GetInt("selectedCharacter") == 1 ? "SKPlayer" : "SKPlayer2";
            GameObject playerInstance = PhotonNetwork.Instantiate(myPrefabName, spawnPosition, Quaternion.identity);
            playerInstance.name = "Player";
            CatController.instance.player = playerInstance;
        }


        if (cicleTransform != null)
        {
            Vector3 spawnPosition = GetRandomPositionNearCircle();
            string myPrefabName = PlayerPrefs.GetInt("selectedCharacter") == 1 ? "SKPlayer" : "SKPlayer2";
            GameObject playerInstance = PhotonNetwork.Instantiate(myPrefabName, spawnPosition, Quaternion.identity);
            playerInstance.name = "Player";
            //CatController.instance.player = playerInstance;
        }


    }
    Vector3 GetRandomPositionNearCat()
    {
        float offsetX = Random.Range(-0.1f, 0.1f);
        float offsetY = Random.Range(-0.1f, 0.1f);
        return catTransform != null ? catTransform.position + new Vector3(offsetX, offsetY, 0) : Vector3.zero;
    }
    Vector3 GetRandomPositionNearCircle()
    {
        float offX = Random.Range(-0.1f, 0.1f);
        float offY = Random.Range(-0.1f, 0.1f);
        return cicleTransform != null ? cicleTransform.position + new Vector3(offX, offY, 0) : Vector3.zero;
    }
}