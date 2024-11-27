using System.Collections;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public enum CatPersonality { Friendly, Picky }
    public CatPersonality catPersonality = CatPersonality.Friendly; // 기본 성격은 Friendly

    //오디오 클립 테스트
    public AudioClip pinkyAudio;  // 재생할 오디오 클립
    private AudioSource audioSource;
    public float playDuration = 5f; // 재생할 시간(초)

    private void Awake()
    {
        //오디오 클립 테스트
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //오디오 클립 테스트
        audioSource.clip = pinkyAudio;
        audioSource.Play();
        // 코루틴을 사용하여 4초마다 오디오를 재생
        StartCoroutine(PlayAudioRepeatedly());
    }
    void Update()
    {
        
        // 키보드 입력으로 고양이 성격 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCatPersonality(CatPersonality.Friendly);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetCatPersonality(CatPersonality.Picky);
        }
    }

    private void SetCatPersonality(CatPersonality personality)
    {
        catPersonality = personality;

        if (personality == CatPersonality.Friendly)
        {
            Debug.Log("고양이가 모든 상호작용을 긍정적으로 반응합니다.");
        }
        else if (personality == CatPersonality.Picky)
        {
            Debug.Log("고양이가 특정 상호작용에 민감합니다.");
        }
    }

    void StopAudio()
    {
        audioSource.Stop();  // 오디오를 멈춤
    }

    //오디오 클립 테스트
    IEnumerator PlayAudioRepeatedly()
    {
        while (true)
        {
            audioSource.Play();  // 오디오 클립 재생
            yield return new WaitForSeconds(4f);  // 4초 대기
        }
    }
}