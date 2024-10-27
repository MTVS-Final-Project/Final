using UnityEngine;

public class SelectItem : MonoBehaviour
{
    //버튼을 누르면 이 칸에 저장된 게임오브잭트를 move툴 objectpoint에 복사해서 보여주기.
    public GameObject obj;

    public Transform point;

     public void ShowObJ()
    {
        GameObject go = Instantiate(obj);
       // go.transform.position = new Vector3(0, 0, 0);
        go.transform.position = point.position;
    }
}
