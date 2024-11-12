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
        // ��ġ�� ���� ���� ����
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);

            // AR Raycast�� ��ġ ��ġ�� ��� ����
            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.Planes))
            {
                // Raycast�� ������ ù ��° ����� ���
                Pose hitPose = hits[0].pose;

                // ������ ��ġ�� �ִ� ������Ʈ Ȯ��
                if (IsTouchedObject(hitPose.position))
                {
                    // �� ��ȯ
                    SceneManager.LoadScene(sceneNum);
                }
            }
        }
    }

    // ������ ��ġ�� Ư�� ������Ʈ�� �ִ��� Ȯ���ϴ� �Լ�
    private bool IsTouchedObject(Vector3 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // ��ġ ��ġ������ Raycast
        if (Physics.Raycast(ray, out hit))
        {
            // �ش� ������Ʈ�� �� ��ũ��Ʈ�� �پ� �ִ� ������Ʈ�� ��� true ��ȯ
            if (hit.transform.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }
}
