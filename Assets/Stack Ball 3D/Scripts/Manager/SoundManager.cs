using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;
    public bool sound = true;

    #region UnityLifecycle
    void Awake()
    {
        MakeSingleton();
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region FunctionPrivate
    private void MakeSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    #region FunctionPublic
    public void SoundOnOff()
    {
        sound = !sound;
    }

    public void PlaySoundFX(AudioClip _audioClip, float _volume)
    {
        if (sound)
            audioSource.PlayOneShot(_audioClip, _volume);
    }
    #endregion
}
