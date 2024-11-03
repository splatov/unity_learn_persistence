using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;

    void Start()
    {
        playerName.text = DataPersistence.Instance.PlayerName;
    }

    public void SetPlayerName()
    {
        DataPersistence.Instance.PlayerName = playerName.text;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowHiScores()
    {
        SceneManager.LoadScene(2);
    }

    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
