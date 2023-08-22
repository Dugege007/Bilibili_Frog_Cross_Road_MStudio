using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using TMPro;    //引用Text Mesh Pro 的命名空间

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameOverPanel;
    public GameObject leaderboardPanel;

    private void OnEnable()
    {
        //游戏时间恢复
        Time.timeScale = 1;
        //添加注册函数
        EventHandler.GetPointEvent += OnGetPointEvent;
        EventHandler.GameOverEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        //注销函数
        EventHandler.GetPointEvent -= OnGetPointEvent;
        EventHandler.GameOverEvent -= OnGameOverEvent;
    }

    private void Start()
    {
        scoreText.text = "00";
    }

    private void OnGetPointEvent(int point)
    {
        scoreText.text = point.ToString();
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);

        if (gameOverPanel.activeInHierarchy)
        {
            //游戏暂停
            Time.timeScale = 0;
        }
    }

    #region 按钮添加方法
    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        TransitionManager.instance.Transition(SceneManager.GetActiveScene().name);
        AudioManager.instance.PlayBGMMusic();
    }

    public void BackToMenu()
    {
        gameOverPanel.SetActive(false);
        TransitionManager.instance.Transition("Title");
        AudioManager.instance.PlayBGMMusic();
    }

    public void OpenLeaderboard()
    {
        leaderboardPanel.SetActive(true);
    }
    #endregion
}
