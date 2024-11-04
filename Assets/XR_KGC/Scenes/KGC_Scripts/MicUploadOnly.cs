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

    // ���� URL
    public string uploadURL = "https://your-server-url.com/upload"; // ���� ������ URL�� �����ϼ���.

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
        // ù ��° ����ũ ��ġ ����
        if (Microphone.devices.Length > 0)
        {
            _microphone = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("����ũ ��ġ�� �����ϴ�.");
        }
    }

    public void StartRecording()
    {
        if (_microphone != null)
        {
            recodingText.SetActive(true);
            _audioClip = Microphone.Start(_microphone, true, 10, 44100);
            Debug.Log("���� ����");
        }
    }

    public void StopRecordingAndSave()
    {
        if (_microphone != null && Microphone.IsRecording(_microphone))
        {
            recodingText.SetActive(false);
            Microphone.End(_microphone);

            // .wav ���Ϸ� ����
            SaveRecording(_audioClip);
            Debug.Log("���� ���� �� ����");

            // ������ ���ε�
            UploadFile();
        }
    }

    private void SaveRecording(AudioClip clip)
    {
        // SavWav Ŭ������ �̿��Ͽ� .wav ���Ϸ� ����
        SavWav.Save("MyRecording", clip);
    }

    // ���� ���ε带 ȣ���ϴ� �Լ�
    public void UploadFile()
    {
        StartCoroutine(Upload());
    }

    private IEnumerator Upload()
    {
        // ���� ���
        string filePath = Path.Combine(Application.persistentDataPath, "MyRecording.wav");

        // ������ ����Ʈ �迭�� �б�
        byte[] fileData = File.ReadAllBytes(filePath);

        // WWWForm�� ����Ͽ� �� �����ͷ� ���� �߰�
        WWWForm form = new WWWForm();
        form.AddBinaryData("file2", fileData, "MyRecording.wav", "audio/wav");

        // UnityWebRequest ����
        UnityWebRequest www = UnityWebRequest.Post(uploadURL, form);

        // ��û ����
        yield return www.SendWebRequest();

        // ���� ó�� (�ؽ�Ʈ ���� ���� ���θ� �α׷� Ȯ��)
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("���� ���ε� ����");
        }
        else
        {
            Debug.Log("���� ���ε� ����: " + www.error);
        }
    }
}
