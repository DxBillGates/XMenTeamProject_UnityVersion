using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySE(AudioClip SE, float volume = 1)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.PlayOneShot(SE);
    }
}
