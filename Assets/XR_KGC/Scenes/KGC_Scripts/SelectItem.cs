using UnityEngine;

public class SelectItem : MonoBehaviour
{
    //��ư�� ������ �� ĭ�� ����� ���ӿ�����Ʈ�� move�� objectpoint�� �����ؼ� �����ֱ�.
    public GameObject obj; //square ������, temp�� ��������Ʈ ���������� ��������Ʈ�� �ٲٸ� ����������
    public PlaceButtonScript PlaceButtonScript;

    private void Start()
    {
       PlaceButtonScript = GameObject.Find("PlaceCanvas").GetComponent<PlaceButtonScript>();
    }
    // public Transform point;

    public void ShowObJ()
    {
        if (!PlaceButtonScript.itemOn)
        {
        GameObject go = Instantiate(obj);
       // go.name = "OBJPreview";
        go.transform.position = new Vector3(0, 0, 0);

           }
       // go.transform.position = point.position;
    }
}
