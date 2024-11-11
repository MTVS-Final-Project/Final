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

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCenterObject();
    }

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            Pose placementPose = hits[0].pose;
            OBJ.SetActive(true);
            OBJ.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }

    }
}