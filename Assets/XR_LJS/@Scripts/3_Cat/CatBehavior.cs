using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public enum CatPersonality { Friendly, Picky }
    public CatPersonality catPersonality;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 긍정, 노란 머리 캐릭터
        {
            catPersonality = CatPersonality.Friendly;
            Debug.Log("고양이가 모든 상호작용을 긍정적으로 반응합니다.");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 까다로운 성격 , 검은 머리 캐릭터
        {
            catPersonality = CatPersonality.Picky;
            Debug.Log("고양이가 특정 상호작용에 민감합니다.");
        }
    }
}
