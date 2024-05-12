using UI.TestedModules;
using UnityEngine;

namespace LevelsController
{
    public class GridController : GridGenerator
    {
        private const string CorrectlyString = "[Correctly]";
        private GameObject _cubes;
        private bool _isWin;
        protected override void Start()
        {
            base.Start();
            _cubes = GameObject.Find("Cubes");
        }
        private void Update()
        {
            if (_cubes.transform.childCount == 0
                || _cubes.transform.childCount != GridSize.Item1 * GridSize.Item2
                || _isWin)
                return;
            
            CheckCorrectly();
        }
        private void CheckCorrectly()
        {
            var isCorrectly = true;
            for(var i = 0; i < GridSize.Item1 * GridSize.Item2; i++)
                if (!GameObjectGrid.transform.GetChild(i).name.Contains(CorrectlyString))
                    isCorrectly = false;
            if (!isCorrectly) return;
            _isWin = true;
            CanvasController.ClassCanvasController.ShowWinPanel();
        }
    }
}