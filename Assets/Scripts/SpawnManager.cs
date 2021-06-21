using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.AR;
using System;
using UnityEngine.XR.ARSubsystems;

public class SpawnManager : ARBaseGestureInteractable
{
    [SerializeField] private Camera arCam;
    [SerializeField] private ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    private Touch touch;
    [SerializeField] private GameObject crosshair;
    private Pose pose;


    // Start is called before the first frame update
    void Start()
    {

    }

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if(gesture.targetObject == null)
        {
            return true;
        }
        return false;
    }


    protected override void OnEndManipulation(TapGesture gesture)
    {
        if(gesture.isCanceled)
        {
            return;
        }

        if(gesture.targetObject != null || isPointerOverUI(gesture))
        {
            return;
        }

        if (GestureTransformationUtility.Raycast(gesture.startPosition, m_Hits, TrackableType.PlaneWithinPolygon))
        {
            GameObject place_Obj = Instantiate(DataManager.Instance.getFurniture(), pose.position, pose.rotation);

            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = pose.position;
            anchorObject.transform.rotation = pose.rotation;

            place_Obj.transform.parent = anchorObject.transform;
        }
    }

    private void FixedUpdate()
    {
        CrosshairCalculation();
    }

    bool isPointerOverUI(TapGesture touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.startPosition.x, touch.startPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    void CrosshairCalculation()
    {
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));

        if (GestureTransformationUtility.Raycast(origin, m_Hits, TrackableType.PlaneWithinPolygon))
        {
            pose = m_Hits[0].pose;
            crosshair.transform.position = pose.position;
            crosshair.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }

}
