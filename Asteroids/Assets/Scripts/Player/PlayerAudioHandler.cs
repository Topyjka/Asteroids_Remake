using UnityEngine;

public class PlayerAudioHandler : AudioHandlerBase, IAudioHandler
{
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _engineSound;
    [SerializeField] private AudioClip _deadSound;

    private AudioSource _engineSource;
    private AudioSource _source;

    protected override void Awake()
    {
        InitializeAudioSources();
    }

    protected override void InitializeAudioSources()
    {
        _engineSource = CreateAudioSource(_engineSound, false, true);
        _source = CreateAudioSource(null, false);
    }

    private void PlayEngineSound(bool isMoving)
    {
        if (isMoving)
        {
            StartSound(_engineSource);
        }
        else
        {
            StopSound(_engineSource);
        }
    }

    private void PlayShotSound() => PlaySound(_source, _shootSound);

    private void PlayDeadSound() => PlaySound(_source, _deadSound);

    protected override void OnEnable() => SubscribeToEvents();

    protected override void OnDisable() => UnsubscribeFromEvents();

    public void SubscribeToEvents()
    {
        PlayerHealth.OnDead += PlayDeadSound;
        PlayerMovement.OnMoving += PlayEngineSound;
        PlayerShooting.OnShoting += PlayShotSound;
    }

    public void UnsubscribeFromEvents()
    {
        PlayerHealth.OnDead -= PlayDeadSound;
        PlayerMovement.OnMoving -= PlayEngineSound;
        PlayerShooting.OnShoting -= PlayShotSound;
    }
}
