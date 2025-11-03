using System;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

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
    private int pixelMultiplicatorCount = 50;

    public int pixelMinCount = 5;

    public float lightKoef = 1f;

    private bool isDisabled = false;

    private float delay = 1f;

    private Texture2D texture;
    Color[] pixels;

    private bool isCleaning = false;

    public CleanPoint[] cleanPoints;

    private int startX = 30;
    private int endX = 226;
    private int startY = 50;
    private int endY = 200;

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

        AddComplexStains();
        texture.Apply();
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
        texture.Apply();
    }

    void Update()
    {
        if (isDisabled)
        {
            return;
        }

        if (delay >= 0)
        {
            delay -= Time.deltaTime;
            return;
        }

        Fade3();

        if (isCleaning)
        {
            Clean();
        }

        texture.Apply();
    }

    public int textureWidth = 256;
    public int textureHeight = 256;
    public float scale = 20f; // Adjust for noise frequency
    public float offsetX = 0f;
    public float offsetY = 0f;
    private Texture2D GenerateNoiseTexture()
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float xCoord = (float)x / textureWidth * scale + offsetX;
                float yCoord = (float)y / textureHeight * scale + offsetY;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = new Color(sample, sample, sample);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
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

    void Fade3()
    {
        var speed = player.velocity.magnitude;

        var alpha = 0.5f;

        var color = new Color(0, 0, 0, alpha);
        var pixelCount = Mathf.CeilToInt(speed * pixelMultiplicatorCount) * 60 * Time.deltaTime;
        if (pixelCount < pixelMinCount)
        {
            if (UnityEngine.Random.value < 0.25f)
            {
                pixelCount = pixelMinCount;
            }
        }

        // pixelCount = Mathf.Clamp(pixelCount, 0, pixelMultiplicatorCount);
        var xSize = endX - startX;
        var ySize = endY - startY;

        //print(pixelCount);
        var pixelCountSpent = 0;

        var lerp = Mathf.Lerp(0.01f, 0.2f, pixelCount / pixelMinCount);
        for (int i = 0; i < pixelCount * 1000; i++)
        {
            var randomRedish = 0f;// UnityEngine.Random.Range(0, 0.2f);
            var randomAlpha = UnityEngine.Random.Range(0.5f, 1f);
            var gray = UnityEngine.Random.Range(0f, 0.3f);

            color = new Color(gray + randomRedish, gray, gray, randomAlpha);

            var randomX = UnityEngine.Random.Range(startX, endX + 1);
            var randomY = UnityEngine.Random.Range(startY, endY + 1);

            var additional = UnityEngine.Random.Range(1, 9);

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (UnityEngine.Random.value < 0.25f)
                    {
                        var pix = texture.GetPixel(randomX + x, randomY + y);
                        if (pix.a > 0.4f)
                        {
                            color.a = 1f;
                        }

                        texture.SetPixel(randomX + x, randomY + y, color);
                        pixelCountSpent++;

                        if (pixelCountSpent >= pixelCount)
                        {
                            return;
                        }
                    }
                }
                //if (UnityEngine.Random.value < 1 /)
            }
        }
    }

    public void AddComplexStains()
    {
        var stainCount = Random.Range(5, 10);
        AddComplexStains(stainCount);
    }

    private void AddComplexStains(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddComplexStain();
        }
    }

    private void AddStains(int count)
    {
        //var stainCount = Random.Range(2, 5);
        /*for (int i = 0; i< stainCount; i++)
        {
            var randomX = Random.Range(startX, endX + 1);
            var randomY = Random.Range(startY, endY + 1);
        }*/

        var stainRadius = Random.Range(10, 20);
        var randomX = Random.Range(startX, endX + 1);
        var randomY = Random.Range(startY, endY + 1);

        AddStain(stainRadius, randomX, randomY);
    }

    private void AddStains()
    {
        //var stainCount = Random.Range(2, 5);
        /*for (int i = 0; i< stainCount; i++)
        {
            var randomX = Random.Range(startX, endX + 1);
            var randomY = Random.Range(startY, endY + 1);
        }*/

        var stainRadius = Random.Range(10, 20);
        var randomX = Random.Range(startX, endX + 1);
        var randomY = Random.Range(startY, endY + 1);

        AddStain(stainRadius, randomX, randomY);
    }

    private void AddComplexStain()
    {
        //var stainCount = Random.Range(2, 5);
        /*for (int i = 0; i< stainCount; i++)
        {
            var randomX = Random.Range(startX, endX + 1);
            var randomY = Random.Range(startY, endY + 1);
        }*/

        // var stainRadius = Random.Range(10, 20);
        var stainRadius = Random.Range(15, 20);
        var randomX = Random.Range(startX, endX + 1);
        var randomY = Random.Range(startY, endY + 1);

        //AddStain(stainRadius, randomX, randomY);

        var additional = Random.Range(5, 8);
        for (int i = 0; i < additional; i++)
        {
            var position = Random.insideUnitCircle * stainRadius /** Random.Range(0.9f, 1.2f)*/;
            var posX = Mathf.RoundToInt(position.x) + randomX;
            var posY = Mathf.RoundToInt(position.y) + randomY;

            var stainRadiusSmall = Mathf.RoundToInt(stainRadius * Random.Range(0.3f, 0.5f));
            AddStain(stainRadiusSmall, posX, posY);
        }
    }

    private void AddStain(int stainRadius, int randomX, int randomY)
    {
        //var

        for (int x = randomX - stainRadius; x <= randomX + stainRadius; x++)
        {
            for (int y = randomY - stainRadius; y < randomY + stainRadius; y++)
            {
                int dx = x - randomX;
                int dy = y - randomY;

                bool isInCentralCircle = dx * dx + dy * dy <= stainRadius * stainRadius;
                if (isInCentralCircle)
                {
                    var randomRedish = 0f;// UnityEngine.Random.Range(0, 0.2f);
                    var randomAlpha = Random.Range(0.5f, 0.8f);
                    var gray = Random.Range(0f, 0.3f);

                    var color = new Color(gray + randomRedish, gray, gray, randomAlpha);

                    var pix = texture.GetPixel(randomX + x, randomY + y);
                    if (pix.a > 0.4f)
                    {
                        color.a = 1f;
                    }
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }

    public float fade2_threshold = 0.1f;
    public float fade2_aMin = 0.1f;
    public float fade2_aMax = 0.2f;


    void Fade2()
    {
        var speed = player.velocity.magnitude;

        int newNoise = UnityEngine.Random.Range(0, 10000);
        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                float xCoord = (float)x / textureWidth * scale + newNoise;
                float yCoord = (float)y / textureHeight * scale + newNoise;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                if (sample < fade2_threshold)
                {
                    var pix = texture.GetPixel(x, y);
                    var randomAlpha = UnityEngine.Random.Range(fade2_aMin, fade2_aMax);
                    var color = new Color(sample, sample, sample, pix.a + randomAlpha);
                    //texture.SetPixel(x, y, color);
                    texture.SetPixel(x, y, color);
                }
            }
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
