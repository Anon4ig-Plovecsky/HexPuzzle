using UnityEngine;
using System;

public class ImageCreator
{
    public Vector2 dimension { get; private set; }
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
    public void CropPaintingByParts()
    {
        for (var i = 0; i < cubeQty.Item2; i++)
            for (var j = 0; j < cubeQty.Item1; j++)
                partsOfPaintings[i * cubeQty.Item1 + j] = CropImage(offsetByWidth + j * intervalWidth,
                    offsetByHeight + i * intervalWidth, intervalWidth, intervalWidth);
    }
    public Sprite CropEntirePainting()
    {
        dimension = new Vector2(intervalWidth * cubeQty.Item1, intervalWidth * cubeQty.Item2);
        var sprite = CropImage(offsetByWidth, offsetByHeight, 
            Convert.ToInt32(dimension.x), Convert.ToInt32(dimension.y));
        return sprite;
    }
    private Sprite CropImage(int x, int y, int widthImage, int heightImage)
    {
    var texture = new Texture2D(widthImage, heightImage);
        texture.SetPixels(painting.texture.GetPixels(x, y, widthImage, heightImage));
        texture.Apply();
        var sprite = Sprite.Create(texture, new Rect(0f, 0f, widthImage, heightImage),
            new Vector2(0.5f, 0.5f));
        return sprite;
    } 
    public Sprite[] GetPartsOfPainting()
    {
        return partsOfPaintings;
    }
}