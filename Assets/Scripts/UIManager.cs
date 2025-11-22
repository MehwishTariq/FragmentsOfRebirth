using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Video;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenu, InGameUI, PauseMenu, CompleteMenu, FailMenu,VideoPanel, CreditsPanel;
    public Image healthFillBar;

    VideoPlayer video;
    public bool videoPlayed;
    void OnEnable()
    {
        GameManager.Instance.LevelComplete += LevelComplete;
        GameManager.Instance.LevelFail += LevelFail;
        PlayerHealth.PlayerHit += HealthUiUpdate;
        video = VideoPanel.GetComponent<VideoPlayer>();
        video.loopPointReached += StartLevel;
    }

    private void StartLevel(VideoPlayer source)
    {
        GameManager.Instance.LoadLevel();
        MainMenu.SetActive(false);
        VideoPanel.SetActive(false);
        videoPlayed = true;
        AudioManager.Instance.PlayMusic();
    }

    void OnDisable()
    {
        GameManager.Instance.LevelComplete -= LevelComplete;
        GameManager.Instance.LevelFail -= LevelFail;
        PlayerHealth.PlayerHit -= HealthUiUpdate;
    }

    public void HealthUiUpdate(float val)
    {
        healthFillBar.DOFillAmount(1 - val / 100, 0.2f);
    }
    
    public void Play()
    {
        if (!videoPlayed)
            VideoPanel.SetActive(true);
        else
            StartLevel(video);

        InGameUI.SetActive(true);
    }

    public void PauseOrResume(bool pauseState)
    {
        InGameUI.SetActive(!pauseState);
        PauseMenu.SetActive(pauseState);
    }

    public void Restart()
    {
        PauseMenu.SetActive(false);
        Play();
        FailMenu.SetActive(false);
    }

    public void Next()
    {
        if(!GameManager.Instance.LoadNextLevelIfAvailable())
        {
            MainMenu.SetActive(true);
        }
        CompleteMenu.SetActive(false);
    }

    public void LevelComplete()
    {
        CompleteMenu.SetActive(true);
        GameManager.Instance.DeleteLevel();
    }

    public void LevelFail()
    {
        FailMenu.SetActive(true);
        GameManager.Instance.DeleteLevel();
    }
    public void GoToMM()
    {
        CompleteMenu.SetActive(false);
        FailMenu.SetActive(false);
        PauseMenu.SetActive(false);
        InGameUI.SetActive(false);
        MainMenu.SetActive(true);
        GameManager.Instance.DeleteLevel();
    }   

    public void Credits(bool open)
    {
        CreditsPanel.SetActive(open);
    }
    public void Quit(){

        Application.Quit();
    }
}
