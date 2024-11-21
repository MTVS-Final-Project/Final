using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    //����� �ൿ ������ ��ũ��Ʈ ���⼭ ������.

    //�÷��̾ ���� ȣ����
    //������� ���� �Ķ���͵�.
    //����� �����, ����� ���. ������� ��а� �÷��̾��� ȣ������ ���� ��ȣ�ۿ��ҿ���.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //cat pos manager�� �����ؼ� ���⼭ �ൿ�� ���ϸ� �� ��ũ��Ʈ���� ��ġ�����ҵ�
    public CatPosManager cpm;

    public float friendly = 100f;//��ȣ��
    public float mood = 100; //���
    public float hunger = 100f; //����� ��ġ
    public float moveTerm = 5; //�󸶳� ���� �����̴���


    public List<GameObject> tiles = new List<GameObject>();//��ȸ�ϴ� �����϶��� ���� ����� Ÿ���� occupied�� �ƴѰ����� �̵�

    //���⼭ �����϶��� catcontroller�� ��� �ڷ�ƾ�� ������Ų�� �����ϴ°� ������

    void Start()
    {
        if (cpm == null)
        {
            GetComponent<CatPosManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (tiles.Count <= 0)
        {
            GameObject tParent = GameObject.Find("TileParent");
            foreach (Transform child in tParent.transform)
            {
                tiles.Add(child.gameObject);
            }

        }
    }

    public IEnumerator Wandering(float Term)
    {
        
        yield return new WaitForSeconds(Term);
    }
}
