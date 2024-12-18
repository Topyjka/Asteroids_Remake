using UnityEngine;

public class PlayerBullet : Bullet, IAttacker
{
    public DamageType DamageType => DamageType.PlayerBullet;
}
