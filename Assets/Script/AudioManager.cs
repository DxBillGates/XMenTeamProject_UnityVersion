using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonComponent<AudioManager>
{
    private const int MAX_VOLEME_LEVEL = 10;

    [SerializeField,Range(0,1)] private float defaultVolume; 

    [SerializeField,Range(0,MAX_VOLEME_LEVEL)] private int bgmMasterVolumeLevel;
    private int beforeBGMMasterVolumeLevel;

    [SerializeField,Range(0,MAX_VOLEME_LEVEL)] private int seMasterVolumeLevel;
    private int beforeSEMasterVolumeLevel;

    [SerializeField] private GameObject prefabAudioPlayer;
    [SerializeField] private GameObject bgmPlayer;
    [SerializeField] private AudioClip testSE;

    // Start is called before the first frame update
    void Start()
    {
        beforeBGMMasterVolumeLevel = bgmMasterVolumeLevel;
        beforeSEMasterVolumeLevel = seMasterVolumeLevel;


        var bgm = bgmPlayer.GetComponent<AudioSource>();
        bgm.volume = GetBGMMasterVolume();
        bgm.loop = true;
        bgm.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(beforeBGMMasterVolumeLevel != bgmMasterVolumeLevel)
        {
            bgmPlayer.GetComponent<AudioSource>().volume = GetBGMMasterVolume();
        }

        // se�̉��ʂ��ς������OneShot�ōĐ�
        if (beforeSEMasterVolumeLevel != seMasterVolumeLevel)
        {
            PlayAudio(testSE, MyAudioType.SE, 1, false);
        }

        beforeBGMMasterVolumeLevel = bgmMasterVolumeLevel;
        beforeSEMasterVolumeLevel = seMasterVolumeLevel;
    }

    // bgm�̃}�X�^�[���ʂ�0 ~ 1�ŕԂ�
    public float GetBGMMasterVolume()
    {
        return defaultVolume * ((float)bgmMasterVolumeLevel / MAX_VOLEME_LEVEL);
    }

    // bgm�̃}�X�^�[���ʃ��x����Ԃ�
    public int GetBGMMasterVolumeLevel()
    {
        return bgmMasterVolumeLevel;
    }

    // se�̃}�X�^�[���ʂ�0 ~ 1�ŕԂ�
    public float GetSEMasterVolume()
    {
        return defaultVolume * ((float)seMasterVolumeLevel / MAX_VOLEME_LEVEL);
    }

    // se�̃}�X�^�[���ʃ��x����Ԃ�
    public int GetSEMasterVolumeLevel()
    {
        return seMasterVolumeLevel;
    }

    public int GetMaxMasterVolume()
    {
        return MAX_VOLEME_LEVEL;
    }

    public float GetDefaultVolume()
    {
        return defaultVolume;
    }

    public void IncreaseBGMMasterVolumeLevel(int value)
    {
        bgmMasterVolumeLevel += value;
        if (bgmMasterVolumeLevel <= 0) bgmMasterVolumeLevel = 0;
        if (bgmMasterVolumeLevel >= MAX_VOLEME_LEVEL) bgmMasterVolumeLevel = MAX_VOLEME_LEVEL;
    }

    public void IncreaseSEMasterVolumeLevel(int value)
    {
        seMasterVolumeLevel += value;

        if (seMasterVolumeLevel <= 0) seMasterVolumeLevel = 0;
        if (seMasterVolumeLevel >= MAX_VOLEME_LEVEL) seMasterVolumeLevel = MAX_VOLEME_LEVEL;
    }

    public void PlayAudio(AudioClip audio, MyAudioType audioType, float volume, bool isLoop = false)
    {
        GameObject sePlayer = Instantiate(prefabAudioPlayer);
        sePlayer.GetComponent<AudioPlayer>().Play(audio,audioType,volume,isLoop);
    }
}
