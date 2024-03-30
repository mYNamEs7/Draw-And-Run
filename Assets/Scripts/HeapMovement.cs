using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class HeapMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private bool isGameStarted;
    private SplineProjector splineProjector;
    private Rigidbody rb;

    void Awake()
    {
        splineProjector = GetComponent<SplineProjector>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        GameController.Instance.OnGameStarted += GameController_OnGameStarted;
    }

    private void GameController_OnGameStarted()
    {
        isGameStarted = true;
    }

    void Update()
    {
        if (!isGameStarted) return;

        transform.position += speed * Time.deltaTime * splineProjector.result.forward;
    }

    public void Stop()
    {
        isGameStarted = false;
    }
}
