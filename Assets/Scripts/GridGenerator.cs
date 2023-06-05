using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System;

public class GridGenerator : MonoBehaviour
{
    //Objects
    protected readonly Tuple<int, int> CubeQty = new(3, 2); //[SerializeField]
    private GameObject[] cellsGameObject;
    protected GameObject GridGameObject;
    private GameObject cellPrefab;
    //Values
    private Tuple<float, float> centerOfGrid;
    private const float Offset = 0.01f;
    private float height, width;
    private Vector3 cellSize;
    protected virtual void Start()
    {
        cellsGameObject = new GameObject[CubeQty.Item1 * CubeQty.Item2];
        GridGameObject = GameObject.Find("Grid");
        var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Cell.prefab");
        asyncOperationHandle.Completed += delegate { LoadCell(asyncOperationHandle); };
    }
    private void LoadCell(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            cellPrefab = asyncOperationHandle.Result;
            cellSize = cellPrefab.GetComponent<BoxCollider>().size;
            CalculateDimensions();
            GenerateCells();
        }
        else
            Debug.Log("Failed to load prefab cell!");
    }
    private void CalculateDimensions()
    {
        width = CubeQty.Item1 * cellSize.x;
        height = CubeQty.Item2 * cellSize.z;
        centerOfGrid = new Tuple<float, float>((width + Offset * (CubeQty.Item1 - 1)) / 2 - cellSize.x / 2,
            (height + Offset * (CubeQty.Item2 - 1)) / 2 - cellSize.z / 2);
    }
    private void GenerateCells()
    {
        for (var i = 0; i < CubeQty.Item2; i++)
            for (var j = 0; j < CubeQty.Item1; j++)
            {
                var coord = new Tuple<float, float>(centerOfGrid.Item1 - j * cellSize.x - j * Offset,
                    centerOfGrid.Item2 - i * cellSize.z - i * Offset);
                var index = i * CubeQty.Item1 + j;
                cellsGameObject[index] = Instantiate(cellPrefab,
                    new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                cellsGameObject[index].transform.SetParent(GridGameObject.transform);
                cellsGameObject[index].transform.localPosition = new Vector3(coord.Item1, 0, coord.Item2);
                cellsGameObject[index].name = $"Cell ({index})";
            }
    }
}