using UnityEngine;

public abstract class DamageableObject : MonoBehaviour, IDamageable
{
    public abstract void TakeDamage(IAttacker attacker);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var attacker = collision.collider.GetComponent<IAttacker>();

        if (attacker != null)
            TakeDamage(attacker);
    }
}
