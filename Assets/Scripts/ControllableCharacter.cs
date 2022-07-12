using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ControllableCharacter : MonoBehaviour
{   

    [Header("Movement")]
    [SerializeField]
    private float walkAcceleration = 70f;
    [SerializeField]
    private float sprintAcceleration = 70 * 2;
    [SerializeField]
    private float maxWalkspeed = 15f;
    [SerializeField]
    private float maxSprintSpeed = 25f;
    [SerializeField]
    private float groundDrag;
    [SerializeField]
    private float airDrag = 0;
    [SerializeField]
    private float airMoveMultiplier = 0.5f;
    [SerializeField]
    private float maxSlopeAngle = 40f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCoolDown = 1f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float gravityScale = 1f;

    [Header("Crouching/Sliding")]
    [SerializeField] private float crouchSpeed = 5f;

    [Header("Events")]
    [SerializeField] public UnityEvent onCrouchStart;
    [SerializeField] public UnityEvent onCrouchEnd;
    [SerializeField] public UnityEvent onJump;
    [SerializeField] public UnityEvent onLand;

    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundcheckRadius = 0.2f;

    [Header("Roof Check")]
    [SerializeField] private LayerMask whatIsRoof;
    [SerializeField] private float roofcheckRadius = 0.2f;

    [Header("References")]
    [SerializeField] private Transform orientation; 
    [SerializeField] private Transform groundcheckPosition;
    [SerializeField] private Transform roofcheckPosition;
    



    private Rigidbody rb;
    private Vector3 moveDir;
    private GroundInfo surfaceInfo = new GroundInfo();
    private bool readyToJump = true;
    bool isSprinting = false;
    bool isCrouching;
    bool wantsToStandUp = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void MovePlayer(float inputX, float inputY, bool sprint = false)
    {
        isSprinting = sprint;

        moveDir = orientation.forward * inputY + orientation.right * inputX;

        if(surfaceInfo.IsGrounded)
            ManageSlope();

        if(surfaceInfo.IsGrounded)
            rb.AddForce(moveDir.normalized * (sprint ? sprintAcceleration : walkAcceleration), ForceMode.Acceleration);
        else
            rb.AddForce(moveDir.normalized * walkAcceleration * airMoveMultiplier, ForceMode.Acceleration);
    }

    private void ManageSlope()
    {
        float angle = Vector3.Angle(Vector3.up, surfaceInfo.Normal);
        moveDir = Vector3.ProjectOnPlane(moveDir, surfaceInfo.Normal);

        // Is grounded means the slope is within limit
    }

    private void FixedUpdate()
    {
        UpdateGroundedState();
        HandleDrag();
        SpeedLimit();
        ApplyGravity();
        UpdateCrouch();
    }

    private void UpdateCrouch()
    {
        if (!wantsToStandUp)
            return;

        if (!isCrouching)
        {
            wantsToStandUp = false;
            return;
        }

        if(!Physics.CheckSphere(roofcheckPosition.position, roofcheckRadius, whatIsRoof))
        {
            isCrouching = false;
            wantsToStandUp = false;
            onCrouchEnd.Invoke();
        }
    }

    private void ApplyGravity()
    {
        Vector3 gravity = Physics.gravity.y * gravityScale * Vector3.up;
        if (rb.velocity.y < 0)
        {
            rb.AddForce(gravity * fallMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    private void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(isCrouching)
        {
            if(flatVel.magnitude > crouchSpeed)
            {
                Vector3 newVel = flatVel.normalized * crouchSpeed;
                rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
            }

            return;
        }

        if (isSprinting && flatVel.magnitude > maxSprintSpeed)
        {
            Vector3 newVel = flatVel.normalized * maxSprintSpeed;
            rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
        }
        else if (!isSprinting && flatVel.magnitude > maxWalkspeed)
        {
            Vector3 newVel = flatVel.normalized * maxWalkspeed;
            rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
        }
    }

    private void HandleDrag()
    {
        if (surfaceInfo.IsGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    public void UpdateGroundedState()
    {
        bool wasGrounded = surfaceInfo.IsGrounded;

        surfaceInfo.IsGrounded = false;
        surfaceInfo.Collider = null;
      //  surfaceInfo.Normal = Vector3.up;

        if (Physics.CheckSphere(groundcheckPosition.position, groundcheckRadius, whatIsGround))
        {
            surfaceInfo.IsGrounded = true;

            if (Physics.Raycast(groundcheckPosition.position, Vector3.down, out var hit, whatIsGround))
            {
                surfaceInfo.Normal = hit.normal;
                surfaceInfo.Collider = hit.collider;

                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (angle > maxSlopeAngle)
                    surfaceInfo.IsGrounded = false;

            }
            else
            {

                // The raycast failed for some reason            

#if UNITY_EDITOR
                Debug.Log("Raycasting ground failed");
#endif
            }
        }

        if (!wasGrounded && surfaceInfo.IsGrounded)
            onLand.Invoke();
        
    }


    public void Jump()
    {
        //if (!surfaceInfo.IsGrounded)
      //      return;

        if (!readyToJump)
            return;

        readyToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // So that you always jump the same height

       // rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);


        rb.AddForce(surfaceInfo.Normal * jumpForce, ForceMode.VelocityChange);
        // Used lastPlaneNormal so that you cant climb any slope by spamming jumps

        onJump.Invoke();

        Invoke(nameof(ResetJump), jumpCoolDown);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }


    public void StartCrouch()
    {
        if(!isCrouching)
            onCrouchStart.Invoke();

        isCrouching = true;
        wantsToStandUp = false;
    }

    public void EndCrouch()
    {
        wantsToStandUp = true;
    }


}
