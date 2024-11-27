using UnityEngine;

public class SpritePos : MonoBehaviour
{
    
    public enum PositionMode
    {
        Mode1, // 기존의 0~10 y좌표에 따른 보간
        Mode2, // 0~10 y좌표에 따른 보간된 값에 +0.001 추가
        Mode3  // 다른 동작 (필요에 따라 추가 가능)
    }
    public SpriteRenderer Renderer; //레이어오더변경용

    public PositionMode currentMode = PositionMode.Mode1;
    Transform child;

    void Start()
    {
        // 자식 객체 설정 (인덱스 1)
        child = transform.GetChild(1);
        Renderer = child.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 현재 부모 객체의 y 좌표 가져오기
        float parentY = transform.position.y;
       // Vector3 worldPosition = child.position;

        switch (currentMode)
        {
            case PositionMode.Mode1:
                // y 좌표가 0에서 10 사이일 때 z 좌표 보간
                if (parentY >= 0 && parentY <= 10)
                {
                    int zOrder = (int)Mathf.Lerp(0, -100, parentY / 10);
                    Renderer.sortingOrder = zOrder;
                    //child.position = worldPosition;
                }
                break;

            case PositionMode.Mode2:
                // y 좌표가 0에서 10 사이일 때 보간된 값에 +0.001 추가
                if (parentY >= 0 && parentY <= 10)
                {
                    int zOrder = (int)Mathf.Lerp(0, -100, parentY / 10)-1;
                    Renderer.sortingOrder = zOrder;
                    //child.position = worldPosition;
                }
                break;

            case PositionMode.Mode3:
                // 필요에 따라 다른 동작 추가 가능
                break;
        }
    }
}
