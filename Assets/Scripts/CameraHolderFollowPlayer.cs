using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolderFollowPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Vector3 offest = Vector3.zero;

    [Header("References")]
    [SerializeField]
    private Transform cameraPosition;
    [SerializeField]
    private Transform crouchCameraPosition;
    [SerializeField]
    private ControllableCharacter player;

    Transform posToFollow;

    private void Awake()
    {
        posToFollow = cameraPosition;
    }
    private void Start()
    {
        player.onCrouchStart.AddListener(() =>
        {
            posToFollow = crouchCameraPosition;
        });

        player.onCrouchEnd.AddListener(() =>
        {
            posToFollow = cameraPosition;
        });
    }
    private void LateUpdate()
    {
        transform.position = posToFollow.position + offest;
    }
}
