using UnityEngine;

public class MakePlane : MonoBehaviour
{
    public GameObject quad;

    public int Xaxis = 5;
    public int Yaxis = 5;


    void Start()
    {
        //���μ��� 1�� ���带 ������ �ٴ��� ���ʹ�.
        //0,0,0���� 1�� ������Ű�鼭 ��
        for (int i = 0; i < Xaxis; i++)
        {
            GameObject go = Instantiate(quad);
            go.transform.position = new Vector3(i, 0, 0);

            for (int j = 0; j < Yaxis; j++)
            {
                GameObject go1 = Instantiate(quad);
                go1.transform.position = new Vector3(i, 0, j);
            }
        }

        //���� �ǹ� �߽����κ��� X������ 1�� ���鼭 ���� ���
        //������ �� ��ŭ ���尡 ������� Z������ 1 �ö󰣴��� �ݺ�
        //�׷��� ���� ���� ���尡 �� �򸱶����� �ݺ�

        //���߿��� �������� ���μ��� ��ĭ ��� ���� ��ȴ��� �޾ƿ;���
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
