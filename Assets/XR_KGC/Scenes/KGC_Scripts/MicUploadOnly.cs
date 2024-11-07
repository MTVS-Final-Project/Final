using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MicUploadOnly : MonoBehaviour
{
    private AudioClip _audioClip;
    private string _microphone;

    public GameObject recodingText;

    public bool clicked = false;

    // 서버 URL
    public string uploadURL = "https://your-server-url.com/upload"; // 실제 서버의 URL로 변경하세요.

    public void RecordStart()
    {
        if (!clicked)
        {
            clicked = true;
            StartRecording();
        }
        else
        {
            clicked = false;
            StopRecordingAndSave();
        }
    }

    void Start()
    {
        // 첫 번째 마이크 장치 선택
        if (Microphone.devices.Length > 0)
        {
            _microphone = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("마이크 장치가 없습니다.");
        }
    }

    public void StartRecording()
    {
        if (_microphone != null)
        {
            recodingText.SetActive(true);
            _audioClip = Microphone.Start(_microphone, true, 10, 44100);
            Debug.Log("녹음 시작");
        }
    }

    public void StopRecordingAndSave()
    {
        if (_microphone != null && Microphone.IsRecording(_microphone))
        {
            recodingText.SetActive(false);
            Microphone.End(_microphone);

            // .wav 파일로 저장
            SaveRecording(_audioClip);
            Debug.Log("녹음 종료 및 저장");

            // 서버에 업로드
            UploadFile();
        }
    }

    private void SaveRecording(AudioClip clip)
    {
        // SavWav 클래스를 이용하여 .wav 파일로 저장
        SavWav.Save("MyRecording", clip);
    }

    // 파일 업로드를 호출하는 함수
    public void UploadFile()
    {
        StartCoroutine(Upload());
    }

    private IEnumerator Upload()
    {
        // 파일 경로
        string filePath = Path.Combine(Application.persistentDataPath, "MyRecording.wav");

        // 파일을 바이트 배열로 읽기
        byte[] fileData = File.ReadAllBytes(filePath);

        // WWWForm을 사용하여 폼 데이터로 파일 추가
        WWWForm form = new WWWForm();
        form.AddBinaryData("file2", fileData, "MyRecording.wav", "audio/wav");

        // UnityWebRequest 생성
        UnityWebRequest www = UnityWebRequest.Post(uploadURL, form);

        // 요청 전송
        yield return www.SendWebRequest();

        // 응답 처리 (텍스트 없이 성공 여부만 로그로 확인)
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("파일 업로드 성공");
        }
        else
        {
            Debug.Log("파일 업로드 실패: " + www.error);
        }
    }
}
