using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using TMPro;    //����Text Mesh Pro �������ռ�

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameOverPanel;
    public GameObject leaderboardPanel;

    private void OnEnable()
    {
        //��Ϸʱ��ָ�
        Time.timeScale = 1;
        //���ע�ắ��
        EventHandler.GetPointEvent += OnGetPointEvent;
        EventHandler.GameOverEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        //ע������
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
    /// ��Ϸ����
    /// </summary>
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);

        if (gameOverPanel.activeInHierarchy)
        {
            //��Ϸ��ͣ
            Time.timeScale = 0;
        }
    }

    #region ��ť��ӷ���
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
