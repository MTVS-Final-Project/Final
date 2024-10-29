using UnityEngine;

public class ToyClose : MonoBehaviour
{
    GameObject toys;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toys = GameObject.Find("ToyCanvas");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Close()
    {
        // toys 자체는 활성 상태로 두고, 자식들만 비활성화
        foreach (Transform child in toys.transform)
        {
            child.gameObject.SetActive(false); // 각 자식 오브젝트 비활성화
        }
    }
}
