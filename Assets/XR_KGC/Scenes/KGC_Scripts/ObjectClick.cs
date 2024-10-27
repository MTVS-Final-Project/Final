using UnityEngine;

public class ObjectClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            DetectObjectOnClick();
        }
    }

    void DetectObjectOnClick()
    {
        //print("11111111111111111111111111111111111");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Debug Ray를 Scene 창에서 시각적으로 확인하기 위해 충분히 길게 설정
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 2f);

        // Raycast 발사 및 충돌 여부 확인
        //if (Physics.Raycast(ray, out hit, 1000f))
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000,1<<10))
        {
           // print("2222222222222222222222222222222222222222");
            GameObject clickedObject = hit.collider.gameObject;

            clickedObject.GetComponent<GoKingdom>().ToKingdom();

            Debug.Log("Clicked on object: " + clickedObject.name); // 오브젝트 이름 출력
        }
        else
        {
            Debug.Log("No object was hit.");
           // print("3333333333333333333333333333333333");
        }


        
    }
}
