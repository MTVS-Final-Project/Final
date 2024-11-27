using Unity.VisualScripting;
using UnityEngine;

public class CatSelectInCastle : MonoBehaviour
{
    public Camera cam;

    public GameObject catCanvas;
    public bool zoom = false;
    private float zoomSpeed = 5f; // �� �ִϸ��̼� �ӵ�
    public GameObject selectedCat; // ���õ� Cat ������Ʈ�� ����
    public GameObject backButton;
    public Vector3 initPos =new Vector3(0,1,-10);
    public CatData catData;

    public int count = 1; //�ѹ��� ������ ������.

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        backButton.SetActive(false);
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

        // �� ������ Ȱ��ȭ�Ǿ��� ��
        if (zoom && selectedCat != null)
        {
            // ī�޶� ����� 2�� ���������� ���̱�
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 2f, Time.deltaTime * zoomSpeed);

            // ���õ� Cat ������Ʈ�� X, Y�� ���󰡰� ����
            Vector3 targetPosition = new Vector3(selectedCat.transform.position.x, selectedCat.transform.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * zoomSpeed);
        }
        if (!zoom)
        {
            catCanvas.SetActive(false);
        }
        else
        {
            catCanvas.SetActive(true);
        }
    }

    // "Cat" ���� �� ó���� ����
    void HandleCatSelection(GameObject catObject)
    {
        Debug.Log("Selected Cat: " + catObject.name);

        // ���õ� Cat ������Ʈ ���� �� �� Ȱ��ȭ
        selectedCat = catObject;
        zoom = true;
        backButton.SetActive(true);
    }

    public void ExitZoom()
    {
        zoom = false;
        cam.orthographicSize =5;
        backButton.SetActive(false);
        cam.transform.position = initPos;
    }

    public void Interaction()
    {
        if (count > 0)
        {
            ExitZoom();
        }
        else
        {
            //catData
        }
    }
}
