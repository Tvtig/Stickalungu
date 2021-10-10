using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    private readonly string ANIMATOR_WALKING = "Walking";
    private readonly string ANIMATOR_RUNNING = "Running";
    private readonly string ANIMATOR_JUMP = "Jump";
    private readonly string ANIMATOR_FIGHTING = "Fighting";
    private readonly string ANIMATOR_HEAVY_PUNCH_01 = "HeavyPunch_01";
    private readonly string ANIMATOR_HEAVY_PUNCH_02 = "HeavyPunch_02";
    private readonly string ANIMATOR_LIGHT_PUNCH_01 = "LightPunch_01";

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
    private float _attackingSpeedModifier = 0.3f;
    [SerializeField]
    private float _turnSharpness = 10f;
    [SerializeField]
    private float _accelerateion = 10f;
    [SerializeField]
    private float _inAirAcceleration = 10f;
    [SerializeField]
    private float _clampToGroundDistance = 0.07f;
    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private float _punchSoundDelay = 0.4f;
    [SerializeField]
    private Transform _leftHandPosition;
    [SerializeField]
    private Transform _rightHandPosition;
    [SerializeField]
    private GameObject _punchEffect;
   
    private PlayerInputHandler _playerInputHandler;
    private Animator _animator;
    private CharacterController _characterController;
    private AudioHandler _playerAudioHandler;
    private Vector3 _playerVelocity;

    private bool _playerIsWalking;
    private bool _playerIsSprinting;
    private float _playerLastJumpTime;
    private float _playerSnapToGroundTimeAllowance = 0.1f;
    private bool _jumped;
    private bool _heavyAttack;
    private bool _lightAttack;

    [SerializeField]
    private bool _isAttacking = false;

    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _playerAudioHandler = GetComponent<AudioHandler>();
    }

    void Update()
    {
        Animator_ResetTriggers();
        Player_UpdateMovementStates();
        Player_UpdateCombat();
        Player_Move();
        Animator_Update();
        
    }

    private void Player_UpdateCombat()
    {
        if (_playerInputHandler.Input_GetHeavyAttack())
        {
            _heavyAttack = true;
        }
        else
        {
            _heavyAttack = false;
        }

        if (_playerInputHandler.Input_GetLightAttack())
        {
            _lightAttack = true;
        }
        else
        {
            _lightAttack = false;
        }
    }

    private void Player_Move()
    {
        _jumped = false;

        float speedModifier = 1f;

        if (_isAttacking)
        {
            speedModifier = _attackingSpeedModifier;
        }
        else if (_playerIsSprinting)
        {
            speedModifier = _sprintSpeedModifier;
        }

        float horizontalInput = _playerInputHandler.Input_GetHorizontal();
        float verticalInput = _playerInputHandler.Input_GetVertical();

        if (_characterController.isGrounded)
        {
            if (horizontalInput + verticalInput != 0)
            {
                transform.localEulerAngles = new Vector3(0, (Mathf.Atan2(_playerInputHandler.Input_GetHorizontal(), _playerInputHandler.Input_GetVertical()) * (180) / Mathf.PI), 0f);
                Vector3 targetVelocity = transform.forward * speedModifier * _maxMovementSpeed;
                _playerVelocity = Vector3.Lerp(_playerVelocity, targetVelocity, _accelerateion * Time.deltaTime);
                _playerVelocity += Vector3.down;
            }
            else
            {
                _playerVelocity = Vector3.down;
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
            _playerVelocity += transform.forward * _inAirAcceleration * Time.deltaTime;
            float verticalVelocity = _playerVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(_playerVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, _maxInAirMovementSpeed * speedModifier);
            _playerVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
            _playerVelocity += Vector3.down * _fallSpeed * Time.deltaTime;
        }

        //No input, zero out player velocity
        //if (horizontalInput + verticalInput == 0)
        //{
        //    _playerVelocity = Vector3.zero;
        //}

        if (!_characterController.isGrounded && transform.position.y <= _clampToGroundDistance && (Time.deltaTime - _playerLastJumpTime) > _playerSnapToGroundTimeAllowance)
        {
            _playerVelocity = Vector3.zero;
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

        if (_heavyAttack)
        {
            int random = Random.Range(1, 2);

            if(random == 0)
            {
                _animator.SetTrigger(ANIMATOR_HEAVY_PUNCH_01);
            }
            else
            {
                _animator.SetTrigger(ANIMATOR_HEAVY_PUNCH_02);
            }
            
            _heavyAttack = false;
        }

        if(_lightAttack)
        {
            _animator.SetTrigger(ANIMATOR_LIGHT_PUNCH_01);
            _lightAttack = false;
        }
        
    }

    private void Animator_ResetTriggers()
    {
        _animator.ResetTrigger(ANIMATOR_LIGHT_PUNCH_01);
        _animator.ResetTrigger(ANIMATOR_HEAVY_PUNCH_01);
        _animator.ResetTrigger(ANIMATOR_HEAVY_PUNCH_02);
    }

    private void Player_UpdateMovementStates()
    {
        bool sprinting = false;
        bool walking = false;

        if (_playerInputHandler.Input_GetSprint())
        {
            if(_playerInputHandler.Input_GetVertical() != 0 || _playerInputHandler.Input_GetHorizontal() != 0)
            {
                sprinting = true;
            }
        }

        if (_playerInputHandler.Input_GetVertical() != 0 || _playerInputHandler.Input_GetHorizontal() != 0)
        {
            walking = true;
        }

        if (sprinting)
        {
            _playerIsWalking = false;
            _playerIsSprinting = true;
        }
        else if (walking)
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

    private void OnTriggerEnter(Collider other)
    {
        Vector3 position = other.ClosestPointOnBounds(_rightHandPosition.position);
        GameObject pe = Instantiate(_punchEffect, position, Quaternion.identity);
        _playerAudioHandler.Sound_Play(PlayerSound.Thud_02, 0f);
        Destroy(pe, 0.5f);
        Cube cube = other.gameObject.GetComponent<Cube>();
        cube.Damage_Take();
    }
}
