using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public event Action OnGameStarted;

    void Awake()
    {
        Instance = this;
    }

    public void GameStarted()
    {
        OnGameStarted?.Invoke();
        TouchPanel.Instance.DestroyPanel();
    }
}
