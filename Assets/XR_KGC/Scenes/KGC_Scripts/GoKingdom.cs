using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GoKingdom : MonoBehaviour
{
    public bool inRange;

    public GameObject player;

    public int SceneNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //�÷��̾ �����Ÿ��ȿ� ������ true�� �ٲ�
        if (Vector3.Distance(transform.position, player.transform.position) < 35)
        {
            inRange = true;

        }
        else
        {
            inRange = false;
        }

    }

    private void OnMouseDown()
    {
        if (inRange)
        {

            print("�̵�");
        }


    }

    public void ToKingdom()
    {

        if (inRange)
        {
            SceneManager.LoadScene(SceneNumber);
             print("���̵�");
        }
        
    }
}
