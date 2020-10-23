using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTPCamera : VirtualCamera
{
    [Header("Camera")]
    [Tooltip("Multiplier for camera sensitivity.")]
    [Range(0f, 300)]
    public float sensitivity = 120f;
    [Tooltip("Relative offset to the target.")]
    public Vector3 offset;
    [Tooltip("Rotation limits for the X-axis in degrees. X represents the lowest and Y the highest value.")]
    public Vector2 rotationLimitsX;
    [Tooltip("Rotation limits for the X-axis in degrees. X represents the lowest and Y the highest value.")]
    public Vector2 rotationLimitsY;
    [Tooltip("Whether the rotation on the X-axis should be limited.")]
    public bool limitXRotation = false;
    [Tooltip("Whether the rotation on the Y-axis should be limited.")]
    public bool limitYRotation = false;
    [Tooltip("Whether the ray will be cast from the mouse position or center of the screen.")]
    public bool useMousePosition = false;

    [Header("Input")]
    [Tooltip("Input button to move to the target position.")]
    public string moveToTarget = "Fire1";
    [Tooltip("Horizontal input axis for rotation, such as horizontal mouse or joystick movement.")]
    public string horizontalRotation = "Mouse X";
    [Tooltip("Vertical input axis for rotation, , such as vertical mouse or joystick movement.")]
    public string verticalRotation = "Mouse Y";

    private Vector2 cameraRotation;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        if(Input.GetButton(moveToTarget))
        {
            Ray ray;
            if(useMousePosition)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0f));
            }

            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                gameObject.transform.position = hit.transform.position + new Vector3(offset.x, offset.y, offset.z);
            }
        }

        cameraRotation.x += Input.GetAxis(horizontalRotation) * sensitivity * Time.deltaTime;
        cameraRotation.y -= TryInvert(Input.GetAxis(verticalRotation)) * sensitivity * Time.deltaTime;

        if(limitXRotation)
        {
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, rotationLimitsX.x, rotationLimitsX.y);
        }
        if(limitYRotation)
        {
            cameraRotation.y = Mathf.Clamp(cameraRotation.y, rotationLimitsY.x, rotationLimitsY.y);
        }

        Quaternion rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);
        transform.rotation = rotation;
    }
}
