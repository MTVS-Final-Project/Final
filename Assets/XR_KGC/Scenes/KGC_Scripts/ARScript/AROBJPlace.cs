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
        //    // ����� ������ ���, �ش� ��ġ�� ������Ʈ ��ġ
        //    Pose placementPose = hits[0].pose;
        //    OBJ.SetActive(true);
        //    OBJ.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        //}
        //else
        //{
        //    // ����� �������� ���� ��� ���� ��ġ ����
        //    PlaceObjectAtRandomDistance();
        //}
    }

    private void PlaceObjectAtRandomDistance()
    {
        // ������ ���� ������ ���� �Ÿ�
        float randomDistance = Random.Range(1.0f, 2.0f);

        // ȭ�� �߾ӿ��� ���� ���� ���� (��/�Ʒ�/��/�� ����)
        Vector2 randomDirection = Random.insideUnitCircle.normalized * randomDistance;

        // ��ũ�� ��ǥ�� ȭ�� �߽ɿ��� ���� �������� �̵��� ��ġ�� ����
        Vector3 randomScreenPos = new Vector3(0.5f + randomDirection.x, 0.5f + randomDirection.y, randomDistance);

        // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ (z���� randomDistance�� ����)
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(randomScreenPos);

        // OBJ ������Ʈ Ȱ��ȭ �� ��ġ ����
        OBJ.SetActive(true);
        OBJ.transform.position = worldPosition;

        // �⺻���� ȸ�� ���� (ī�޶� ���ϵ��� ����)
        OBJ.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

}
