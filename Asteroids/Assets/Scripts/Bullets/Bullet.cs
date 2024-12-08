using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public static event UnityAction<Bullet> OnDestroyed;

    public float MoveSpeed => _moveSpeed;

    [SerializeField] private float _moveSpeed = 500f;

    [SerializeField] private float _lifeTime = 2f;

    public float LifeTime => _lifeTime;

    private Rigidbody2D _rigitbody;

    private void Awake()
    {
        _rigitbody = GetComponent<Rigidbody2D>(); 
    }

    public void Project(Vector2 direction)
    {
        _rigitbody.AddForce(direction.normalized * _moveSpeed);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != gameObject.layer && collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            OnDestroyed?.Invoke(this);
    }
}
