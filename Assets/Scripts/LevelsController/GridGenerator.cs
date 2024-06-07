using LevelsController.TestedModules;
using UnityEngine;
using System;

namespace LevelsController
{
    public class GridGenerator : MonoBehaviour
    {
        //Objects
        protected Tuple<int, int> GridSize;
        private GameObject[] _arrGameObjCells;
        protected GameObject GameObjectGrid;
        private GameObject _cellPrefab;
        
        //Values
        private Tuple<float, float> _endOfGrid;
        private const float Space = 0.01f;
        private float _height, _width;
        private Vector3 _cellSize;

        private LevelInfoTransfer _levelInfoTransfer;
        
        protected virtual void Start()
        {
            // Loading grid size from a static class
            _levelInfoTransfer = LevelInfoTransfer.GetInstance();
            GridSize = _levelInfoTransfer.GridSize;
            
            _arrGameObjCells = new GameObject[GridSize.Item1 * GridSize.Item2];
            GameObjectGrid = GameObject.Find(CommonKeys.Names.Grid);

            var asyncTask = CommonKeys.LoadResource<GameObject>(CommonKeys.Addressable.CellPrefab);
            asyncTask.Wait();
            _cellPrefab = asyncTask.Result;
            _cellSize = _cellPrefab.GetComponent<BoxCollider>().size;
            CalculateDimensions();
            GenerateCells();
        }
        private void CalculateDimensions()
        {
            _width = GridSize.Item2 * _cellSize.x;
            _height = GridSize.Item1 * _cellSize.z;
            
            // Getting the end of the mesh (lower right corner) relative to the mesh center coordinates
            _endOfGrid = new Tuple<float, float>((_width + Space * (GridSize.Item2 - 1)) / 2 - _cellSize.x / 2,
                (_height + Space * (GridSize.Item1 - 1)) / 2 - _cellSize.z / 2);
        }
        private void GenerateCells()
        {
            for (var i = 0; i < GridSize.Item1; i++)
                for (var j = 0; j < GridSize.Item2; j++)
                {
                    var coord = new Tuple<float, float>(_endOfGrid.Item1 - j * _cellSize.x - j * Space,
                        _endOfGrid.Item2 - i * _cellSize.z - i * Space);
                    var index = i * GridSize.Item2 + j;
                    _arrGameObjCells[index] = Instantiate(_cellPrefab,
                        new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                    _arrGameObjCells[index].transform.SetParent(GameObjectGrid.transform);
                    _arrGameObjCells[index].transform.localPosition = new Vector3(coord.Item1, 0, coord.Item2);
                    _arrGameObjCells[index].name = $"Cell ({index})";
                }
        }
    }
}