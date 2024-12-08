using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Vector2 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    public void Respawn()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
    }
}
