using UnityEngine;

public class SelectItem : MonoBehaviour
{
    //��ư�� ������ �� ĭ�� ����� ���ӿ�����Ʈ�� move�� objectpoint�� �����ؼ� �����ֱ�.
    public GameObject[] obj = new GameObject[5];   //square ������, temp�� ��������Ʈ ���������� ��������Ʈ�� �ٲٸ� ����������,�迭0���� 1ĭ¥�� ����, �迭1���� 2ĭ
    // ĹŸ��,ȭ���,����� ��׸�
    public int gaguSize;
    public Sprite gaguImage;

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
        GameObject go = Instantiate(obj[gaguSize]);
       // go.name = "OBJPreview";
        go.transform.position = new Vector3(0, 0, 0);

            Transform secondChild = go.transform.GetChild(1);
            SpriteRenderer sr = secondChild.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                sr.sprite = gaguImage;
            }
        

           }
       // go.transform.position = point.position;
    }
}
