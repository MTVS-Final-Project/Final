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
        // GPS 기반의 씬으로 가게끔 부탁합니다.
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
        // 만약 inputField의 ID 와 PW가 서버에 존재한다면, 로그인한다.
        // 그렇지 않다면, loginPop을 활성화한다.
    }

    public void Four_CreateName()
    {
        // create_name.text
        // create_name.text 의 텍스트가 서버에 존재한다면, GameObject Pop을 활성화한다.
        // 그렇지 않다면, GameObject Check을 활성화한다.
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

    // 각종 팝업에 대한 스크립트 작성되지 않았음
}
