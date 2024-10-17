using UnityEngine;

public class OBJPlacement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //아래쪽으로 레이를 쏴서 ground 래이어 오브젝트에 맞으면 그 오브젝트에 붙게 만들기
        RaycastHit hit;

        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out hit, 100,1<<16))
        {
            transform.position = hit.transform.position;
        }

    }
}
