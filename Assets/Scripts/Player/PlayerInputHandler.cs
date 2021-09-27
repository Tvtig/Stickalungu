using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private const string AXIS_FIRE1 = "Fire3";
    private const string AXIS_JUMP = "Jump";

    private bool _jumpPressed = false;

    void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        Set_JumpPressedState();
    }

    private void Set_JumpPressedState()
    {
        if(Input.GetAxisRaw(AXIS_JUMP) > 0)
        {
            if (!_jumpPressed)
            {
                _jumpPressed = true;
            }
        }
        else
        {
            _jumpPressed = false;
        }
    }

    public float Input_GetHorizontal()
    {
        return Input.GetAxis(AXIS_HORIZONTAL);
    }

    public float Input_GetVertical()
    {
        return Input.GetAxis(AXIS_VERTICAL);
    }

    public bool Input_GetSprint()
    {
        return Input.GetAxisRaw(AXIS_FIRE1) > 0;
    }

    public bool Input_GetJump()
    {
        return _jumpPressed;
    }
}
