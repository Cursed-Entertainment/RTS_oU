using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class MusicPlaylist : MonoBehaviour
{
    AudioSource audioSource;
    int SongCounter = 0;
    int NumberOfSongs;
    bool SongPlaying = false;

    public bool PlayMusic = true;
    public AudioClip[] Songs;

    void Start()
    {
        GetAudioSource();
        NumberOfSongs = Songs.Length;
        Application.runInBackground = true;
    }

    void GetAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (PlayMusic)
        {
            if (!SongPlaying)
            {
                audioSource.clip = Songs[SongCounter];
                audioSource.Play();
                SongCounter++;
                if (SongCounter >= NumberOfSongs) SongCounter = 0;
                SongPlaying = true;
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    SongPlaying = false;
                }
            }
        }
    }
}
