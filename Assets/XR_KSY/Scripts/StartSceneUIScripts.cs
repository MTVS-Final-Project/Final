using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using TMPro;

public class StartSceneUIScripts : MonoBehaviour
{
    public Image touchToScreen;

    public GameObject oneStep;
    public GameObject twoStep;
    public GameObject threeStep;
    public GameObject fourStep_SignIN;
    public GameObject fourStep_Create;

    public TMP_InputField login_ID;
    public TMP_InputField login_PW;
    public TMP_InputField create_name;
    public TMP_InputField create_email;
    public TMP_InputField create_code;
    public TMP_InputField create_id;
    public TMP_InputField create_pw;

    public GameObject loginPop;

    void Start()
    {
        oneStep.SetActive(false);
        twoStep.SetActive(false);
        threeStep.SetActive(false);
        fourStep_Create.SetActive(false);
        fourStep_SignIN.SetActive(false);
    }

    public void StartScreen()
    {
        touchToScreen.enabled = false;
        oneStep.SetActive(true);
    }

    public void Agree()
    {
        twoStep.SetActive(true);
        oneStep.SetActive(false);
    }

    public void Create()
    {
        fourStep_Create.SetActive(true);
        twoStep.SetActive(false) ;
    }

    public void Guest()
    {
        SceneManager.LoadScene(1);
        // GPS ����� ������ ���Բ� ��Ź�մϴ�.
    }

    public void LogIN()
    {
        threeStep.SetActive(true) ;
        twoStep.SetActive(false) ;
    }

    public void Naver()
    {

    }

    public void Kakao()
    {

    }

    public void Google()
    {

    }

    public void Nyang()
    {
        fourStep_SignIN.SetActive(true);
        threeStep.SetActive(false) ;
    }

    public void Four_LogIN()
    {
        // login_ID.text
        // login_PW.text
        // ���� inputField�� ID �� PW�� ������ �����Ѵٸ�, �α����Ѵ�.
        // �׷��� �ʴٸ�, loginPop�� Ȱ��ȭ�Ѵ�.
    }

    public void Four_CreateName()
    {
        // create_name.text
        // create_name.text �� �ؽ�Ʈ�� ������ �����Ѵٸ�, GameObject Pop�� Ȱ��ȭ�Ѵ�.
        // �׷��� �ʴٸ�, GameObject Check�� Ȱ��ȭ�Ѵ�.
    }

    public void Four_CreateGetCode()
    {

    }

    public void Four_CreateSetCode()
    {

    }

    public void Four_CreateID()
    {

    }

    public void Four_CreateAccount()
    {

    }

    // ���� �˾��� ���� ��ũ��Ʈ �ۼ����� �ʾ���
}
