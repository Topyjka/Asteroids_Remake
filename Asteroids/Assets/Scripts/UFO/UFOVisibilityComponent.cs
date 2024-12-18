using UnityEngine;
using UnityEngine.Events;

public class UFOVisibilityComponent : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;

    public static event UnityAction<UFO, UFOVisibilityComponent> OnVisible;

    public bool IsVisible {  get; private set; }

    private UFO _ufo;

    public void Initialize(UFO ufo)
    {
        IsVisible = false;
        _ufo = ufo;
    }

    private void OnBecameVisible()
    {
        IsVisible = true;
        OnVisible?.Invoke(_ufo, this);
    }

    private void OnBecameInvisible()
    {
        IsVisible = false;
        OnVisible?.Invoke(_ufo, this);
    }

    public void Explode()
    {
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        IsVisible = false;
    }
}
