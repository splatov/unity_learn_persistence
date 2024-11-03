using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence Instance { get; private set; }
    public string PlayerName;

    public List<Score> hiScores;

    static readonly string saveFileName = "hiscore.json";
    static readonly string defaultPlayerName = "Player";

    [System.Serializable]
    class ScoreTable
    {
        public List<Score> hiScores;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHiScore();
        }
    }

    string GetSavePath()
    {
        return Application.persistentDataPath + "/" + saveFileName;
    }

    public void SaveHiScore()
    {
        ScoreTable table = new ScoreTable();
        table.hiScores = hiScores;

        string json = JsonUtility.ToJson(table);
        File.WriteAllText(GetSavePath(), json);
    }

    public void LoadHiScore()
    {
        string path = GetSavePath();

        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            hiScores = JsonUtility.FromJson<ScoreTable>(data).hiScores;
        }

        if (hiScores == null || hiScores.Count == 0)
        {
            CreateDefaultHiScore();
        }
    }

    void CreateDefaultHiScore()
    {
        hiScores = new List<Score>(3);
        for (int i = 0; i < hiScores.Capacity; i++)
        {
            Score item = new Score();
            item.playerName = defaultPlayerName;
            item.score = 0;
            hiScores.Add(item);
        }
    }
    
#if UNITY_EDITOR
    public static void CreateInstance()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("Data Persistence");
            obj.AddComponent(typeof(DataPersistence));
            Instantiate(obj);
            Instance.PlayerName = defaultPlayerName;
        }
    }
#endif
}
