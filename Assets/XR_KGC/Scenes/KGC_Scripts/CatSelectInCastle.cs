using UnityEngine;

public class CatSelectInCastle : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ���� ��ư Ŭ�� Ȯ��
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����ĳ��Ʈ�� ������ ������Ʈ üũ
            if (Physics.Raycast(ray, out hit))
            {
                // ������ ������Ʈ�� �̸��� "Cat"���� Ȯ��
                if (hit.collider.gameObject.name == "Cat")
                {
                    Debug.Log("Cat detected: " + hit.collider.gameObject.name);
                    HandleCatSelection(hit.collider.gameObject);
                }
            }
        }
    }

    // "Cat" ���� �� ó���� ����
    void HandleCatSelection(GameObject catObject)
    {
        // ���⿡ "Cat"�� �������� ���� �ൿ �ۼ�
        Debug.Log("Selected Cat: " + catObject.name);
    }
}
