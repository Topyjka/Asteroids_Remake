using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

public class UFO : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _randomMovementIntensity = 1f;
    [SerializeField] private float _timeBetweenShots = 2f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _explosionEffect; 
    [SerializeField] private float _verticalBoundaryPadding = 0.1f; // ���������� ����������� ������ � �����


    public float Size { get; private set; } = 1f;
    public bool IsVisible { get; private set; }

    private Transform _player;
    private ObjectPool<Bullet> _bulletPool;
    private Vector2 _direction;
    private float _timeSinceLastShot = 0f;
    private float _timeToChangeDirection; // ����� �� ���������� ��������� �����������
    private float _currentVerticalDirection; // ������� ����������� �� ��� Y


    public static event UnityAction OnShoting;
    public static event UnityAction OnDead;
    public static event UnityAction<UFO> OnVisible;

    private void Awake()
    {
        _bulletPool = GameObject.FindGameObjectWithTag("EnemyBulletPool").GetComponent<ObjectPool<Bullet>>();

    }

    private void Start()
    {
        if (transform.position.x < 0)
        {
            _direction = Vector2.right;
        }
        else
        {
            _direction = Vector2.left;
        }

        _timeToChangeDirection = Random.Range(2f, 5f); // ��������� ����� �� ������� ��������� �����������
        _currentVerticalDirection = 0f; // ��������� ������������ ����������
    }

    private void Update()
    {
        MoveUFO();
        HandleVerticalDirectionChange();

        _timeSinceLastShot += Time.deltaTime;

        if (_timeSinceLastShot >= _timeBetweenShots)
        {
            Shoot();
            _timeSinceLastShot = 0f;
        }
    }

    private void MoveUFO()
    {
        // ������� UFO � ���������� �������� ����������� + ��������� ������������ ������������
        Vector2 movement = _direction + new Vector2(0, _currentVerticalDirection);
        Vector3 newPosition = transform.position + (Vector3)(movement * _speed * Time.deltaTime);

        // ������������ ������������ �������� � �������� ������� ������ ������
        float screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        // ��������� ���������� ����������� ������ � �����
        float adjustedTop = screenTop - (screenTop - screenBottom) * _verticalBoundaryPadding;
        float adjustedBottom = screenBottom + (screenTop - screenBottom) * _verticalBoundaryPadding;

        // ������������ ������ ��������
        newPosition.y = Mathf.Clamp(newPosition.y, adjustedBottom, adjustedTop);

        transform.position = newPosition; // ������������� ����� �������
    }

    private void HandleVerticalDirectionChange()
    {
        // ��������� ����� �� ���������� ��������� ����������
        _timeToChangeDirection -= Time.deltaTime;

        // ���� ����� �����, ������ �����������
        if (_timeToChangeDirection <= 0f)
        {
            // ���������� ��������� ����� ����������� (����� ��� ����)
            _currentVerticalDirection = Random.Range(-_randomMovementIntensity, _randomMovementIntensity);

            // ���������� ������ ��� ���������� ��������� �����������
            _timeToChangeDirection = Random.Range(2f, 5f);
        }
    }

    private void Shoot()
    {
        if (IsVisible)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;

            if (_bulletPrefab != null && _player != null)
            {
                Bullet bullet = _bulletPool.GetObject();

                if (bullet != null)
                {
                    bullet.transform.position = transform.position;
                    Vector2 directionToPlayer = (_player.position - transform.position).normalized;
                    bullet.Project(directionToPlayer.normalized);
                    StartCoroutine(ReturnBulletWithDelay(bullet, bullet.LifeTime));
                    OnShoting?.Invoke();
                }
            }
        }
    }

    private void ReturnInPool(Bullet bullet)
    {
        if (bullet != null)
        {
            _bulletPool.ReturnObject(bullet);
        }
    }

    private IEnumerator ReturnBulletWithDelay(Bullet bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bullet != null)
        {
            _bulletPool.ReturnObject(bullet);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            OnDead?.Invoke();
        }
    }

    public void ResetSize(float size)
    {
        Size = size;
        transform.localScale = Vector3.one * Size;
    }

    private void OnBecameVisible()
    {
        IsVisible = true;
        OnVisible?.Invoke(this);
    }

    private void OnBecameInvisible()
    {
        IsVisible = false;
        OnVisible?.Invoke(this);
    }
}