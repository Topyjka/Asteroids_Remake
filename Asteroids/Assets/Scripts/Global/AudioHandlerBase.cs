using UnityEngine;

public abstract class AudioHandlerBase : MonoBehaviour
{
    protected AudioSource CreateAudioSource(AudioClip clip,bool playOnAwake, bool loop = false)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = playOnAwake;
        audioSource.loop = loop;
        audioSource.clip = clip;
        return audioSource;
    }

    protected abstract void InitializeAudioSources();

    protected abstract void OnEnable();

    protected abstract void OnDisable();

    protected abstract void Awake();

    protected virtual void PlaySound(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.PlayOneShot(clip);
        }
    }

    protected virtual void StartSound(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    protected virtual void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
