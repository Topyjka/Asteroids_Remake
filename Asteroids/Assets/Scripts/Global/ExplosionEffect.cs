using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private float _lifetime;

    private ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = _particleSystem.main;
        _lifetime = mainModule.startLifetime.constantMax;
        Destroy(gameObject, _lifetime);
    }
}
