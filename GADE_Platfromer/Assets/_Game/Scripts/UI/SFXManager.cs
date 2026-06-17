using UnityEngine;

[System.Serializable]
public struct SoundEntry
{
    public string soundName;
    public AudioClip clip;
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Background Music")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    [Header("Sound Library (loaded into CustomHashMap on Awake)")]
    public SoundEntry[] soundLibrary;

    private CustomHashMap<string, AudioClip> sfxMap;

    private AudioSource audioSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;

        }

        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        sfxMap = new CustomHashMap<string, AudioClip>(50);

        foreach (SoundEntry entry in soundLibrary)
        {
            if (entry.clip != null && !string.IsNullOrEmpty(entry.soundName))
            {
                sfxMap.Put(entry.soundName, entry.clip);
            }
        }
    }
    private void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void PlaySFX(string soundName)
    {
        AudioClip clipToPlay = sfxMap.Get(soundName);

        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.LogWarning("SFXManager: Sound not found -> " + soundName);
        }
    }
}


