using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    private CanvasGroup canvasGroup;
    public float fadeScaler;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(Fade(0));
    }

    public void Transition(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        yield return Fade(1);

        //异步加载场景
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return Fade(0);
    }

    //协程
    /// <summary>
    /// 渐变
    /// </summary>
    /// <param name="amount">1是黑，0是透明</param>
    /// <returns></returns>
    private IEnumerator Fade(int amount)
    {
        //遮挡射线检测
        canvasGroup.blocksRaycasts = true;

        while (canvasGroup.alpha != amount)
        {
            switch (amount)
            {
                case 0:
                    canvasGroup.alpha -= Time.deltaTime * fadeScaler;
                    break;
                case 1:
                    canvasGroup.alpha += Time.deltaTime * fadeScaler;
                    break;
            }
            yield return null;  //让协程进行下一步
        }
        canvasGroup.blocksRaycasts = false;
    }
}
