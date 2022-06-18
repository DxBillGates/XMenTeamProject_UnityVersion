using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayManager : MonoBehaviour
{
    //SE鳴らすためのオブジェクト
    [SerializeField] private GameObject SEPlayer;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SESeting(AudioClip SE, float volume = 1.0f)
    {
        GameObject sePlayer = Instantiate(SEPlayer);
        sePlayer.GetComponent<SEPlayer>().PlaySE(SE, volume);
    }
}
