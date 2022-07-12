using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private bool hideCursor = true;
    [SerializeField]
    private float sensitivityX = 1;
    [SerializeField]
    private float sensitivityY = 1;
    [SerializeField]
    private float maxLookUp = 90f;
    [SerializeField]
    private float maxLookDown = -90f;

    [Header("References")]
    [SerializeField]
    private Transform Orientation;


    private float inputX;
    private float inputY;
    private float xRot;
    private float yRot;

    
    private void Awake()
    {
        NullCheckVars();
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    private void LateUpdate()
    {
        GetInput();


        yRot += inputX;
        xRot -= inputY;

        xRot = Mathf.Clamp(xRot, maxLookDown, maxLookUp);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0); // Z rotation should always be zero i guess....
        Orientation.rotation = Quaternion.Euler(0, yRot, 0); // Replacce these zeros with current rotation if there is any problem
    }



    private void GetInput()
    {
        inputX = Input.GetAxisRaw("Mouse X") * sensitivityX;
        inputY = Input.GetAxisRaw("Mouse Y") * sensitivityY;
        
    }








    #region Helpers
    private void NullCheckVars()
    {

#if UNITY_EDITOR
        if (Orientation == null)
        {
            Debug.LogError(nameof(Orientation) + $" variable is not assigned in {nameof(CameraLook)} script.");
            EditorApplication.ExitPlaymode();
            Debug.Log("Exited Playmode");
        }
#endif
    }
    #endregion
}
