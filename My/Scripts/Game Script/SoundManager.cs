using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    #region singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBGM;

    public Sound[] effectSound;
    public Sound[] bgmSound;

    public string[] playSoundName;

    public float effectVolume;
    public float bgmVolume;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
        effectVolume = GameManager.Instance.soundEffectVolume;
        bgmVolume = GameManager.Instance.soundBgmVolume;
    }

    public void PlaySE(string name)
    {
        for(int i= 0; i < effectSound.Length; i++)
        {
            if(name == effectSound[i].name)
            {
                for(int j= 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSound[i].name;
                        audioSourceEffects[j].clip = effectSound[i].clip;
                        audioSourceEffects[j].volume = effectVolume;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                return;
            }
        }
    }

    public void StopAllSE()
    {
        for (int i= 0;i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
    }

    public void SetEffectVolume(float volume)
    {
        effectVolume = volume;
        foreach (var source in audioSourceEffects)
        {
            source.volume = effectVolume;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        audioSourceBGM.volume = bgmVolume;
    }
}
