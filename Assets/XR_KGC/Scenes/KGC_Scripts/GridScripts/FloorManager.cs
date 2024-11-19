using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    //�ٴ� ���ӿ�����Ʈ
    public GameObject floor;
    public GameObject TileParent;

    public List<float> Xpos = new List<float>();
    public List<float> Ypos = new List<float>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TileParent�� null�̶�� Find�� �˻�
        if (TileParent == null)
        {
            TileParent = GameObject.Find("TileParent");
            FloorPosCheck();
        }

        // TileParent�� ������ ��� �ڽ� ��ȸ
      
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadFloor();
        }
    }

    public void FloorPosCheck()
    {
        if (TileParent != null)
        {
            // ����Ʈ �ʱ�ȭ (�ߺ� �߰� ����)
            Xpos.Clear();
            Ypos.Clear();

            // �ڽ� ��ȸ�ϸ� X, Y �� �߰�
            foreach (Transform child in TileParent.transform)
            {
                // �ڽ��� position �� ��������
                Vector3 childPosition = child.position;

                // X�� Y ���� ����Ʈ�� �߰� (�Ҽ��� ���Ÿ� ���� int ��ȯ)
                Xpos.Add(childPosition.x);
                Ypos.Add(childPosition.y);
            }
        }
    }

    public void LoadFloor()
    {
        // Xpos ����Ʈ�� ������ ����ŭ ��ȯ
        for (int i = 0; i < Xpos.Count; i++)
        {
            // ���ο� floor ���� ������Ʈ ����
            GameObject go = Instantiate(floor);

            // �̸� ���� (optional)
            go.name = $"floor_{i}";
            go.transform.parent = TileParent.transform;

            // ������ ���� ������Ʈ�� ��ġ ����
            go.transform.position = new Vector3(Xpos[i], Ypos[i], 0f);
        }
    }

}
