using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

[RequireComponent(typeof(SplineComputer))]

public class DrawingSpline : MonoBehaviour
{
    [SerializeField] private DrawingTexture drawingTexture;

    private SplineComputer spline;

    private readonly List<SplinePoint> points = new List<SplinePoint>();

    void Awake()
    {
        spline = GetComponent<SplineComputer>();
    }

    void Start()
    {
        UnitFactory.Instance.SpawnUnitsOnSpline(spline);
    }

    public void ClearPoints()
    {
        points.Clear();
    }

    public void AddSplinePoint(Vector3 position)
    {
        points.Add(new SplinePoint(position));
    }

    public void UpdateSpline()
    {
        StartCoroutine(UpdateSplineCoroutine());
    }

    private IEnumerator UpdateSplineCoroutine()
    {
        spline.SetPoints(points.ToArray(), SplineComputer.Space.Local);
        spline.Rebuild();

        yield return null;

        UnitFactory.Instance.MoveUnits(spline);

        yield return null;

        drawingTexture.ResetTexture();
    }
}
