using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public enum CatPersonality { Friendly, Picky }
    public CatPersonality catPersonality;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // ����, ��� �Ӹ� ĳ����
        {
            catPersonality = CatPersonality.Friendly;
            Debug.Log("����̰� ��� ��ȣ�ۿ��� ���������� �����մϴ�.");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // ��ٷο� ���� , ���� �Ӹ� ĳ����
        {
            catPersonality = CatPersonality.Picky;
            Debug.Log("����̰� Ư�� ��ȣ�ۿ뿡 �ΰ��մϴ�.");
        }
    }
}
