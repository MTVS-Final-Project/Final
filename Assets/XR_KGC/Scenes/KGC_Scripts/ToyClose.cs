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
        // toys ��ü�� Ȱ�� ���·� �ΰ�, �ڽĵ鸸 ��Ȱ��ȭ
        foreach (Transform child in toys.transform)
        {
            child.gameObject.SetActive(false); // �� �ڽ� ������Ʈ ��Ȱ��ȭ
        }
    }
}
