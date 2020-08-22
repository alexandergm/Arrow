using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCamera : VirtualCamera
{
    [Header("Camera")]
    [Tooltip("Multiplier for the movement speed.")]
    [Range(0f, 200f)]
    public float dragSpeed = 2;
    [Tooltip("Multiplier for the zoom speed.")]
    [Range(0f, 200f)]
    public float scrollSpeed = 100.0f;
    [Tooltip("Smoothing factor.")]
    [Range(0f, 10f)]
    public float smoothFactor = 0.2f;
    [Tooltip("Movement limits on the X-axis. X represents the lowest and Y the highest value.")]
    public Vector2 moveLimitsX;
    [Tooltip("Movement limits on the Y-axis. X represents the lowest and Y the highest value.")]
    public Vector2 scrollLimitsY;
    [Tooltip("Movement limits on the Z-axis. X represents the lowest and Y the highest value.")]
    public Vector2 moveLimitsZ;

    [Header("Input")]
    [Tooltip("Input axis to change the offset.")]
    public string zoom = "Mouse ScrollWheel";
    [Tooltip("Input button to start dragging the camera.")]
    public string drag = "Fire1";

    private Vector3 dragOrigin;
    private Vector3 movement;
    private Vector3 worldPosition;
    private Vector3 desiredPosition;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        desiredPosition = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    void LateUpdate()
    {
        if(Input.GetButtonDown(drag))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if(!Input.GetButton(drag)) return;

        worldPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

        movement = new Vector3(TryInvert(-worldPosition.x) * dragSpeed, 0, TryInvert(-worldPosition.y) * dragSpeed);

        movement.y = Input.GetAxis(zoom) * scrollSpeed * Time.deltaTime;

        movement += desiredPosition;
        movement.x = Mathf.Clamp(movement.x, moveLimitsX.x, moveLimitsX.y);
        movement.y = Mathf.Clamp(movement.y, scrollLimitsY.x, scrollLimitsY.y);
        movement.z = Mathf.Clamp(movement.z, moveLimitsX.x, moveLimitsX.y);
        desiredPosition = movement;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothFactor);
    }
}
