using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
    [Header("Preferences")]
    [Tooltip("Whether the movement should be inverted.")]
    public bool invertMovement = false;
    [Tooltip("Whether the cursor should be hidden in play mode.")]
    public bool hideCursor = false;

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(hideCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    protected float TryInvert(float value)
    {
        if(invertMovement)
        {
            value = -1 * value;
        }

        return value;
    }
}
