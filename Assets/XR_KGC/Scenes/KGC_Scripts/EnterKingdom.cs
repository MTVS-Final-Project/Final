using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnterKingdom : MonoBehaviour
{
    public int sceneNum;
    public ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Update is called once per frame
    void Update()
    {
        // 터치가 있을 때만 실행
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);

            // AR Raycast로 터치 위치의 평면 감지
            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.Planes))
            {
                // Raycast로 감지된 첫 번째 결과를 사용
                Pose hitPose = hits[0].pose;

                // 감지된 위치에 있는 오브젝트 확인
                if (IsTouchedObject(hitPose.position))
                {
                    // 씬 전환
                    SceneManager.LoadScene(sceneNum);
                }
            }
        }
    }

    // 감지된 위치에 특정 오브젝트가 있는지 확인하는 함수
    private bool IsTouchedObject(Vector3 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // 터치 위치에서의 Raycast
        if (Physics.Raycast(ray, out hit))
        {
            // 해당 오브젝트가 이 스크립트가 붙어 있는 오브젝트인 경우 true 반환
            if (hit.transform.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }
}
