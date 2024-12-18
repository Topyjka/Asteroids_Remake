using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

public class UFO : DamageableObject, IAttacker
{
    private UFOMovementComponent _movement;
    private UFOShootingComponent _shooting;
    private UFOVisibilityComponent _visibility;

    public static event UnityAction OnDead;

    public float Size { get; private set; }

    public DamageType DamageType => DamageType.UFO;

    private void Awake()
    {
        _movement = GetComponent<UFOMovementComponent>();
        _shooting = GetComponent<UFOShootingComponent>();
        _visibility = GetComponent<UFOVisibilityComponent>();

        _movement.Initialize();
        _shooting.Initialize();
        _visibility.Initialize(this);
    }

    private void Update()
    {
        _movement.UpdateMovement();
        _shooting.TryShoot();
    }
    public void ResetSize(float size)
    {
        Size = size;
        transform.localScale = Vector3.one * Size;
    }

    public override void TakeDamage(IAttacker attacker)
    {
        if (attacker.DamageType != DamageType.PlayerBullet)
            return;

        _visibility.Explode();
        OnDead?.Invoke();
    }
}