using UnityEngine;
using UnityEngine.UI;

public class CharatarCustomizer : MonoBehaviour
{
    [SerializeField] Transform[] cameraPositions;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject avatarMenu;

    [SerializeField] AvatarSetUp avatar;


    [SerializeField] Color[] defaultColor;

    [SerializeField] Image hairButton;
    [SerializeField] Image bodyButton;
    [SerializeField] Image pantButton;
    [SerializeField] Image shoesButton;


    void Start()
    {
    }
    void Update()
    {
        
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        avatarMenu.SetActive(false);
        Camera.main.transform.position = cameraPositions[0].position;
    }

    public void ChangeHair(int param)
    {
        PlayerData.Instance.data.hair += param;

        if(PlayerData.Instance.data.hair >= avatar.hairCount)
        {
            PlayerData.Instance.data.hair = 0;
        }
        if(PlayerData.Instance.data.hair < 0)
        {
            PlayerData.Instance.data.hair = avatar.hairCount - 1;
        }
        PlayerPrefs.SetInt("hair", PlayerData.Instance.data.hair);
        avatar.SetAvatar(PlayerData.Instance.data);
    }

    public void ChangeBody(int param)
    {
        PlayerData.Instance.data.body += param;

        if (PlayerData.Instance.data.body >= avatar.bodyCount)
        {
            PlayerData.Instance.data.body = 0;
        }
        if (PlayerData.Instance.data.body < 0)
        {
            PlayerData.Instance.data.body = avatar.bodyCount - 1;
        }
        PlayerPrefs.SetInt("body", PlayerData.Instance.data.body);
        avatar.SetAvatar(PlayerData.Instance.data);
    }
}

