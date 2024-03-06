using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameObject inGameUI;
    public GameObject mainUI;
    public Dropdown selectDifficulty;
    public GameObject traitsPanel;
    public GameObject puzzlePanel;
    public GameObject settingPanel;

    private void Update()
    {
        MainUIShow();
        InGameUIShow();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(traitsPanel.activeSelf)
            {
                traitsPanel.SetActive(false);
            }
            if(puzzlePanel.activeSelf)
            {
                puzzlePanel.SetActive(false);
            }
            if(settingPanel.activeSelf)
            {
                settingPanel.SetActive(false);
            }
        }
    }

    void InGameUIShow()
    {
        if (!GameManager.Instance.mainScene)
        {
            inGameUI.SetActive(true);
            mainUI.SetActive(false);
        }
    }

    void MainUIShow()
    {
        if(GameManager.Instance.mainScene)
        {
            inGameUI.SetActive(false);
            mainUI.SetActive(true);
        }
    }

    public void OnStartBtn()
    {
        SoundManager.instance.PlaySE("Button_Click");
        GameManager.Instance.PlayerInit();
        GameManager.Instance.mainScene = false;
        switch (selectDifficulty.value)
        {
            case 0:
                GameManager.Instance.difficultyLevel = 0.75f;
                SoundManager.instance.PlayBGM("BGM_Easy");
                break;
            case 1:
                GameManager.Instance.difficultyLevel = 1f;
                SoundManager.instance.PlayBGM("BGM_Normal");
                break;
            case 2:
                GameManager.Instance.difficultyLevel = 1.5f;
                SoundManager.instance.PlayBGM("BGM_Hard");
                break;
            case 3:
                GameManager.Instance.difficultyLevel = 3f;
                SoundManager.instance.PlayBGM("BGM_Nightmare");
                break;
        }
    }

    public void OnTraitsBtn()
    {
        SoundManager.instance.PlaySE("Button_Click");
        traitsPanel.SetActive(true);
    }

    public void OnPuzzleBtn()
    {
        SoundManager.instance.PlaySE("Button_Click");
        puzzlePanel.SetActive(true);
    }

    public void OnSettingBtn()
    {
        SoundManager.instance.PlaySE("Button_Click");
        settingPanel.SetActive(true);
    }

    public void GameQuitBtn()
    {
        SoundManager.instance.PlaySE("Button_Click");
        GameManager.Instance.SavePlayerDataToJson();
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public void HomeBtn()
    {
        SoundManager.instance.PlaySE("Button_Click");
        GameManager.Instance.PlayerInit();
        GameManager.Instance.SavePlayerDataToJson();
    }

    public void SaveSettingBtn()
    {
        SoundManager.instance.PlaySE("Button_Click");
        GameManager.Instance.SavePlayerSettingDataToJson();
        settingPanel.SetActive(false);
    }
}
