using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    public AudioClip Loose;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    void Start()
    {
        PlayMusic(menuMusic);
    }

    public void PlayOnce(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.loop = false;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}