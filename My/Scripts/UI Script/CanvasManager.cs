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
        GameManager.Instance.PlayerInit();
        GameManager.Instance.mainScene = false;
        switch (selectDifficulty.value)
        {
            case 0:
                GameManager.Instance.difficultyLevel = 0.75f;
                break;
            case 1:
                GameManager.Instance.difficultyLevel = 1f;
                break;
            case 2:
                GameManager.Instance.difficultyLevel = 1.5f;
                break;
            case 3:
                GameManager.Instance.difficultyLevel = 3f;
                break;
        }
    }

    public void OnTraitsBtn()
    {
        traitsPanel.SetActive(true);
    }

    public void OnPuzzleBtn()
    {
        puzzlePanel.SetActive(true);

    }

    public void OnSettingBtn()
    {
        settingPanel.SetActive(true);
    }

    public void GameQuitBtn()
    {
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
        GameManager.Instance.PlayerInit();
    }
}
