using System.Collections;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public enum CatPersonality { Friendly, Picky }
    public CatPersonality catPersonality = CatPersonality.Friendly; // �⺻ ������ Friendly

    //����� Ŭ�� �׽�Ʈ
    public AudioClip pinkyAudio;  // ����� ����� Ŭ��
    private AudioSource audioSource;
    public float playDuration = 5f; // ����� �ð�(��)

    private void Awake()
    {
        //����� Ŭ�� �׽�Ʈ
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //����� Ŭ�� �׽�Ʈ
        audioSource.clip = pinkyAudio;
        audioSource.Play();
        // �ڷ�ƾ�� ����Ͽ� 4�ʸ��� ������� ���
        StartCoroutine(PlayAudioRepeatedly());
    }
    void Update()
    {
        
        // Ű���� �Է����� ����� ���� ����
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
            Debug.Log("����̰� ��� ��ȣ�ۿ��� ���������� �����մϴ�.");
        }
        else if (personality == CatPersonality.Picky)
        {
            Debug.Log("����̰� Ư�� ��ȣ�ۿ뿡 �ΰ��մϴ�.");
        }
    }

    void StopAudio()
    {
        audioSource.Stop();  // ������� ����
    }

    //����� Ŭ�� �׽�Ʈ
    IEnumerator PlayAudioRepeatedly()
    {
        while (true)
        {
            audioSource.Play();  // ����� Ŭ�� ���
            yield return new WaitForSeconds(4f);  // 4�� ���
        }
    }
}