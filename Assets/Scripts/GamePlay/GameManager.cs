using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;  //序列化列表
using System.IO;    //输入输出

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<int> scoreList;
    private int score;

    private string dataPath;

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/LeaderBoard.json";    //各平台默认数据保存路径
        scoreList = GetScoreListData();

        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;
        EventHandler.GetPointEvent += OnGetPointEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
        EventHandler.GetPointEvent -= OnGetPointEvent;
    }

    private void OnGameOverEvent()
    {
        //在列表中添加新的分数、排序
        if (!scoreList.Contains(score))
        {
            scoreList.Add(score);
        }

        scoreList.Sort();
        scoreList.Reverse();

        //将列表保存到对应路径的文件当中
        File.WriteAllText(dataPath, JsonConvert.SerializeObject(scoreList));
    }

    private void OnGetPointEvent(int point)
    {
        score = point;
    }

    public List<int> GetScoreListData()
    {
        if (File.Exists(dataPath))  //判断数据文件是否存在
        {
            string jsonData = File.ReadAllText(dataPath);
            return JsonConvert.DeserializeObject<List<int>>(jsonData);  //JsonConvert.DeserializeObject() 返回的是GameObject，需要<>确认转换类型
        }

        return new List<int>();
    }
}
