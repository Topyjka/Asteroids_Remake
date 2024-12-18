using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : DamageableObject
{
    [SerializeField] private int _totalLives = 3;
    [SerializeField] private Image[] _lifeIcons;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _invulnerabilityDuration = 3f;

    private int _currentLives;
    private Vector2 _startPosition;
    private bool _isInvulnerable = false;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    public static event System.Action OnDead;
    public static event System.Action OnLivesDepleted;

    private void Awake()
    {
        _currentLives = _totalLives;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _startPosition = transform.position;

        UpdateLifeUI();
    }

    private void UpdateLifeUI()
    {
        for (int i = 0; i < _lifeIcons.Length; i++)
        {
            _lifeIcons[i].enabled = i < _currentLives;
        }
    }

    private void Die(bool isLivesDepleted)
    {

        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        }

        if (isLivesDepleted)
        {
            OnLivesDepleted?.Invoke();
        }
        else
        {
            Respawn();
        }

        OnDead?.Invoke();
    }

    private void Respawn()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        StartCoroutine(ActivateInvulnerability());
    }

    private IEnumerator ActivateInvulnerability()
    {
        Color originalColor = _spriteRenderer.color;
        _isInvulnerable = true;
        _collider.enabled = false;
        float blinkTime = 0f;

        while (blinkTime < _invulnerabilityDuration)
        {
            float lerpValue = Mathf.PingPong(Time.time * 5, 1);
            _spriteRenderer.color = Color.Lerp(Color.white, Color.black, lerpValue);

            blinkTime += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = originalColor;
        _collider.enabled = true;
        _isInvulnerable = false;
    }

    public override void TakeDamage(IAttacker attacker)
    {
        if (attacker.DamageType != DamageType.Asteroid &&
            attacker.DamageType != DamageType.UFO &&
            attacker.DamageType != DamageType.UFOBullet)
            return;

        if (_isInvulnerable)
            return;

        _currentLives--;
        UpdateLifeUI();

        Die(_currentLives <= 0);
    }
}