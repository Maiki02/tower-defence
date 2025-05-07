using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [Header("Fuentes de Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips de Sonido (orden según SoundType)")]
    [SerializeField] private AudioClip[] soundClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlaySFX(SoundType type)
    {
        AudioClip clip = soundClips[(int)type];
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(SoundType type, bool loop = true)
    {

        AudioClip clip = soundClips[(int)type];
        if (clip != null){ 
            PlayMusic(clip, loop);
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
 
    //Detectamos el cambio de escena para poner la música adecuada
    private void OnEnable()  {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
     SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Escena cargada: " + scene.name);
        if (scene.name == "Game")
            PlayMusic(SoundType.StartScene);
    }

}
