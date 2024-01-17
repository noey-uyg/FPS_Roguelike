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

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
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
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 오디오 소스가 사용중입니다.");
                return;
            }
            Debug.Log(name + "사운드가 등록되지 않았습니다.");
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

        Debug.Log("재생 중인" + name + "사운드가 없습니다.");
    }
}
