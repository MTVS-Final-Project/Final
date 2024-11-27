// PhotonNet.cs
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Net;
using ExitGames.Client.Photon;
using Spine.Unity;
using UnityEngine.Audio;

public class PhotonNet : MonoBehaviourPunCallbacks
{
    public Transform catTransform;
    public Transform cicleTransform;

    public AudioClip backgroundMusicClip;  // 배경음악으로 사용할 AudioClip
    private AudioSource audioSource;

    private void Awake()
    {
        
    }
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
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        // AudioClip을 AudioSource에 할당
        audioSource.clip = backgroundMusicClip;

        // 배경음악을 반복 재생하도록 설정
        audioSource.loop = true;

        // 배경음악 재생
        audioSource.Play();
        


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