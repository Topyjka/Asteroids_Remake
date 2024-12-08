using UnityEngine;

public class AsteroidsAudioHandler : AudioHandlerBase, IAudioHandler
{
    [SerializeField] private AudioClip _smallBangSound;
    [SerializeField] private AudioClip _mediumBangSound;
    [SerializeField] private AudioClip _largeBangSound;

    private AudioSource _audioSource;

    protected override void Awake()
    {
        InitializeAudioSources();
    }

    private void PlayBangSound(Asteroid asteroid)
    {
        AudioClip clip = 
                        asteroid.Size > asteroid.Medium ? _largeBangSound : 
                        asteroid.Size > asteroid.Small ? _mediumBangSound :
                        _smallBangSound;

        PlaySound(_audioSource, clip);
    }

    protected override void OnEnable() => SubscribeToEvents();

    protected override void OnDisable() => UnsubscribeFromEvents();

    protected override void InitializeAudioSources()
    {
        _audioSource = CreateAudioSource(null, false);
    }

    public void SubscribeToEvents()
    {
        Asteroid.OnObjectDestroyed += PlayBangSound;
    }

    public void UnsubscribeFromEvents()
    {
        Asteroid.OnObjectDestroyed -= PlayBangSound;
    }
}