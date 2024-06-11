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
        public string StrSceneName;                         // Name of the level scene in which you need to complete the puzzle
        public float Timer;                                 // Countdown timer value
        public bool IsTestMode;                             // When enabled, does not enable the debug camera so as not to cause an exception

        // Empty constructor
        private LevelInfoTransfer()
        {
            LvlNumber = 0;
            GridSize = new Tuple<int, int>(0, 0);
            ImageNameList = new List<string>();
            StrSceneName = "";
            Timer = 0.0f;
            IsTestMode = false;
        }
        
        // Complete constructor
        public LevelInfoTransfer(int lvlNumber, Tuple<int, int> gridSize, IEnumerable<string> imageNameList, string strSceneName)
        {
            LvlNumber = lvlNumber;
            GridSize = gridSize;
            ImageNameList = new List<string>(imageNameList);
            StrSceneName = strSceneName;
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
                levelInfoTransfer.ImageNameList,
                levelInfoTransfer.StrSceneName
            );

            return _levelInfoTransferKeeper;
        }
        
        public static LevelInfoTransfer SetInstance(int lvlNumber, Tuple<int, int> gridSize, IEnumerable<string> imageNameList, string strSceneName)
        {
            _levelInfoTransferKeeper = new LevelInfoTransfer(lvlNumber, gridSize, imageNameList, strSceneName);
            return _levelInfoTransferKeeper;
        }
    }
}