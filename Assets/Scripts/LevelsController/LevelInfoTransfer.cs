using System.Collections.Generic;
using System;

namespace LevelsController
{
    public class LevelInfoTransfer
    {
        private static LevelInfoTransfer _levelInfoTransferKeeper;          // Static level data for transfer between scenes

        public int LvlNumber = 0;                           // Current level number
        public Tuple<int, int> GridSize;                    // Puzzle grid size: length and width (in quantities)
        public List<ButtonImagePaths> ImagePathsList;       // Images that will be at this level, index 0 - main image

        // Empty constructor
        private LevelInfoTransfer()
        {
            LvlNumber = 0;
            GridSize = new Tuple<int, int>(0, 0);
            ImagePathsList = new List<ButtonImagePaths>();
        }
        
        // Complete constructor
        private LevelInfoTransfer(int lvlNumber, Tuple<int, int> gridSize, IEnumerable<ButtonImagePaths> imagePathsList)
        {
            LvlNumber = lvlNumber;
            GridSize = gridSize;
            ImagePathsList = new List<ButtonImagePaths>(imagePathsList);
        }

        // Returns or created static class
        public LevelInfoTransfer GetInstance() =>  _levelInfoTransferKeeper ??= new LevelInfoTransfer();
        
        public LevelInfoTransfer GetInstance(int lvlNumber, Tuple<int, int> gridSize, IEnumerable<ButtonImagePaths> imagePathsList)
        {
            _levelInfoTransferKeeper = new LevelInfoTransfer(lvlNumber, gridSize, imagePathsList);
            return _levelInfoTransferKeeper;
        }
    }
}