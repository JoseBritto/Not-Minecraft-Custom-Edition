using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ControllableCharacter))]
public class PlayerMovementInput : MonoBehaviour
{
    const string JUMP_BUTTON_NAME = "Jump";
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";
    const string SPRINT_BUTTON_NAME = "Sprint";
    const string CROUCH_BUTTON_NAME = "Crouch";

    float hInput;
    float vInput;

    private ControllableCharacter character;


    private void Awake()
    {
        character = GetComponent<ControllableCharacter>();
    }

    private void GetInput()
    {
        hInput = Input.GetAxisRaw(HORIZONTAL_AXIS);
        vInput = Input.GetAxisRaw(VERTICAL_AXIS);
    }

    private void Update()
    {
        GetInput();

        if (Input.GetButton(JUMP_BUTTON_NAME))
            character.Jump();

        if (Input.GetButtonDown(CROUCH_BUTTON_NAME))
            character.StartCrouch();

        if (Input.GetButtonUp(CROUCH_BUTTON_NAME))
            character.EndCrouch();
    }


    private void FixedUpdate()
    {
        character.MovePlayer(hInput, vInput, Input.GetButton(SPRINT_BUTTON_NAME));
    }
}
