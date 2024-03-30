using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public static UnitFactory Instance { get; private set; }

    [SerializeField] private Transform particlePrefab;
    [SerializeField] private Transform unitPrefab;

    [SerializeField] private float movingDuration = 0.2f;

    private HeapMovement heapMovement;
    private readonly List<Transform> units = new List<Transform>();

    void Awake()
    {
        Instance = this;

        heapMovement = GetComponent<HeapMovement>();
    }

    public void SpawnUnitsOnSpline(SplineComputer spline)
    {
        for (int i = 0; i < spline.pointCount; i++)
        {
            var unit = Instantiate(unitPrefab, spline.GetPoint(i).position, Quaternion.identity, transform);
            units.Add(unit);
        }

        SpawnParticle();
    }

    private void SpawnParticle()
    {
        var particle = Instantiate(particlePrefab, transform);
        Destroy(particle.gameObject, 0.5f);
    }

    public void MoveUnits(SplineComputer spline)
    {
        for (int i = 0; i < units.Count; i++)
        {
            float normalizedParam = i / (float)units.Count;

            Vector3 spawnPosition = spline.EvaluatePosition(normalizedParam);

            StartCoroutine(MoveUnitsSmoothly(units[i], spawnPosition, movingDuration));
        }
    }

    private IEnumerator MoveUnitsSmoothly(Transform unit, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = unit.position;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            unit.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        unit.position = targetPosition;
    }

    public void AddUnit(Transform unit)
    {
        units.Add(unit);
        unit.SetParent(transform);
    }

    public void RemoveUnit(Transform unit)
    {
        units.Remove(unit);

        if (units.Count == 0)
        {
            heapMovement.Stop();

            DrawingPanel.Instance.DestroyPanel();
            GameOverUI.Instance.Init(false);
        }
    }

    public void OnVictory()
    {
        foreach (Transform unit in units)
        {
            var player = unit.GetComponent<Player>();
            player.StartVictoryAnimation();
            heapMovement.Stop();
        }

        DrawingPanel.Instance.DestroyPanel();
        GameOverUI.Instance.Init(true);
    }
}
