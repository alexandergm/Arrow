using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera2D : VirtualCamera
{
    [Header("Camera")]
    [Tooltip("Multiplier for camera sensitivity.")]
    [Range(0f, 300)]
    public float sensitivity = 90f;
    [Tooltip("Multiplier for normal camera movement.")]
    [Range(0f, 20f)]
    public float normalMoveSpeed = 10f;
    [Tooltip("Multiplier for slower camera movement.")]
    [Range(0f, 5f)]
    public float slowMoveSpeed = 0.25f;
    [Tooltip("Multiplier for faster camera movement.")]
    [Range(0f, 40f)]
    public float fastMoveSpeed = 3f;
    [Tooltip("Relative offset to the original position.")]
    public Vector3 offset;
    [Tooltip("Movement limits on the X-axis. X represents the lowest and Y the highest value.")]
    public Vector2 moveLimitsX;
    [Tooltip("Movement limits on the Y-axis. X represents the lowest and Y the highest value.")]
    public Vector2 moveLimitsY;

    [Header("Input")]
    [Tooltip("Horizontal input axis, such as left and right arrow keys.")]
    public string horizontal = "Horizontal";
    [Tooltip("Vertical input axis, such as up and down arrow keys.")]
    public string vertical = "Vertical";
    [Tooltip("Input button for faster movement.")]
    public string moveFast;
    [Tooltip("Input button for slower movement.")]
    public string moveSlow;

    private Vector3 position;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        position = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        if(moveFast.Length > 0 && moveSlow.Length > 0)
        {
            if(Input.GetButton(moveFast))
            {
                position.x += (normalMoveSpeed * fastMoveSpeed) * Input.GetAxis(horizontal) * Time.deltaTime;
                position.y += (normalMoveSpeed * fastMoveSpeed) * TryInvert(Input.GetAxis(vertical)) * Time.deltaTime;
            }
            else if(Input.GetButton(moveSlow))
            {
                position.x += (normalMoveSpeed * slowMoveSpeed) * Input.GetAxis(horizontal) * Time.deltaTime;
                position.y += (normalMoveSpeed * slowMoveSpeed) * TryInvert(Input.GetAxis(vertical)) * Time.deltaTime;
            }
            else
            {
                position.x += normalMoveSpeed * Input.GetAxis(horizontal) * Time.deltaTime;
                position.y += normalMoveSpeed * TryInvert(Input.GetAxis(vertical)) * Time.deltaTime;
            }
        }
        else
        {
            position.x += normalMoveSpeed * Input.GetAxis(horizontal) * Time.deltaTime;
            position.y += normalMoveSpeed * TryInvert(Input.GetAxis(vertical)) * Time.deltaTime;
        }

        position.x = Mathf.Clamp(position.x, moveLimitsX.x, moveLimitsX.y);
        position.y = Mathf.Clamp(position.y, moveLimitsY.x, moveLimitsY.y);
        transform.position = position + offset;
    }
}
