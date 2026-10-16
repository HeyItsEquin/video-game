using UnityEngine;

public class GlobalSounds : MonoBehaviour
{
    // UI Sounds
    [SerializeField] public AudioSource MenuButtonClick;
    [SerializeField] public AudioSource MenuButtonHover;
    [SerializeField] public AudioSource AmbientWind;

    public void PlaySound(AudioSource source, float pitch = 1.0f, float volume = 1.0f, bool loop = false)
    {
        if (!source)
        {
            Debug.LogError("Attempted to play null AudioSource");
        }

        source.loop = loop;
        source.pitch = pitch;
        source.volume = volume;
        source.Play();
    }

    public void PlayRandomized(AudioSource source,
                                float min, float max,
                                bool randomizeVolume = true, bool randomizePitch = true)
    {
        if (!source)
        {
            Debug.LogError("Attempted to play null AudioSource");
        }

        if (randomizeVolume) source.volume = Random.Range(min, max);
        if (randomizePitch) source.volume = Random.Range(min, max);
        source.Play();
    }

    public static GlobalSounds GetGlobalSounds()
    {
        GlobalSounds globalS = FindAnyObjectByType<GlobalSounds>();
        if (!globalS) Debug.LogError("Failed to find any GlobalSounds object");
        return globalS;
    }
}
