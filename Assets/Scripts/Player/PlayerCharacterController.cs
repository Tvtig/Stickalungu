using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private readonly string ANIMATOR_WALKING = "Walking";
    private readonly string ANIMATOR_RUNNING = "Running";
    private readonly string ANIMATOR_JUMP = "Jump";

    [SerializeField]
    private float _maxMovementSpeed = 10f;
    [SerializeField]
    private float _maxInAirMovementSpeed = 25f;
    [SerializeField]
    private float _fallSpeed = 5f;
    [SerializeField]
    private float _jumpVelocity = 5f;
    [SerializeField]
    private float _sprintSpeedModifier = 2f;
    [SerializeField]
    private float _turnSharpness = 10f;
    [SerializeField]
    private float _accelerateion = 10f;
    [SerializeField]
    private float _inAirAcceleration = 10f;
    [SerializeField]
    private float _clampToGroundDistance = 0.07f;
    [SerializeField]
    [Tooltip("The horizontal input is a float between [-1,1], the character will continue to turn as the input becomes closer to zero, this threshold determines how soon the character should stop turning")]
    private float _turnStopThreshhold = 0.5f;
   
    private PlayerInputHandler _playerInputHandler;
    private Animator _animator;
    private CharacterController _characterController;
    private Vector3 _playerVelocity;

    private bool _playerIsWalking;
    private bool _playerIsSprinting;
    private float _playerLastJumpTime;
    private float _playerSnapToGroundTimeAllowance = 0.1f;
    private bool _jumped;

    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Player_UpdateMovementStates();
        Player_Move();
        Animator_Update();
    }

    private void Player_Move()
    {
        _jumped = false;
        float speedModifier = (_playerIsSprinting) ? _sprintSpeedModifier : 1f;

        float horizontalInput = _playerInputHandler.Input_GetHorizontal();
        Vector3 targetHorizontalDirection = Vector3.zero;

        bool rotate = false;

        if (horizontalInput < -(_turnStopThreshhold))
        {
            //Look left
            targetHorizontalDirection = -transform.right;
            rotate = true;
        }
        else if (horizontalInput > _turnStopThreshhold)
        {
            //Look right
            targetHorizontalDirection = transform.right;
            rotate = true;
        }

        if (rotate)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetHorizontalDirection, (_turnSharpness * Time.deltaTime), 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        Vector3 targetVerticalDirection = Vector3.zero;
        float verticalInput = _playerInputHandler.Input_GetVertical();

        Vector3 worldSpaceVerticalMovement = Vector3.zero;

        if (_characterController.isGrounded)
        {
            bool move = false;

            if (verticalInput > 0)
            {
                targetVerticalDirection = Vector3.forward;
                move = true;
            }
            else if(verticalInput < 0)
            {
                targetVerticalDirection = Vector3.back;
                //Half the movement speed if backwards
                speedModifier = speedModifier / 2;
                move = true;
            }

            worldSpaceVerticalMovement = transform.TransformVector(targetVerticalDirection);
            Vector3 targetVelocity = worldSpaceVerticalMovement * _maxMovementSpeed * speedModifier;
            targetVelocity += Vector3.down * _fallSpeed;

            if (move)
            {
                _playerVelocity = Vector3.Lerp(_playerVelocity, targetVelocity, _accelerateion * Time.deltaTime);
            }
            else
            {
                _playerVelocity = Vector3.zero;
            }

            if (_playerInputHandler.Input_GetJump())
            {
                _playerVelocity = new Vector3(_playerVelocity.x, 0f, _playerVelocity.z);
                _playerVelocity += (Vector3.up * _jumpVelocity);
                _playerLastJumpTime = Time.deltaTime;
                _jumped = true;
            }
        }
        else
        {
            _playerVelocity += worldSpaceVerticalMovement * _inAirAcceleration * Time.deltaTime;
            float verticalVelocity = _playerVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(_playerVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, _maxInAirMovementSpeed * speedModifier);
            _playerVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
            _playerVelocity += Vector3.down * _fallSpeed * Time.deltaTime;
        }

        if (!_characterController.isGrounded && transform.position.y <= _clampToGroundDistance && (Time.deltaTime - _playerLastJumpTime) > _playerSnapToGroundTimeAllowance)
        {
            _characterController.SimpleMove(Vector3.down);
        }
        else
        {
            _characterController.Move(_playerVelocity * Time.deltaTime);
        }
    }

    private void Animator_Update()
    {
        _animator.SetBool(ANIMATOR_WALKING, _playerIsWalking);
        _animator.SetBool(ANIMATOR_RUNNING, _playerIsSprinting);
        if (_jumped)
        {
            _animator.SetTrigger(ANIMATOR_JUMP);
        }
        
    }

    private void Player_UpdateMovementStates()
    {
        bool sprinting = false;
        bool walking = false;

        if (_playerInputHandler.Input_GetSprint())
        {
            if(_playerInputHandler.Input_GetVertical() > 0)
            {
                sprinting = true;
            }
        }

        if (_playerInputHandler.Input_GetVertical() != 0)
        {
            walking = true;
        }

        if (sprinting && _characterController.isGrounded)
        {
            _playerIsWalking = false;
            _playerIsSprinting = true;
        }
        else if (walking && _characterController.isGrounded)
        {
            _playerIsWalking = true;
            _playerIsSprinting = false;
        }
        else
        {
            _playerIsWalking = false;
            _playerIsSprinting = false;
        }
    }
}
