using System.Collections.Generic;
using System;

namespace LevelsController.TestedModules
{
    public class LevelInfoTransfer
    {
        private static LevelInfoTransfer _levelInfoTransferKeeper;      // Static level data for transfer between scenes

        public int LvlNumber;                               // Current level number
        public Tuple<int, int> GridSize;                    // Puzzle grid size: length and width (in quantities)
        public List<string> ImageNameList;                  // Images that will be at this level, index 0 - main image
        public float Timer;                                 // Countdown timer value
        public bool IsTestMode;                             // When enabled, does not enable the debug camera so as not to cause an exception

        // Empty constructor
        private LevelInfoTransfer()
        {
            LvlNumber = 0;
            GridSize = new Tuple<int, int>(0, 0);
            ImageNameList = new List<string>();
            Timer = 0.0f;
            IsTestMode = false;
        }
        
        // Complete constructor
        public LevelInfoTransfer(int lvlNumber, Tuple<int, int> gridSize, IEnumerable<string> imageNameList)
        {
            LvlNumber = lvlNumber;
            GridSize = gridSize;
            ImageNameList = new List<string>(imageNameList);
            Timer = 0.0f;
            IsTestMode = false;
        }

        // Returns or created static class
        public static LevelInfoTransfer GetInstance() =>  _levelInfoTransferKeeper ??= new LevelInfoTransfer();

        public static LevelInfoTransfer SetInstance(LevelInfoTransfer levelInfoTransfer)
        {
            _levelInfoTransferKeeper = new LevelInfoTransfer(
                levelInfoTransfer.LvlNumber,
                levelInfoTransfer.GridSize,
                levelInfoTransfer.ImageNameList
            );

            return _levelInfoTransferKeeper;
        }
        
        public static void SetInstance(int lvlNumber, Tuple<int, int> gridSize, IEnumerable<string> imageNameList)
        {
            _levelInfoTransferKeeper = new LevelInfoTransfer(lvlNumber, gridSize, imageNameList);
        }
    }
}