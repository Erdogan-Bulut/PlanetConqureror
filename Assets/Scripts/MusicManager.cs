using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Music Settings")]
    [SerializeField] private AudioClip[] musicList;
    private AudioSource audioSource;
    private int lastSongIndex = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (instance == this)
        {
            PlayRandomMusic();
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }

    private void PlayRandomMusic()
    {
        if (musicList.Length <= 1) return;

        int nextSongIndex;
        do
        {
            nextSongIndex = Random.Range(0, musicList.Length);
        } while (nextSongIndex == lastSongIndex);

        lastSongIndex = nextSongIndex;
        audioSource.clip = musicList[nextSongIndex];
        audioSource.Play();
    }
}