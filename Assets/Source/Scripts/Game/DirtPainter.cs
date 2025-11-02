using Assets.Source.Scripts.Game;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class CleanPoint
{
    public int centerX;
    public int centerY;
    public int radius = 10;

    public float startAngle = 120;
    public float currentAngle = 120;
    public float endAngle = 60;
    // public float angleSpeed = 10;
    public float rotationAmplitude = 100;
    public bool isCleaning;
    public Color Color = Color.red;
}

public class DirtPainter : MonoBehaviour
{
    public CharacterController player;
    public SpriteRenderer targetRenderer;

    public int textureSize = 512;
    public int pixelMultiplicatorCount = 100;

    public int pixelMinCount = 5;

    public float lightKoef = 1f;

    private bool isDisabled = false;

    private Texture2D texture;
    Color[] pixels;

    private bool isCleaning = false;

    public CleanPoint[] cleanPoints;

    public int startX;
    public int startY;
    public int endX;
    public int endY;

    public static DirtPainter Instance;
    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        isDisabled = false;
        texture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Bilinear;

        // Залей всё
        FillAll();
        texture.Apply();

        targetRenderer.color = Color.white;
        // Назначь в спрайт
        targetRenderer.sprite = Sprite.Create(texture,
            new Rect(0, 0, textureSize, textureSize),
            new Vector2(0.5f, 0.5f),
            textureSize); // Пиксель пер юнит

        rotationX = textureSize / 2;
    }


    public void FillAll()
    {
        pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = new Color(0, 0, 0, 0);
        texture.SetPixels(pixels);
    }

    public void FillAll_Test()
    {
        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                texture.SetPixel(x, y, new Color(0, 1, 0, 1));
            }
        }
    }

    void Update()
    {
        if (isDisabled)
        {
            return;
        }

        Fade();

        if (isCleaning)
        {
            Clean();
        }

        texture.Apply();
    }

    void Fade()
    {
        var speed = player.velocity.magnitude;
        // прозрачный

        // Определяем центр текстуры
        int centerX = textureSize / 2;
        int centerY = textureSize / 2;

        float darkSum = 0;

        //print(speed);

        var alpha = 0.5f;

        var color = new Color(0, 0, 0, alpha);
        var pixelCount = Mathf.CeilToInt(speed) * pixelMultiplicatorCount;
        if (pixelCount < pixelMinCount)
        {
            pixelCount = pixelMinCount;
        }
        // pixelCount = Mathf.Clamp(pixelCount, 0, pixelMultiplicatorCount);
        for (int i = 0; i < pixelCount; i++)
        {
            var gray = UnityEngine.Random.Range(0f, 0.3f);
            color = new Color(gray, gray, gray, alpha);
            var randomX = UnityEngine.Random.Range(0, textureSize + 1);
            var randomY = UnityEngine.Random.Range(0, textureSize + 1);
            var pix = texture.GetPixel(randomX, randomY);
            if (pix.a > 0.4f)
            {
                color.a = 1f;
            }
            texture.SetPixel(randomX, randomY, color);
        }
    }


    public void StartClean()
    {
        if (isCleaning)
            return;

        var minAngle = cleanPoints.Max(t => t.startAngle);
        var maxAngle = cleanPoints.Min(t => t.endAngle);
        currentAngle = minAngle;

        foreach (var item in cleanPoints)
        {
            item.currentAngle = item.startAngle;
            item.isCleaning = true;
        }
        isCleaning = true;
    }

    public int rotationX = 120;
    public int rotationY = 150;

    //public float startAngle = 120;
    public float currentAngle = 120;
    //public float endAngle = 60;
    //public float rotationAmplitude = 100;
    public float angleSpeed = 200;
    int minAngle = 0;
    int maxAngle = 0;
    int index = 0;

    private void Clean()
    {
        //var minAngle = cleanPoints.Min(t => t.startAngle);
        //var maxAngle = cleanPoints.Max(t => t.endAngle);

        var color = new Color(0, 0, 0, 0);
        float angle = currentAngle * Mathf.Deg2Rad;
        foreach (var cleanPoint in cleanPoints)
        {
            if (!cleanPoint.isCleaning)
                continue;

            //float angle = cleanPoint.currentAngle * Mathf.Deg2Rad;
            cleanPoint.centerX = rotationX + Mathf.RoundToInt(cleanPoint.rotationAmplitude * Mathf.Cos(angle));
            cleanPoint.centerY = rotationY + Mathf.RoundToInt(cleanPoint.rotationAmplitude * Mathf.Sin(angle));
        }

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                for (int i = 0; i < cleanPoints.Length; i++)
                {
                    //if (i != index)
                    //    continue;

                    var point = cleanPoints[i];
                    if (currentAngle >= point.startAngle || currentAngle <= point.endAngle)
                    {
                        continue;
                    }

                    int dx = x - point.centerX;
                    int dy = y - point.centerY;

                    bool isInCentralCircle = dx * dx + dy * dy <= point.radius * point.radius;
                    if (isInCentralCircle)
                    {
                        //texture.SetPixel(x, y, point.Color);
                        texture.SetPixel(x, y, color);
                    }
                }
            }
        }

        var allCompleted = true;
        foreach (var cleanPoint in cleanPoints)
        {
            if (!cleanPoint.isCleaning)
                continue;

            if (currentAngle <= cleanPoint.endAngle)
            {
                cleanPoint.currentAngle = cleanPoint.startAngle;
                cleanPoint.isCleaning = false;
                continue;
            }
            allCompleted = false;

            //cleanPoint.currentAngle -= angleSpeed * Time.deltaTime;
        }
        currentAngle -= angleSpeed * Time.deltaTime;
        index++;
        if (index >= cleanPoints.Length)
        {
            index = 0;
        }
        /*if (allCompleted)
        {
            isCleaning = false;
        }
        */
        if (currentAngle <= maxAngle)
        {

            isCleaning = false;
        }

    }
    /*
    public void CleanTest()
    {
        var
        print(centerX);

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                int dx = x - centerX;
                int dy = y - centerY;

                int newDx = x - rotationX;
                int newDy = y - rotationY;

                bool isInNewCircle = newDx * newDx + newDy * newDy <= rotationAmplitude * rotationAmplitude;
                if (isInNewCircle)
                {
                    texture.SetPixel(x, y, new Color(1, 0, 0, 1));
                }
                /*

            }
        }


    }*/

    public void Disable()
    {
        isDisabled = true;

    }
}
