using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public enum CatPersonality { Friendly, Picky }
    public CatPersonality catPersonality = CatPersonality.Friendly; // �⺻ ������ Friendly

    void Update()
    {
        // Ű���� �Է����� ����� ���� ����
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
            Debug.Log("����̰� ��� ��ȣ�ۿ��� ���������� �����մϴ�.");
        }
        else if (personality == CatPersonality.Picky)
        {
            Debug.Log("����̰� Ư�� ��ȣ�ۿ뿡 �ΰ��մϴ�.");
        }
    }
}