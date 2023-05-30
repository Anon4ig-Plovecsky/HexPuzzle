using System;
using UnityEngine;

public class ImageCreator
{
    private Sprite painting;
    private int height, width;
    private int intervalHeight, intervalWidth;
    private readonly Tuple<int, int> cubeQty;
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
        
    }
    public Sprite[] CropPainting()
    {
        for (var i = 0; i < cubeQty.Item1; i++)
        {
            for (var j = 0; j < cubeQty.Item2; j++)
            {
                // var sprite = Sprite.Create(painting.texture, new Rect())
            }
        }
        return partsOfPaintings;
    }
}