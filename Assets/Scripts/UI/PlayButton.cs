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

        //��Ҫִ�еķ����󶨵���ť��
        button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        //������Ϸ ������Ϸ����
        //SceneManager.LoadScene("Gameplay");
        TransitionManager.instance.Transition("Gameplay");
    }
}
