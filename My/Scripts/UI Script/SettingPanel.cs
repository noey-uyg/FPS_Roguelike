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
        mouseSet.value = GameManager.Instance.mouseSensitivity;
        mouseSetInput.text = (mouseSet.value * 100).ToString();

        bgmSet.value = GameManager.Instance.soundBgmVolume;
        bgmSetInput.text = (bgmSet.value * 100).ToString();

        seSet.value = GameManager.Instance.soundEffectVolume;
        seSetInput.text = (seSet.value * 100).ToString();

        InitResolution();

    }

    //slider�� �ΰ��� ����
    public void MouseSensitivityChange()
    {
        mouseSetInput.text = (mouseSet.value*100).ToString();
        GameManager.Instance.mouseSensitivity = mouseSet.value;
    }

    //text�� �ΰ��� ����
    public void InputTextMouseSens()
    {
        mouseSet.value = float.Parse(mouseSetInput.text)/100;
    }

    //slider�� bgm���� ����
    public void BgmVolumeChange()
    {
        bgmSetInput.text = (bgmSet.value * 100).ToString();
        GameManager.Instance.soundBgmVolume = bgmSet.value;
        SoundManager.instance.SetBGMVolume(GameManager.Instance.soundBgmVolume);
    }

    //text�� bgm���� ����
    public void InputTextBgmVolume()
    {
        bgmSet.value = float.Parse(bgmSetInput.text) / 100;
        GameManager.Instance.soundBgmVolume = bgmSet.value;
        SoundManager.instance.SetBGMVolume(GameManager.Instance.soundBgmVolume);
    }

    //slider�� ȿ���� ����
    public void SeVolumeChange()
    {
        seSetInput.text = (seSet.value * 100).ToString();
        GameManager.Instance.soundEffectVolume = seSet.value;
        SoundManager.instance.SetEffectVolume(GameManager.Instance.soundEffectVolume);
    }

    //text�� ȿ���� ����
    public void InputTextSeVolume()
    {
        seSet.value = float.Parse(seSetInput.text) / 100;
        GameManager.Instance.soundEffectVolume = seSet.value;
        SoundManager.instance.SetEffectVolume(GameManager.Instance.soundEffectVolume);
    }

    //�ػ� �ʱ�ȭ
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
        fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }


    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
        ResolutionOkBtn();
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        ResolutionOkBtn();
    }

    //�ػ� ����
    public void ResolutionOkBtn()
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height,
            screenMode);
    }
}