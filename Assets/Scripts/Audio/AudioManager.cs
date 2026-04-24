using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource chirpingSource;
    public AudioSource rainSource;
    [Header("AudioClips")]
    public AudioClip backgroundMusic;
    public AudioClip rainSounds;
    public AudioClip chirpingSounds;

    public AudioClip jump1SFX;
    public AudioClip jump2SFX;
    public AudioClip collectibleSFX;
    public AudioClip step1SFX;
    public AudioClip step2SFX;
    public AudioClip dash1SFX;
    public AudioClip dash2SFX;
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

    public void PlayLoop(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null) return;

        if (source.clip == clip && source.isPlaying) return;

        source.clip = clip;
        source.loop = true;
        source.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.Stop();
        }

        if (chirpingSource != null && chirpingSource.clip != null)
        {
            chirpingSource.Stop();
        }

        if (rainSource != null && rainSource.clip != null)
        {
            rainSource.Stop();
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

    public void PlayRandomDash()
    {
        AudioClip clip = Random.value < 0.5f ? dash1SFX : dash2SFX;
        PlaySFX(clip);
    }

    public void PlayRandomJump()
    {
        AudioClip clip = Random.value < 0.5f ? jump1SFX : jump2SFX;
        PlaySFX(clip);
    }

}
