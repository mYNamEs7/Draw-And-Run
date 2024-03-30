using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DrawingTexture))]

public class DrawingPanel : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    public static DrawingPanel Instance { get; private set; }

    [SerializeField] private DrawingSpline drawingSpline;

    private DrawingTexture drawingTexture;
    private RectTransform rectTransform;
    private bool isPressed;

    void Awake()
    {
        Instance = this;

        drawingTexture = GetComponent<DrawingTexture>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        drawingSpline.ClearPoints();

        drawingTexture.Draw(eventData.position);

        AddSplinePoint(eventData.position);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!isPressed) return;

        drawingTexture.Draw(eventData.position);

        AddSplinePoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        drawingTexture.Draw(eventData.position);

        AddSplinePoint(eventData.position);

        drawingSpline.UpdateSpline();

        if (TouchPanel.Instance != null)
            GameController.Instance.GameStarted();
    }

    private void AddSplinePoint(Vector2 position)
    {
        Vector3 pointPosition = GetTouchPositionNormalized(position);
        pointPosition.z = pointPosition.y;
        pointPosition.y = 0f;

        drawingSpline.AddSplinePoint(pointPosition);
    }

    private Vector2 GetTouchPositionNormalized(Vector2 touchPosition)
    {
        Vector2 position = touchPosition - (Vector2)transform.position;
        position.x /= rectTransform.rect.width / 2;
        position.y /= rectTransform.rect.height / 2;
        return Vector2.ClampMagnitude(position, 1f);
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }
}
