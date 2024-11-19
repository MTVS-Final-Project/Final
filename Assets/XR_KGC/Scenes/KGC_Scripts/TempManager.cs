using System.Collections.Generic;
using UnityEngine;

public class TempManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static TempManager Instance;
    //�÷��̾ ���� ��� �������� �����ؾߵǴ� ��ũ��Ʈ
    //

    //�÷��̾��� Ŀ���� ��  
    //�÷��̾��� ����� ����,�� ����̰� ���� ����.����� ����,�÷��̾ ���� ȣ���� ��
    //�÷��̾ ���� ��ū,�÷��̾ ������ ����,�÷��̾ ���� ������
    //�÷��̾� �������� ���� ,����� ��׸��� ���ֳ� �پ�����, ȭ����� ������ֳ� ���
    //�÷��̾ ������ ����Ʈ��.

    //�÷��̾��� ���
    public int tokne;

    // ���� ������ ����Ʈ (�ּ� ����),�÷��̾��� ���� ��ġ�� �������� ����
    public List<GaguSave.GaguData> gaguDataList = new List<GaguSave.GaguData>();

    void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϴ� ��� �� ������Ʈ�� �ı�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ���� ��� �� ������Ʈ�� �ν��Ͻ��� �����ϰ� �ı����� �ʵ��� ����
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
