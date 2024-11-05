using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public enum CatPersonality { Friendly, Picky }
    public CatPersonality catPersonality = CatPersonality.Friendly; // 기본 성격은 Friendly

    void Update()
    {
        // 키보드 입력으로 고양이 성격 변경
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
            Debug.Log("고양이가 모든 상호작용을 긍정적으로 반응합니다.");
        }
        else if (personality == CatPersonality.Picky)
        {
            Debug.Log("고양이가 특정 상호작용에 민감합니다.");
        }
    }
}