using UnityEngine;
using UnityEngine.Events;

public class UFOShootingComponent : MonoBehaviour
{
    [SerializeField] private float _timeBetweenShots = 2f;
    [SerializeField] private GameObject _bulletPrefab;

    public static event UnityAction OnShoting;

    private ObjectPool<Bullet> _bulletPool;
    private Transform _player;
    private float _timeSinceLastShot;

    public void Initialize()
    {
        _bulletPool = GameObject.FindGameObjectWithTag("EnemyBulletPool").GetComponent<BulletPool>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TryShoot()
    {
        _timeSinceLastShot += Time.deltaTime;

        if (_timeSinceLastShot < _timeBetweenShots || _player == null) return;

        Bullet bullet = _bulletPool.GetObject();

        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            Vector2 directionToPlayer = (_player.position - transform.position).normalized;
            bullet.Project(directionToPlayer);
            OnShoting?.Invoke();
        }

        _timeSinceLastShot = 0f;
    }
}
