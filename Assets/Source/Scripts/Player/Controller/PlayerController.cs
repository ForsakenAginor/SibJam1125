using Assets.Source.Scripts.Utility;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _mask;
    [SerializeField] private Flashlight _flashlight;

    private Vector3 _maskOffset = new Vector3(0, -0.5f, 0.32f);

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float maxSpeedKoef = 0.5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Look Settings")]
    [SerializeField] private Transform _cameraAnchor;
    [SerializeField] private float verticalSensitivity = 2f;
    [SerializeField] private float horizontalSensitivity = 10f;
    [SerializeField] private float maxLookAngle = 80f;

    private CharacterController _characterController;
    private Camera _playerCamera;
    private IPlayerInput _input;

    private Vector3 _velocity;
    private float _xRotation = 0f;
    private bool _isGrounded;

    [Inject]
    public void Construct(IPlayerInput playerInput, IGameSettings gameSettings)
    {
        _input = playerInput;
        walkSpeed = gameSettings.WalkSpeed;
        sprintSpeed = gameSettings.SprintSpeed;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerCamera = Camera.main;
        _playerCamera.transform.SetParent(_cameraAnchor);
        _playerCamera.transform.localPosition = Vector3.zero;
        _playerCamera.transform.rotation = Quaternion.identity;

        _mask.SetParent(_playerCamera.transform);
        _mask.localPosition = _maskOffset;

        _flashlight = _mask.GetComponentInChildren<Flashlight>();
    }

    private void OnEnable()
    {
        _input.OnLook += OnLook;
        _input.OnJump += OnJump;
    }

    private void OnDisable()
    {
        _input.OnLook -= OnLook;
        _input.OnJump -= OnJump;
    }

    private void Update()
    {
        HandleMovement();
    }

    public void SetXSens(float x) => horizontalSensitivity = x;

    public void SetYSens(float y) => verticalSensitivity = y;

    private void HandleMovement()
    {
        // Проверка земли
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Небольшая сила прижимающая к земле
        }

        // Получение ввода движения
        Vector2 moveInput = _input.GetMoveInput();
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Выбор скорости (ходьба/бег)
        float currentSpeed = _input.IsSprinted ? sprintSpeed : walkSpeed;
        if (_flashlight.IsRecharge)
            currentSpeed *= maxSpeedKoef;

        // Применение движения
        _characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Гравитация
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void OnJump()
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void OnLook(Vector2 lookInput)
    {

        float x = Mathf.Clamp(lookInput.x, -1f, 1f);
        float y = Mathf.Clamp(lookInput.y, -1f, 1f);
        _xRotation -= y * verticalSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -maxLookAngle, maxLookAngle);
        _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * x * horizontalSensitivity);
    }
}
