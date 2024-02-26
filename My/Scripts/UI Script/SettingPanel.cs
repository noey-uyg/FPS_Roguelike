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

    List<Resolution> resolutions = new List<Resolution>();
    FullScreenMode screenMode;
    public Dropdown resolutionDd;
    public Toggle fullScreenBtn;
    public int resolutionNum;

    private void Start()
    {
        mouseSet.value = GameManager.Instance.settingData.mouseSensitivity;
        mouseSetInput.text = (mouseSet.value * 100).ToString();

        bgmSet.value = GameManager.Instance.settingData.soundBgmVolume;
        bgmSetInput.text = (bgmSet.value * 100).ToString();

        seSet.value = GameManager.Instance.settingData.soundEffectVolume;
        seSetInput.text = (seSet.value * 100).ToString();

        InitResolution();

    }

    //slider로 민감도 조절
    public void MouseSensitivityChange()
    {
        mouseSetInput.text = (mouseSet.value*100).ToString();
        GameManager.Instance.settingData.mouseSensitivity = mouseSet.value;
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
        GameManager.Instance.settingData.soundBgmVolume = bgmSet.value;
        SoundManager.instance.SetBGMVolume(GameManager.Instance.settingData.soundBgmVolume);
    }

    //text로 bgm볼륨 조절
    public void InputTextBgmVolume()
    {
        bgmSet.value = float.Parse(bgmSetInput.text) / 100;
        GameManager.Instance.settingData.soundBgmVolume = bgmSet.value;
        SoundManager.instance.SetBGMVolume(GameManager.Instance.settingData.soundBgmVolume);
    }

    //slider로 효과음 조절
    public void SeVolumeChange()
    {
        seSetInput.text = (seSet.value * 100).ToString();
        GameManager.Instance.settingData.soundEffectVolume = seSet.value;
        SoundManager.instance.SetEffectVolume(GameManager.Instance.settingData.soundEffectVolume);
    }

    //text로 효과음 조절
    public void InputTextSeVolume()
    {
        seSet.value = float.Parse(seSetInput.text) / 100;
        GameManager.Instance.settingData.soundEffectVolume = seSet.value;
        SoundManager.instance.SetEffectVolume(GameManager.Instance.settingData.soundEffectVolume);
    }

    //해상도 초기화
    void InitResolution()
    {
        resolutions.AddRange(Screen.resolutions);
        resolutionDd.options.Clear();

        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "X" + item.height + " " + item.refreshRateRatio + "hz";
            resolutionDd.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDd.value = optionNum;
            }
            optionNum++;
        }
        resolutionDd.RefreshShownValue();
        fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.ExclusiveFullScreen) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
        ResolutionOkBtn();
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        ResolutionOkBtn();
    }

    //해상도 변경
    public void ResolutionOkBtn()
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height,
            screenMode);
    }
}