using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler
{
    public static TouchPanel Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameController.Instance.GameStarted();
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }
}
