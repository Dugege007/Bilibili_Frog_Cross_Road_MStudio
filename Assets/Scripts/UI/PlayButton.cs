using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        //将要执行的方法绑定到按钮上
        button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        //启动游戏 加载游戏场景
        //SceneManager.LoadScene("Gameplay");
        TransitionManager.instance.Transition("Gameplay");
    }
}
