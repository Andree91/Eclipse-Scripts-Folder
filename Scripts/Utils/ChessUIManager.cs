using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AG;

public class ChessUIManager : MonoBehaviour
{
    [SerializeField] GameObject UIParent;
    // [SerializeField] Button restartButton;
    // [SerializeField] Button quitButton;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GameObject pauseScreen;
    public GameManager gameManager;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }


    public void HideUI()
    {
        UIParent.SetActive(false);
    }

    public void TogglePauseScreen()
    {
        if (!pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            pauseScreen.SetActive(false);
        }
    }

    public void OnGameFinished(string winner)
    {
        UIParent.SetActive(true);
        if (winner == "Black")
        {
            resultText.color = Color.black;
            // string replacement = "B";
            // winner = replacement + winner.Substring(1);
        }
        else
        {
            resultText.color = Color.white;
        }
        resultText.text = string.Format("{0} player won the game", winner);
    }

    public void QuitChess()
    {
        gameManager.LoadNewScene("SampleScene");
    }
}
