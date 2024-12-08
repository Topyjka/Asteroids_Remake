using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    public static event UnityAction OnShoting;

    [SerializeField] private ObjectPool<Bullet> _bulletPool;

    private void Shoot()
    {
        Bullet bullet = _bulletPool.GetObject();
        bullet.transform.position = transform.position;
        bullet.Project(transform.up.normalized);

        StartCoroutine(ReturnBulletWithDelay(bullet, bullet.LifeTime));
        OnShoting?.Invoke();
    }

    public void OnPointerClick()
    {
        Shoot();
    }

    private IEnumerator ReturnBulletWithDelay(Bullet bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bullet != null)
        {
            _bulletPool.ReturnObject(bullet);
        }
    }

    private void OnEnable()
    {
        Bullet.OnDestroyed += ReturnInPool;
    }

    private void OnDisable()
    {
        Bullet.OnDestroyed -= ReturnInPool;
    }

    private void ReturnInPool(Bullet bullet)
    {
        if (bullet != null)
        {
            _bulletPool.ReturnObject(bullet);
        }
    }
}
