using System.Collections.Generic;
using System;

namespace LevelsController
{
    public class LevelInfoTransfer
    {
        private static LevelInfoTransfer _levelInfoTransferKeeper;      // Static level data for transfer between scenes

        public int LvlNumber;                               // Current level number
        public Tuple<int, int> GridSize;                    // Puzzle grid size: length and width (in quantities)
        public List<string> ImageNameList;                  // Images that will be at this level, index 0 - main image

        // Empty constructor
        private LevelInfoTransfer()
        {
            LvlNumber = 0;
            GridSize = new Tuple<int, int>(0, 0);
            ImageNameList = new List<string>();
        }
        
        // Complete constructor
        public LevelInfoTransfer(int lvlNumber, Tuple<int, int> gridSize, IEnumerable<string> imageNameList)
        {
            LvlNumber = lvlNumber;
            GridSize = gridSize;
            ImageNameList = new List<string>(imageNameList);
        }

        // Returns or created static class
        public LevelInfoTransfer GetInstance() =>  _levelInfoTransferKeeper ??= new LevelInfoTransfer();
        
        public LevelInfoTransfer GetInstance(int lvlNumber, Tuple<int, int> gridSize, IEnumerable<string> imageNameList)
        {
            _levelInfoTransferKeeper = new LevelInfoTransfer(lvlNumber, gridSize, imageNameList);
            return _levelInfoTransferKeeper;
        }
    }
}