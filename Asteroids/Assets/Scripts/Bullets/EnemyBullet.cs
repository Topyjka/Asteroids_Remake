using UnityEngine;

public class EnemyBullet : Bullet, IAttacker
{
    public DamageType DamageType => DamageType.UFOBullet;
}
