using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : DamageableObject, IAttacker
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private GameObject _explosionEffect;

    public static event UnityAction<Asteroid> OnObjectDestroyed;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    public DamageType DamageType => DamageType.Asteroid;

    public float Size { get; private set; }  = 1f;
    public float Small { get; } = 1f;
    public float Medium { get; } = 1.5f;
    public float Large { get; } = 2f;
    public float MoveSpeed => _moveSpeed;

    public void ResetSize(float size)
    {
        Size = size; 
        transform.localScale = Vector3.one * Size;
        _rigidbody.mass = Size;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        transform.localScale = Vector3.one * Size;
        _rigidbody.mass = Size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction.normalized * _moveSpeed);
    }

    public override void TakeDamage(IAttacker attacker)
    {
        if (attacker.DamageType == DamageType.PlayerBullet)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            OnObjectDestroyed?.Invoke(this);
        }
    }
}
