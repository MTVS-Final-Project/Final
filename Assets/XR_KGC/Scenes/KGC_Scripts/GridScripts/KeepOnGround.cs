using UnityEngine;
using UnityEngine.UIElements;

public class KeepOnGround : MonoBehaviour
{
    public bool inGround;

    public GaguCheck check;
    public DragObject dragObject;

    public Vector3 nowPos;
    public Vector3 lastPos;

    public Quaternion nowRot;
    public Quaternion lastRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        check = transform.GetChild(0).GetComponent<GaguCheck>();
        nowPos = transform.position;
        nowRot = transform.rotation;
        dragObject = transform.GetComponent<DragObject>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (check.onGagu)
        //{
        //    transform.position = check.snapPos;

        //}
        //드레그 끝났을때만 체크
        if (!dragObject.isDragging)
        {

            if (check.onGround)
            {
                lastPos = nowPos;
                nowPos = transform.position;

                lastRot = nowRot;
                nowRot = transform.rotation;
            }
            else if (!check.onGround)
            {
                transform.position = check.snapPos;
                transform.rotation = lastRot;
            }
        }
    }
}
