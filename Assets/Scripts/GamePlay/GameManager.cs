using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;  //���л��б�
using System.IO;    //�������

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<int> scoreList;
    private int score;

    private string dataPath;

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/LeaderBoard.json";    //��ƽ̨Ĭ�����ݱ���·��
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
        //���б�������µķ���������
        if (!scoreList.Contains(score))
        {
            scoreList.Add(score);
        }

        scoreList.Sort();
        scoreList.Reverse();

        //���б��浽��Ӧ·�����ļ�����
        File.WriteAllText(dataPath, JsonConvert.SerializeObject(scoreList));
    }

    private void OnGetPointEvent(int point)
    {
        score = point;
    }

    public List<int> GetScoreListData()
    {
        if (File.Exists(dataPath))  //�ж������ļ��Ƿ����
        {
            string jsonData = File.ReadAllText(dataPath);
            return JsonConvert.DeserializeObject<List<int>>(jsonData);  //JsonConvert.DeserializeObject() ���ص���GameObject����Ҫ<>ȷ��ת������
        }

        return new List<int>();
    }
}
