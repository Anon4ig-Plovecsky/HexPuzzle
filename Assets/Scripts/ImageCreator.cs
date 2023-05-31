using UnityEngine;
using System;

public class ImageCreator
{
    private int intervalWidth;
    private readonly Sprite painting;
    private readonly int height, width;
    private readonly Tuple<int, int> cubeQty;
    private int offsetByWidth, offsetByHeight;
    private readonly Sprite[] partsOfPaintings;
    public ImageCreator(Sprite mainPainting, Tuple<int, int> cubeQty)
    {
        painting = mainPainting;
        height = painting.texture.height;
        width = painting.texture.width;
        partsOfPaintings = new Sprite[cubeQty.Item1 * cubeQty.Item2];
        this.cubeQty = cubeQty;
        CalculateWidth();
    }
    private void CalculateWidth()
    {
        intervalWidth = Convert.ToInt32(Math.Min(height / cubeQty.Item2, width / cubeQty.Item1));
        offsetByWidth = (width - intervalWidth * cubeQty.Item1) / 2;
        offsetByHeight = (height - intervalWidth * cubeQty.Item2) / 2;
    }
    public void CropPainting()
    {
        for (var i = 0; i < cubeQty.Item2; i++)
        {
            for (var j = 0; j < cubeQty.Item1; j++)
            {
                var texture = new Texture2D(intervalWidth, intervalWidth);
                texture.SetPixels(painting.texture.GetPixels(offsetByWidth + j * intervalWidth,
                    offsetByHeight + i * intervalWidth, intervalWidth, intervalWidth));
                texture.Apply();
                var sprite = Sprite.Create(texture, new Rect(0f, 0f, intervalWidth, intervalWidth),
                    new Vector2(0.5f, 0.5f));
                partsOfPaintings[i * cubeQty.Item1 + j] = sprite;
            }
        }
    }
    public Sprite[] GetPartsOfPainting()
    {
        return partsOfPaintings;
    }
}