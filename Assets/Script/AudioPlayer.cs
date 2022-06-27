using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyAudioType
{
    BGM,
    SE
}

public class AudioPlayer : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying == false) Destroy(gameObject);
    }

    public void Play(AudioClip audio, MyAudioType audioType, float volume,bool isLoop = false)
    {
        audioSource = GetComponent<AudioSource>();
        AudioManager audioManager = AudioManager.GetInstance();

        float masterVolume = audioType == MyAudioType.SE ? audioManager.GetSEMasterVolume() : audioManager.GetBGMMasterVolume();
        audioSource.volume = volume * masterVolume;
        audioSource.loop = isLoop;
        audioSource.clip = audio;
        audioSource.Play();
    }
}
