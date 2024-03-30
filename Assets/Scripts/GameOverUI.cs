using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button playButton;

    private void Awake()
    {
        Instance = this;

        playButton.onClick.AddListener(() => SceneManager.LoadSceneAsync(0));
        gameObject.SetActive(false);
    }

    public void Init(bool isVictory)
    {
        gameObject.SetActive(true);

        if (isVictory)
        {
            victoryText.text = "victory!";
            buttonText.text = "next";
        }
        else
        {
            victoryText.text = "try again!";
            buttonText.text = "restart";
        }
    }
}
