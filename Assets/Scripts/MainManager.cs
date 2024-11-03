using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text BestScore;
    public Text PlayerName;
    public Text ScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (DataPersistence.Instance == null)
        {
            DataPersistence.CreateInstance();
        }
#endif
        UpdateBestScoreText();
        PlayerName.text = "Player: " + DataPersistence.Instance.PlayerName;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        UpdateScore();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void UpdateBestScoreText()
    {
        Score best = DataPersistence.Instance.hiScores[0];
        BestScore.text = $"Best Score : {best.playerName} : {best.score}";
    }

    public void UpdateScore()
    {
        List<Score> bestScores = DataPersistence.Instance.hiScores;
        for (int i = 0; i < bestScores.Count; i++)
        {
            if (bestScores[i].score < m_Points)
            {
                Score newScore = new Score();

                newScore.score = m_Points;
                newScore.playerName = DataPersistence.Instance.PlayerName;

                bestScores.Insert(i, newScore);
                bestScores.RemoveAt(bestScores.Count - 1);

                DataPersistence.Instance.hiScores = bestScores;
                DataPersistence.Instance.SaveHiScore();
                UpdateBestScoreText();
                break;
            }
        }
    }
}
