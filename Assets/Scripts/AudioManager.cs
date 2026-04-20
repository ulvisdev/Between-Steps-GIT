using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    [Header("AudioClips")]
    public AudioClip backgroundMusic;
    public AudioClip jumpSFX;
    public AudioClip collectibleSFX;
    public AudioClip step1SFX;
    public AudioClip step2SFX;
    public AudioClip dashSFX;
    public AudioClip deathSFX;


        void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // if (backgroundMusic != null && musicSource != null)
        // {
        //     musicSource.clip = backgroundMusic;
        //     musicSource.loop = true;
        //     musicSource.Play();
        // }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.Stop();
        }
    }

    public void RestartMusic()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.Stop();
            musicSource.Play();
        }
    }
}
