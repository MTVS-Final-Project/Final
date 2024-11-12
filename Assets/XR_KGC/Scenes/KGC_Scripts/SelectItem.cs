using UnityEngine;

public class SelectItem : MonoBehaviour
{
    //버튼을 누르면 이 칸에 저장된 게임오브잭트를 move툴 objectpoint에 복사해서 보여주기.
    public GameObject[] obj = new GameObject[5];   //square 프리펩, temp의 스프라이트 렌더러에서 스프라이트를 바꾸면 되지않을까,배열0번은 1칸짜리 가구, 배열1번은 2칸 침대
    // 캣타워,화장실,고양이 밥그릇,침대아닌 2칸가구
    public int gaguSize;
    public Sprite gaguImage;

    private GameObject gaguParent;//생성된 가구 한곳에 모아두는용

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
        // GaguParent 오브젝트가 없으면 생성
       

        if (!PlaceButtonScript.itemOn)
        {
        GameObject go = Instantiate(obj[gaguSize]);
       // go.name = "OBJPreview";
        go.transform.position = new Vector3(0, 0, 0);
            // gaguParent를 부모로 설정
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
