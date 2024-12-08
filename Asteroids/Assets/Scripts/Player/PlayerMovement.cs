using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 200f;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    private float _targetRotation = 0f;

    public static event System.Action<bool> OnMoving;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleInput()
    {
        Vector2 input = new Vector2(_joystick.Direction.x, _joystick.Direction.y);

        if (input.sqrMagnitude > 0.1f)
        {
            OnMoving?.Invoke(true);
            _moveDirection = input.normalized;
        }
        else
        {
            OnMoving?.Invoke(false);
            _moveDirection = Vector2.zero;
        }

        if (_moveDirection != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg - 90f;
        }
    }

    private void ApplyMovement()
    {
        if (_moveDirection != Vector2.zero)
        {
            _rigidbody.AddForce(_moveDirection * _moveSpeed);
        }

        if (_moveDirection != Vector2.zero)
        {
            float angle = Mathf.MoveTowardsAngle(_rigidbody.rotation, _targetRotation, _turnSpeed * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(angle);
        }
    }
}
