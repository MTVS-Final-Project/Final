using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AROBJPlace : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject OBJ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CodeStart());
    }

    // Update is called once per frame
    void Update()
    {
    }


    public IEnumerator CodeStart()
    {
        yield return new WaitForSeconds(1f);

        UpdateCenterObject();

    }
    private void UpdateCenterObject()
    {
        PlaceObjectAtRandomDistance();

        //Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        //List<ARRaycastHit> hits = new List<ARRaycastHit>();
        //arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        //if (hits.Count > 0)
        //{
        //    // 평면이 감지된 경우, 해당 위치에 오브젝트 배치
        //    Pose placementPose = hits[0].pose;
        //    OBJ.SetActive(true);
        //    OBJ.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        //}
        //else
        //{
        //    // 평면이 감지되지 않은 경우 랜덤 위치 설정
        //    PlaceObjectAtRandomDistance();
        //}
    }

    private void PlaceObjectAtRandomDistance()
    {
        // 지정된 범위 사이의 랜덤 거리
        float randomDistance = Random.Range(1.0f, 2.0f);

        // 화면 중앙에서 랜덤 방향 선택 (위/아래/좌/우 임의)
        Vector2 randomDirection = Random.insideUnitCircle.normalized * randomDistance;

        // 스크린 좌표를 화면 중심에서 랜덤 방향으로 이동한 위치로 설정
        Vector3 randomScreenPos = new Vector3(0.5f + randomDirection.x, 0.5f + randomDirection.y, randomDistance);

        // 스크린 좌표를 월드 좌표로 변환 (z값을 randomDistance로 설정)
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(randomScreenPos);

        // OBJ 오브젝트 활성화 및 위치 설정
        OBJ.SetActive(true);
        OBJ.transform.position = worldPosition;

        // 기본적인 회전 설정 (카메라를 향하도록 설정)
        OBJ.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

}
