using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Slider bgmSet;
    public InputField bgmSetInput;

    public Slider seSet;
    public InputField seSetInput;

    public Slider mouseSet;
    public InputField mouseSetInput;

    private void Start()
    {
        mouseSet.value = GameManager.Instance.mouseSensitivity;
        mouseSetInput.text = (mouseSet.value * 100).ToString();

        bgmSet.value = GameManager.Instance.soundBgmVolume;
        bgmSetInput.text = (bgmSet.value * 100).ToString();

        seSet.value = GameManager.Instance.soundEffectVolume;
        seSetInput.text = (seSet.value * 100).ToString();

    }

    //slider로 민감도 조절
    public void MouseSensitivityChange()
    {
        mouseSetInput.text = (mouseSet.value*100).ToString();
        GameManager.Instance.mouseSensitivity = mouseSet.value;
    }

    //text로 민감도 조절
    public void InputTextMouseSens()
    {
        mouseSet.value = float.Parse(mouseSetInput.text)/100;
    }

    //slider로 bgm볼륨 조절
    public void BgmVolumeChange()
    {
        bgmSetInput.text = (bgmSet.value * 100).ToString();
        GameManager.Instance.soundBgmVolume = bgmSet.value;
        SoundManager.instance.SetBGMVolume(GameManager.Instance.soundBgmVolume);
    }

    //text로 bgm볼륨 조절
    public void InputTextBgmVolume()
    {
        bgmSet.value = float.Parse(bgmSetInput.text) / 100;
        GameManager.Instance.soundBgmVolume = bgmSet.value;
        SoundManager.instance.SetBGMVolume(GameManager.Instance.soundBgmVolume);
    }

    //slider로 효과음 조절
    public void SeVolumeChange()
    {
        seSetInput.text = (seSet.value * 100).ToString();
        GameManager.Instance.soundEffectVolume = seSet.value;
        SoundManager.instance.SetEffectVolume(GameManager.Instance.soundEffectVolume);
    }

    //text로 효과음 조절
    public void InputTextSeVolume()
    {
        seSet.value = float.Parse(seSetInput.text) / 100;
        GameManager.Instance.soundEffectVolume = seSet.value;
        SoundManager.instance.SetEffectVolume(GameManager.Instance.soundEffectVolume);
    }
}
