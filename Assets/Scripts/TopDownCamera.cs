using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    private static readonly float PanSpeed = 1000f;
    private static readonly float ZoomSpeedTouch = 0.1f;
    private static readonly float ZoomSpeedMouse = 40f;

    private static readonly float[] BoundsX = new float[] { -4900f, 4900f }; //left to right
    private static readonly float[] BoundsZ = new float[] { -3000f, 3000f }; //up and down
    private static readonly float[] ZoomBounds = new float[] { 40f, 165f };  // {zoomed in, zoomed out}

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    public GameObject target; // the object to follow when camera is locked
    public GameObject followButtonManager;
    public GameObject compassButtonManager;
    public bool CameraLocked = true;
    public bool compassRotationOn = true;
    private float smoothFactor = 4f; //constant used to smooth out map rotation based on the compass

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        target = GameObject.Find("User");
        followButtonManager = GameObject.Find("Follow User");
        compassButtonManager = GameObject.Find("Compass Button Text");
        
        //enable compass
        Input.compass.enabled = true;
        Input.location.Start();
    }

    void Update()
    {
        CameraLocked = followButtonManager.GetComponent<FollowUserButtonManager>().followOn;
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer && !CameraLocked)
        {
            HandleTouch();
        }
        else if (!CameraLocked)
        {
            HandleMouse();
        }
        else //means that the camera is locked
        {
            // Update the camera's position and rotation to follow the target.
            Vector3 targetPosition = target.transform.position;
            Vector3 cameraPosition = new Vector3(targetPosition.x, targetPosition.y + 800f, targetPosition.z);
            transform.position = cameraPosition;
        }        
    }

    void LateUpdate(){
        compassRotationOn = compassButtonManager.GetComponent<CompassButtonManager>().rotateOn;
        if (compassRotationOn == true){
            Quaternion rotation = Quaternion.Euler(90f, Input.compass.trueHeading + 270f, 0f); //for overhead camera (North at 270 degrees)
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothFactor);
        }
        else{
            transform.rotation = Quaternion.Euler(90f, 270f, 0f); //face North
        }
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Calculate the dynamic pan speed factor based on the camera's field of view.
        float dynamicPanSpeed = PanSpeed * (cam.fieldOfView / ZoomBounds[0]);

        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);

    
        // Use the camera's rotation to adjust the offset
        offset = Quaternion.Euler(0, 0, -cam.transform.eulerAngles.y) * offset;

        // Apply the pan speed to both horizontal and vertical movements
        Vector3 move = new Vector3(offset.x * dynamicPanSpeed, 0, offset.y * dynamicPanSpeed);
        
        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
}