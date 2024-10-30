using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using TMPro;
using UnityEngine.UIElements;

public class HomeSceneUIScripts : MonoBehaviour
{
    public GameObject underImage;
    public GameObject menuBtn;

    public GameObject shop;
    public GameObject town;
    public GameObject cash;
    public GameObject set;
    public GameObject profile;
    public GameObject friend;
    public GameObject quest;
    public GameObject events;
    public GameObject commu;
    public GameObject money;

    public ScrollView normal;
    public ScrollView special;

    public GameObject castle;
    public GameObject market;
    public GameObject gwangjang;
    public GameObject hospital;
    public GameObject homePop;

    public GameObject gongji;
    public GameObject setPop;
    public GameObject yakgwan;

    public GameObject soundSet;
    public GameObject alarmSet;
    public GameObject arSet;
    public GameObject languageSet;

    public GameObject questPop;
    public GameObject userQuestPop;
    public GameObject suju;

    void Start()
    {
        shop.SetActive(false);
        town.SetActive(true);
        set.SetActive(false);
        profile.SetActive(false);
        friend.SetActive(false);
        quest.SetActive(false);
        events.SetActive(false);
        commu.SetActive(false);
        underImage.SetActive(false);
        menuBtn.SetActive(false);
    }

    public void Btn_Menu()
    {
        underImage.SetActive(true);
        menuBtn.SetActive(false);
    }

    public void Menu_CloseBtn()
    {
        underImage.SetActive(false);
        menuBtn.SetActive(true);
    }

    public void Btn_profile()
    {
        profile.SetActive(true);
    }
    public void Btn_profileClose()
    {
        profile.SetActive(false);
    }

    public void Btn_friend()
    {
        friend.SetActive(true);
    }
    public void Btn_friendClose()
    {
        friend.SetActive(false );
    }

    public void Btn_quest()
    {
        quest.SetActive(true);
    }
    public void Btn_questClose()
    {
        quest.SetActive(false) ;
    }

    public void Btn_town()
    {
        town.SetActive(true);
        homePop.SetActive(false ) ;
    }
    public void Btn_townClose()
    {
        town.SetActive(false ) ;
        underImage.SetActive(false) ;
        menuBtn.SetActive(true) ;
    }

    public void Btn_event()
    {
        events.SetActive(true);
    }
    public void Btn_eventClose()
    {
        events.SetActive(false);
    }

    public void Btn_money()
    {
        money.SetActive(true);
    }
    public void Btn_moneyClose()
    {
        money.SetActive(false);
    }

    public void Btn_home()
    {
    }

    public void Btn_commu()
    {
        commu.SetActive(true);
    }
    public void Btn_commuClose()
    {
        commu.SetActive(false );
    }

    public void Btn_walk()
    {
        SceneManager.LoadScene(3);
        // GPS 기반의 씬으로 바뀌게 부탁합니다.
    }

    public void Btn_shop()
    {
        shop.SetActive(true );
    }
    public void Btn_shopClose()
    {
        shop.SetActive (false );
    }

    public void Btn_setting()
    {
        set.SetActive(true);
    }
    public void Btn_settingClose()
    {
        set.SetActive (false );
    }

    public void Btn_castle()
    {
        castle.SetActive(true);
    }
    public void Btn_castleClose()
    {
        castle.SetActive (false );
    }

    public void Btn_market()
    {
        market.SetActive(true);
    }
    public void Btn_marketClose()
    {
        market.SetActive (false );
    }

    public void Btn_gwangjang()
    {
        gwangjang.SetActive(true);
    }
    public void Btn_gwangjangClose()
    {
        gwangjang.SetActive (false );
    }

    public void Btn_hospital()
    {
        hospital.SetActive(true);
    }
    public void Btn_hospitalClose()
    {
        hospital.SetActive (false );
    }

    public void Btn_hometown()
    {
        homePop.SetActive(true);
    }
    public void Btn_hometownClose()
    {
        homePop.SetActive(false);
    }

    public void Set_gongji()
    {
        gongji.SetActive(true);
    }

    public void Set_gongjiClose()
    {
        gongji.SetActive (false);
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
        yakgwan.SetActive (false);
    }

    public void Set_sound()
    {
        soundSet.SetActive(true);
    }
    public void Set_soundClose()
    {
        soundSet.SetActive (false);
    }

    public void Set_alarm()
    {
        alarmSet.SetActive(true);
    }
    public void Set_alarmClose()
    {
        alarmSet.SetActive (false);
    }
    public void Set_ar()
    {
        arSet.SetActive(true);
    }
    public void Set_arClose()
    {
        arSet.SetActive (false);
    }

    public void Set_lang()
    {
        languageSet.SetActive(true);
    }
    public void Set_langClose()
    {
        languageSet.SetActive (false);
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

    public void Quest()
    {
        questPop.SetActive (true);
    }

    public void QuestClose()
    {
        questPop.SetActive (false);
    }

    public void GetQuest()
    {
        userQuestPop.SetActive (true);
    }

    public void LetsQuest()
    {
        // 퀘스트를 수락하는 로직
        userQuestPop.SetActive(false);
    }

    public void NopeQuest()
    {
        // 퀘스트를 거절하는 로직
        userQuestPop.SetActive (false);
    }

    public void QuestSuju()
    {
        // 퀘스트 수주 로직
        suju.SetActive (true);
    }

    public void QuestSujuClose()
    {
        suju.SetActive (false);
    }


    public void GetQuest_Close()
    {
        userQuestPop.SetActive (false);
    }

}
