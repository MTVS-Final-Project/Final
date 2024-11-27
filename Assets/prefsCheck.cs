using UnityEngine;

public class prefsCheck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // "selectedCharacter" 키 값이 있는지 확인
        if (PlayerPrefs.HasKey("selectedCharacter"))
        {
            // 값 가져오기
            string selectedCharacter = PlayerPrefs.GetString("selectedCharacter");
            Debug.Log("Selected Character: " + selectedCharacter);
        }
        else
        {
            Debug.Log("No character selected yet!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
