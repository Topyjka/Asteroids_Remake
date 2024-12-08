using System.Collections;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _explosionEffect;

    public void ShowExplosion(Vector3 position)
    {
        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, position, Quaternion.identity);
        }
    }

    public IEnumerator ActivateInvulnerability(float duration)
    {
        Color originalColor = _spriteRenderer.color;
        float blinkTime = 0f;

        while (blinkTime < duration)
        {
            float lerpValue = Mathf.PingPong(Time.time * 5, 1);
            _spriteRenderer.color = Color.Lerp(Color.white, Color.black, lerpValue);

            blinkTime += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = originalColor;
    }
}
