using UnityEngine;

public class SelectItem : MonoBehaviour
{
    //��ư�� ������ �� ĭ�� ����� ���ӿ�����Ʈ�� move�� objectpoint�� �����ؼ� �����ֱ�.
    public GameObject obj;

    public Transform point;

     public void ShowObJ()
    {
        GameObject go = Instantiate(obj);
       // go.transform.position = new Vector3(0, 0, 0);
        go.transform.position = point.position;
    }
}
