using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiScoreUI : MonoBehaviour
{
    [SerializeField] List<GameObject> hiScoreLines;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (DataPersistence.Instance == null)
        {
            DataPersistence.CreateInstance();
        }
#endif
        FillScoreTable();
    }

    void FillScoreTable()
    {
        List<Score> scores = DataPersistence.Instance.hiScores;

        for (int i = 0; i < hiScoreLines.Count; i++)
        {
            if (i >= scores.Count)
            {
                break;
            }

            Text[] labels = hiScoreLines[i].GetComponentsInChildren<Text>();

            labels[0].text = $"{i + 1}.";
            labels[1].text = scores[i].playerName;
            labels[2].text = $"{scores[i].score}";
        }
    }

    // Update is called once per frame
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
