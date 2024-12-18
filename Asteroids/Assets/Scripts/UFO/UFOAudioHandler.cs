using UnityEngine;

public class UFOAudioHandler : AudioHandlerBase, IAudioHandler
{
    [SerializeField] private AudioClip _engineSoundSmall;
    [SerializeField] private AudioClip _engineSoundBig;
    [SerializeField] private AudioClip _bangSound;
    [SerializeField] private AudioClip _shotSound;

    private AudioSource _engineSourceSmall;
    private AudioSource _engineSourceBig;
    private AudioSource _audioSource;

    protected override void Awake()
    {
        InitializeAudioSources();
    }

    protected override void OnEnable() => SubscribeToEvents();

    protected override void OnDisable() => UnsubscribeFromEvents();

    private void PlayShootSound() => PlaySound(_audioSource, _shotSound);

    private void PlayDeadSound() => PlaySound(_audioSource, _bangSound);

    private void HandleVisibilityChange(UFO ufo, UFOVisibilityComponent visibilityComponent)
    {
        AudioSource engineSource = ufo.Size > 2 ? _engineSourceBig : _engineSourceSmall;

        if (visibilityComponent.IsVisible)
        {
            StartSound(engineSource);
        }
        else
        {
            StopSound(engineSource);
        }
    }

    protected override void InitializeAudioSources()
    {
        _engineSourceSmall = CreateAudioSource(_engineSoundSmall, false, true);
        _engineSourceBig = CreateAudioSource(_engineSoundBig, false, true);
        _audioSource = CreateAudioSource(null, false);
    }

    public void SubscribeToEvents()
    {
        UFO.OnDead += PlayDeadSound;
        UFOVisibilityComponent.OnVisible += HandleVisibilityChange;
        UFOShootingComponent.OnShoting += PlayShootSound;
    }

    public void UnsubscribeFromEvents()
    {
        UFO.OnDead -= PlayDeadSound;
        UFOVisibilityComponent.OnVisible -= HandleVisibilityChange;
        UFOShootingComponent.OnShoting -= PlayShootSound;
    }
}