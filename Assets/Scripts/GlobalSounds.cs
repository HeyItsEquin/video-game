using UnityEngine;

public class GlobalSounds : MonoBehaviour
{
    // UI Sounds
    [SerializeField] public AudioSource MenuButtonClick;
    [SerializeField] public AudioSource MenuButtonHover;
    [SerializeField] public AudioSource AmbientWind;
    float globalVolume;
    public AudioSource jumpSound;

    private void Start()
    {
        globalVolume = SaveManager.Instance.Get(Opts.Volume);
        Opts.Volume.Changed += (newVol) => globalVolume = newVol;
    }
    public void PlaySound(AudioSource source, float pitch = 1.0f, float volume = 1.0f, bool loop = false)
    {
        if (!source)
        {
            Debug.LogError("Attempted to play null AudioSource");
        }

        source.loop = loop;
        source.pitch = pitch;
        source.volume = volume * (globalVolume/100);
        source.Play();
    }

    public void PlayRandomized(AudioSource source,
                                float min, float max,
                                bool randomizeVolume = true, bool randomizePitch = true)
    {
        var volume = randomizeVolume ? Random.Range(min , max) : 1.0f;
        var pitch = randomizePitch ? Random.Range(min, max) : 1.0f;

        PlaySound(source, pitch, volume * (globalVolume / 100));
    }

    public static GlobalSounds GetGlobalSounds()
    {
        GlobalSounds globalS = FindAnyObjectByType<GlobalSounds>();
        if (!globalS) Debug.LogError("Failed to find any GlobalSounds object");
        return globalS;
    }
}
