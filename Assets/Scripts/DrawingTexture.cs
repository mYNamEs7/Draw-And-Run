using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingTexture : MonoBehaviour
{
    [SerializeField] private Color brushColor = Color.black;
    [SerializeField] private int brushSize = 30;

    private RectTransform rectTransform;
    private RawImage rawImage;

    private Texture2D texture;
    private Texture2D sourceTexture;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rawImage = GetComponent<RawImage>();

        sourceTexture = (Texture2D)rawImage.texture;

        ResetTexture();
    }

    public void ResetTexture()
    {
        if (texture != null)
            Destroy(texture);

        texture = new Texture2D(sourceTexture.width, sourceTexture.height);
        Color32[] pixels = sourceTexture.GetPixels32();
        texture.SetPixels32(pixels);
        texture.Apply();

        rawImage.texture = texture;
    }

    public void Draw(Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, null, out Vector2 localPosition);

        int centerX = Mathf.FloorToInt((localPosition.x + rectTransform.rect.width * 0.5f) / rectTransform.rect.width * texture.width);
        int centerY = Mathf.FloorToInt((localPosition.y + rectTransform.rect.height * 0.5f) / rectTransform.rect.height * texture.height);

        for (int y = centerY - brushSize / 2; y < centerY + brushSize / 2; y++)
        {
            for (int x = centerX - brushSize / 2; x < centerX + brushSize / 2; x++)
            {
                if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                    if (distance < brushSize / 2)
                    {
                        texture.SetPixel(x, y, brushColor);
                    }
                }
            }
        }

        texture.Apply();
    }
}
