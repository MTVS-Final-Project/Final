using UnityEngine;

public class SelectItem : MonoBehaviour
{
    //��ư�� ������ �� ĭ�� ����� ���ӿ�����Ʈ�� move�� objectpoint�� �����ؼ� �����ֱ�.
    public GameObject[] obj = new GameObject[5];   //square ������, temp�� ��������Ʈ ���������� ��������Ʈ�� �ٲٸ� ����������,�迭0���� 1ĭ¥�� ����, �迭1���� 2ĭ ħ��
    // ĹŸ��,ȭ���,����� ��׸�,ħ��ƴ� 2ĭ����
    public int gaguSize;
    public Sprite gaguImage;

    private GameObject gaguParent;//������ ���� �Ѱ��� ��Ƶδ¿�

    public PlaceButtonScript PlaceButtonScript;

    private void Start()
    {
       PlaceButtonScript = GameObject.Find("PlaceCanvas").GetComponent<PlaceButtonScript>();

        if (gaguParent == null)
        {
            gaguParent = GameObject.Find("GaguParent");
        }
    }
    // public Transform point;

    public void ShowObJ()
    {
        // GaguParent ������Ʈ�� ������ ����
       

        if (!PlaceButtonScript.itemOn)
        {
        GameObject go = Instantiate(obj[gaguSize]);
       // go.name = "OBJPreview";
        go.transform.position = new Vector3(0, 0, 0);
            // gaguParent�� �θ�� ����
            go.transform.SetParent(gaguParent.transform);


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
