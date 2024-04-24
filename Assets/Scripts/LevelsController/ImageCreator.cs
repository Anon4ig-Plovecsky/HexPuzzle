using UnityEngine;
using System;

namespace LevelsController
{
    public class ImageCreator
    {
        public Vector2 Dimension { get; private set; }
        private int _intervalWidth;
        private readonly Sprite _painting;
        private readonly int _height, _width;
        private readonly Tuple<int, int> _gridSize;
        private int _offsetByWidth, _offsetByHeight;
        private readonly Sprite[] _arrPartsOfPainting;
        public ImageCreator(Sprite painting, Tuple<int, int> gridSize)
        {
            _painting = painting;
            _height = _painting.texture.height;
            _width = _painting.texture.width;
            _arrPartsOfPainting = new Sprite[gridSize.Item1 * gridSize.Item2];
            _gridSize = gridSize;
            CalculateWidth();
        }
        private void CalculateWidth()
        {
            _intervalWidth = Convert.ToInt32(Math.Min(_height / _gridSize.Item1, _width / _gridSize.Item2));
            _offsetByWidth = (_width - _intervalWidth * _gridSize.Item2) / 2;
            _offsetByHeight = (_height - _intervalWidth * _gridSize.Item1) / 2;
        }
        public void CropPaintingByParts()
        {
            for (var i = 0; i < _gridSize.Item1; i++)
                for (var j = 0; j < _gridSize.Item2; j++)
                    _arrPartsOfPainting[i * _gridSize.Item2 + j] = CropImage(_offsetByWidth + j * _intervalWidth,
                        _offsetByHeight + i * _intervalWidth, _intervalWidth, _intervalWidth);
        }
        public Sprite CropEntirePainting()
        {
            Dimension = new Vector2(_intervalWidth * _gridSize.Item2, _intervalWidth * _gridSize.Item1);
            var sprite = CropImage(_offsetByWidth, _offsetByHeight, 
                Convert.ToInt32(Dimension.x), Convert.ToInt32(Dimension.y));
            return sprite;
        }
        private Sprite CropImage(int x, int y, int widthImage, int heightImage)
        {
            var texture = new Texture2D(widthImage, heightImage);
            texture.SetPixels(_painting.texture.GetPixels(x, y, widthImage, heightImage));
            texture.Apply();
            var sprite = Sprite.Create(texture, new Rect(0f, 0f, widthImage, heightImage),
                new Vector2(0.5f, 0.5f));
            return sprite;
        } 
        public Sprite[] GetPartsOfPainting() => _arrPartsOfPainting;
    }
}