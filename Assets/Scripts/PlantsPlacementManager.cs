using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class PlantsPlacementManager : MonoBehaviour
{
    public static PlantsPlacementManager instance;
    public List<GameObject> plants;
    public XROrigin aRSessionOrigin;
    public ARRaycastManager aRRaycastManager;
    public ARPlaneManager aRPlaneManager;

    public List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();
    private GameObject selectedPlant = null;
    private float initialDistance;
    private Vector3 initialScale;
    public int activePlant = 0;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            HandleSingleTouch();
        }
        else if (Input.touchCount == 2 && selectedPlant != null)
        {
            HandlePinchToScale();
        }
    }
    public void SetActivePlant(int id)
    {
        activePlant = id;
    }
    private void HandleSingleTouch()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            HandleTouchBegan(touch);
        }
        else if (touch.phase == TouchPhase.Moved && selectedPlant != null)
        {
            MoveSelectedPlant(touch);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            DeselectPlant();
        }
    }

    private void HandleTouchBegan(Touch touch)
    {
        Ray ray = aRSessionOrigin.Camera.ScreenPointToRay(touch.position);

        if (TrySelectPlant(ray))
        {
            // Plant was selected, no need to place a new one
            return;
        }

        if (TryPlaceNewPlant(ray))
        {
            DisablePlanesAndManager();
        }
    }

    private bool TrySelectPlant(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Plant"))
            {
                selectedPlant = hit.transform.gameObject;
                return true;
            }
        }
        return false;
    }

    private bool TryPlaceNewPlant(Ray ray)
    {
        if (aRRaycastManager.Raycast(ray, aRRaycastHits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            Pose hitPose = aRRaycastHits[0].pose;
            GameObject randomPlant = plants[activePlant];
            selectedPlant = Instantiate(randomPlant, hitPose.position, hitPose.rotation);
            return true;
        }
        return false;
    }

    private void MoveSelectedPlant(Touch touch)
    {
        Ray ray = aRSessionOrigin.Camera.ScreenPointToRay(touch.position);
        if (aRRaycastManager.Raycast(ray, aRRaycastHits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            Pose hitPose = aRRaycastHits[0].pose;
            selectedPlant.transform.position = hitPose.position;
        }
    }

    private void DeselectPlant()
    {
        selectedPlant = null;
    }

    private void HandlePinchToScale()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
        {
            InitializePinchToScale(touchZero, touchOne);
        }
        else if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
        {
            ScaleSelectedPlant(touchZero, touchOne);
        }
    }

    private void InitializePinchToScale(Touch touchZero, Touch touchOne)
    {
        initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
        initialScale = selectedPlant.transform.localScale;
    }

    private void ScaleSelectedPlant(Touch touchZero, Touch touchOne)
    {
        float currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
        if (Mathf.Approximately(initialDistance, 0))
        {
            return; // Avoid division by zero
        }

        float scaleFactor = currentDistance / initialDistance;
        selectedPlant.transform.localScale = initialScale * scaleFactor;
    }

    private void DisablePlanesAndManager()
    {
        foreach (var plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        aRPlaneManager.enabled = false;
    }
}
