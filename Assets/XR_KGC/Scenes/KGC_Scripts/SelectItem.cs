using UnityEngine;

public class SelectItem : MonoBehaviour
{
    //버튼을 누르면 이 칸에 저장된 게임오브잭트를 move툴 objectpoint에 복사해서 보여주기.
    public GameObject obj; //square 프리펩, temp의 스프라이트 렌더러에서 스프라이트를 바꾸면 되지않을까
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
