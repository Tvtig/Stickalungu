using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private const string INPUT_A = "A";
    private const string INPUT_B = "B";
    private const string INPUT_X = "X";
    private const string INPUT_Y = "Y";
    private const string INPUT_LB = "LB";
    private const string INPUT_RB = "RB";

    private bool _jumpPressed = false;
    private bool _heavyAttackPressed = false;
    private bool _lightAttackPressed = false;

    void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        Set_JumpPressedState();
        Set_HeavyAttackPressedState();
        Set_LightAttackPressedState();
    }

    private void Set_JumpPressedState()
    {
        if(Input.GetAxisRaw(INPUT_A) > 0)
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

    private void Set_HeavyAttackPressedState()
    {
        if (Input.GetAxisRaw(INPUT_B) > 0)
        {
            if (!_heavyAttackPressed)
            {
                _heavyAttackPressed = true;
            }
        }
        else
        {
            _heavyAttackPressed = false;
        }
    }


    private void Set_LightAttackPressedState()
    {
        if (Input.GetAxisRaw(INPUT_X) > 0)
        {
            if (!_lightAttackPressed)
            {
                _lightAttackPressed = true;
            }
        }
        else
        {
            _lightAttackPressed = false;
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
        return Input.GetAxisRaw(INPUT_RB) > 0;
    }

    public bool Input_GetHeavyAttack()
    {
        return _heavyAttackPressed;
    }

    public bool Input_GetLightAttack()
    {
        return _lightAttackPressed;
    }

    public bool Input_GetJump()
    {
        return _jumpPressed;
    }

    public bool Input_GetFighting()
    {
        return Input.GetAxisRaw(INPUT_LB) > 0;
    }
}
