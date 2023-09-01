using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip ButtonSfx;
    public AudioClip MerchantSfx;

    void Awake()
    {
        GameController.Instance.AudioController = this;
    }

    public void PlayButtonSfx()
    {
        PlaySound(ButtonSfx);
    }

    public void PlayMerchantSfx()
    {
        PlaySound(MerchantSfx);
    }

    private void PlaySound(AudioClip clip)
    {
        AudioSource.PlayOneShot(clip);
    }
}
