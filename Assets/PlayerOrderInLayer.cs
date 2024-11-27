using Spine.Unity;
using UnityEngine;

public class PlayerOrderInLayer : MonoBehaviour
{
    // public SkeletonAnimation SkeletonAnimation;
    public MeshRenderer mr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // SkeletonAnimation = GetComponent<SkeletonAnimation>();
       mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // y 좌표가 0에서 10 사이일 때 z 좌표 보간
        if (transform.position.y >= 0 && transform.position.y <= 10)
        {
            int zOrder = (int)Mathf.Lerp(0, -100, transform.position.y / 10);
            mr.sortingOrder = zOrder;
            //child.position = worldPosition;
        }
    }
}
