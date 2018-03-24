using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [Header("Sound Sources")]
    public AudioSource SourceSFX;
    public AudioSource SourceMusic;

    [Header("Sound Clips")]
    public AudioClip Click;
    public AudioClip Victory;
    public AudioClip Loose;
    public AudioClip AwesomeVictory;

    private static SoundManager SoundGameObject;

    private void Awake()
    {
        if (SoundGameObject != null && SoundGameObject != this)
        {
            Destroy(gameObject);
        }
        else
        {
            SoundGameObject = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySFXClick()
    {
        SourceSFX.PlayOneShot(Click);
    }

    public void PlaySFXVictory()
    {
        SourceSFX.PlayOneShot(Victory);
    }

    public void PlaySFXLoose()
    {
        SourceSFX.PlayOneShot(Loose);
    }

    public void PlaySFXAwesomeVictory()
    {
        SourceSFX.PlayOneShot(AwesomeVictory);
    }
}
