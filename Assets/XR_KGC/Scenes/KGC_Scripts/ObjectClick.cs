using UnityEngine;

public class ObjectClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ��
        {
            DetectObjectOnClick();
        }
    }

    void DetectObjectOnClick()
    {
        //print("11111111111111111111111111111111111");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Debug Ray�� Scene â���� �ð������� Ȯ���ϱ� ���� ����� ��� ����
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 2f);

        // Raycast �߻� �� �浹 ���� Ȯ��
        //if (Physics.Raycast(ray, out hit, 1000f))
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000,1<<10))
        {
           // print("2222222222222222222222222222222222222222");
            GameObject clickedObject = hit.collider.gameObject;

            clickedObject.GetComponent<GoKingdom>().ToKingdom();

            Debug.Log("Clicked on object: " + clickedObject.name); // ������Ʈ �̸� ���
        }
        else
        {
            Debug.Log("No object was hit.");
           // print("3333333333333333333333333333333333");
        }


        
    }
}
