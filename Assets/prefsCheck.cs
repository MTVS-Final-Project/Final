using UnityEngine;

public class prefsCheck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // "selectedCharacter" Ű ���� �ִ��� Ȯ��
        if (PlayerPrefs.HasKey("selectedCharacter"))
        {
            // �� ��������
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
