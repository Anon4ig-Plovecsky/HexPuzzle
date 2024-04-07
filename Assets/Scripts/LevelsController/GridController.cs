using UnityEngine;
using UI;

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
            if (_cubes.transform.childCount != CubeQty.Item1 * CubeQty.Item2 || _isWin) return;
            CheckCorrectly();
        }
        private void CheckCorrectly()
        {
            var isCorrectly = true;
            for(var i = 0; i < CubeQty.Item1 * CubeQty.Item2; i++)
                if (!GridGameObject.transform.GetChild(i).name.Contains(CorrectlyString))
                    isCorrectly = false;
            if (!isCorrectly) return;
            _isWin = true;
            CanvasController.ClassCanvasController.ShowWinPanel();
        }
    }
}