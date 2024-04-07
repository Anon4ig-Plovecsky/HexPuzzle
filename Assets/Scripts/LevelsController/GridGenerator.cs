using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System;

namespace LevelsController
{
    public class GridGenerator : MonoBehaviour
    {
        //Objects
        protected readonly Tuple<int, int> CubeQty = new(3, 2); //[SerializeField]
        private GameObject[] _cellsGameObject;
        protected GameObject GridGameObject;
        private GameObject _cellPrefab;
        
        //Values
        private Tuple<float, float> _centerOfGrid;
        private const float Offset = 0.01f;
        private float _height, _width;
        private Vector3 _cellSize;
        protected virtual void Start()
        {
            _cellsGameObject = new GameObject[CubeQty.Item1 * CubeQty.Item2];
            GridGameObject = GameObject.Find("Grid");
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Cell.prefab");
            asyncOperationHandle.Completed += delegate { LoadCell(asyncOperationHandle); };
        }
        private void LoadCell(AsyncOperationHandle<GameObject> asyncOperationHandle)
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _cellPrefab = asyncOperationHandle.Result;
                _cellSize = _cellPrefab.GetComponent<BoxCollider>().size;
                CalculateDimensions();
                GenerateCells();
            }
            else
                Debug.Log("Failed to load prefab cell!");
        }
        private void CalculateDimensions()
        {
            _width = CubeQty.Item1 * _cellSize.x;
            _height = CubeQty.Item2 * _cellSize.z;
            _centerOfGrid = new Tuple<float, float>((_width + Offset * (CubeQty.Item1 - 1)) / 2 - _cellSize.x / 2,
                (_height + Offset * (CubeQty.Item2 - 1)) / 2 - _cellSize.z / 2);
        }
        private void GenerateCells()
        {
            for (var i = 0; i < CubeQty.Item2; i++)
            for (var j = 0; j < CubeQty.Item1; j++)
            {
                var coord = new Tuple<float, float>(_centerOfGrid.Item1 - j * _cellSize.x - j * Offset,
                    _centerOfGrid.Item2 - i * _cellSize.z - i * Offset);
                var index = i * CubeQty.Item1 + j;
                _cellsGameObject[index] = Instantiate(_cellPrefab,
                    new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                _cellsGameObject[index].transform.SetParent(GridGameObject.transform);
                _cellsGameObject[index].transform.localPosition = new Vector3(coord.Item1, 0, coord.Item2);
                _cellsGameObject[index].name = $"Cell ({index})";
            }
        }
    }
}