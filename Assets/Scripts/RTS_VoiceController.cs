using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class RTS_VoiceController : MonoBehaviour
{
    public AudioClip ConstructionComplete;
    public AudioClip BaseUnderAttack;
    public AudioClip LowFunds;
    public AudioClip LowPower;
    public AudioClip SilosNeeded;
    public AudioClip UnitReady;
    public AudioClip UnitUnderAttack;
    AudioSource audioSource;
    List<AudioClip> ClipsToPlay = new List<AudioClip>();

    public static RTS_VoiceController temp;

    void Start()
    {
        GetAudioSource();
        temp = this;
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
        if (ClipsToPlay.Count > 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = ClipsToPlay[0];
                audioSource.Play();
                ClipsToPlay.RemoveAt(0);
            }
        }
    }

    public void playVoice(string voice)
    {
        if (voice == "constructionComplete")
        {
            ClipsToPlay.Add(ConstructionComplete);
        }
        else if (voice == "baseUnderAttack")
        {
            ClipsToPlay.Add(BaseUnderAttack);
        }
        else if (voice == "lowFunds")
        {
            ClipsToPlay.Add(LowFunds);
        }
        else if (voice == "lowPower")
        {
            ClipsToPlay.Add(LowPower);
        }
        else if (voice == "silosNeeded")
        {
            ClipsToPlay.Add(SilosNeeded);
        }
        else if (voice == "unitReady")
        {
            ClipsToPlay.Add(UnitReady);
        }
        else
        {
            ClipsToPlay.Add(UnitUnderAttack);
        }
    }
}
