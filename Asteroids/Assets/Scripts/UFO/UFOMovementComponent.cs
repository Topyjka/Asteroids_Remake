using UnityEngine;

public class UFOMovementComponent : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _verticalBoundaryPadding = 0.1f;
    private Vector2 _direction;

    public void Initialize()
    {
        _direction = transform.position.x < 0 ? Vector2.right : Vector2.left;
    }

    public void UpdateMovement()
    {
        Vector2 movement = _direction * _speed * Time.deltaTime;
        transform.Translate(movement);

        float adjustedTop = GetScreenTop() - _verticalBoundaryPadding;
        float adjustedBottom = GetScreenBottom() + _verticalBoundaryPadding;

        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, adjustedBottom, adjustedTop);
        transform.position = position;
    }

    private float GetScreenTop() => Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    private float GetScreenBottom() => Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
}
