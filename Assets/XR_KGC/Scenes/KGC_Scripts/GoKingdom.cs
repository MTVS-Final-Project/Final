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
        //플레이어가 일정거리안에 있으면 true로 바꿈
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

            print("이동");
        }


    }

    public void ToKingdom()
    {

        if (inRange)
        {
            SceneManager.LoadScene(SceneNumber);
             print("씬이동");
        }
        
    }
}
