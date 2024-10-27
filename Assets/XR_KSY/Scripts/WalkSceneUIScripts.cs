using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using TMPro;
using UnityEngine.UIElements;

public class WalkSceneUIScripts : MonoBehaviour
{
    public GameObject set;

    public GameObject gongji;
    public GameObject setPop;
    public GameObject yakgwan;

    public GameObject soundSet;
    public GameObject alarmSet;
    public GameObject arSet;
    public GameObject languageSet;
    void Start()
    {
        
    }

    public void Btn_home()
    {
        // 씬이 Home 으로 , 마이홈이 보이게 부탁합니다.
    }

    public void Nyang()
    {
        SceneManager.LoadScene(2);
        // 씬이 Home 으로 , Town을 활성화하게 부탁합니다.
    }

    public void Btn_setting()
    {
        set.SetActive(true);
    }

    public void Btn_settingClose()
    {
        set.SetActive(false);
    }
    public void Set_gongji()
    {
        gongji.SetActive(true);
    }

    public void Set_gongjiClose()
    {
        gongji.SetActive(false);
    }

    public void Set_set()
    {
        setPop.SetActive(true);
    }
    public void Set_setClose()
    {
        setPop.SetActive(false);
    }

    public void Set_yakgwan()
    {
        yakgwan.SetActive(true);
    }
    public void Set_yakgwanClose()
    {
        yakgwan.SetActive(false);
    }

    public void Set_sound()
    {
        soundSet.SetActive(true);
    }
    public void Set_soundClose()
    {
        soundSet.SetActive(false);
    }

    public void Set_alarm()
    {
        alarmSet.SetActive(true);
    }
    public void Set_alarmClose()
    {
        alarmSet.SetActive(false);
    }
    public void Set_ar()
    {
        arSet.SetActive(true);
    }
    public void Set_arClose()
    {
        arSet.SetActive(false);
    }

    public void Set_lang()
    {
        languageSet.SetActive(true);
    }
    public void Set_langClose()
    {
        languageSet.SetActive(false);
    }

    public void Set_account()
    {

    }
    public void Set_accountClose()
    {

    }

    public void Set_logout()
    {

    }

    public void Set_delete()
    {

    }
}
