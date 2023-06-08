using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private AssetLabelReference assetLabelReferenceNormalMap;
    [SerializeField] private AssetLabelReference assetLabelReferenceTexture;
    [SerializeField] private GameObject pauseController;
    private readonly Tuple<int, int> cubeQty = new(3, 2); //[SerializeField]
    private GameObject cubesParent;
    //Cubes
    private readonly List<Transform> pointsOfSpawn = new();
    private HashSet<int> cubeSpawn = new();
    private GameObject[] cubesGameObjects;
    private GameObject[] cubesPrefab;
    private const int CubeSides = 6;
    private Shader cubeShader;
    //Paintings
    private readonly Tuple<ImageCreator, ImageCreator>[] partsOfPaintings = new Tuple<ImageCreator, ImageCreator>[CubeSides]; // [0] - Top
    private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");
    private HashSet<int> paintingIndexes = new();
    private bool mainPaintingIsCrop;
    private List<Sprite> normalMaps; //[0] - mainPainting
    private List<Sprite> paintings; //[0] - mainNormalMap
    private void Start()
    {
        cubesParent = GameObject.Find("Cubes");
        cubeShader = Shader.Find("Standard");
        cubesPrefab = new GameObject[cubeQty.Item1 * cubeQty.Item2];
        cubesGameObjects = new GameObject[cubesPrefab.Length];
        var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/PartOfPainting.prefab");
        asyncOperationHandle.Completed += delegate
        {
            if(asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                for (var i = 0; i < cubesPrefab.Length; i++)
                    cubesPrefab[i] = asyncOperationHandle.Result;
        };
        var asyncOperationListHandleNormalMap = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceNormalMap, _ => {});
        asyncOperationListHandleNormalMap.Completed += delegate
        {
            if(asyncOperationListHandleNormalMap.Status == AsyncOperationStatus.Succeeded)
                normalMaps = new List<Sprite>(asyncOperationListHandleNormalMap.Result.ToArray());
            else 
                Debug.Log("Failed to load Normal Maps!");
        };
        var pointsOfSpawnParent = GameObject.Find("PointsOfSpawn");
        for(var i = 0; i < pointsOfSpawnParent.transform.childCount; i++)
            pointsOfSpawn.Add(pointsOfSpawnParent.transform.GetChild(i));
        var asyncOperationIListHandleTexture = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceTexture, _ => {});
        asyncOperationIListHandleTexture.Completed += delegate { OnLoadDone(asyncOperationIListHandleTexture); };
    }
    private void Update()
    {
        if (CanvasController.classCanvasController is null || mainPaintingIsCrop) return;
        mainPaintingIsCrop = true;
        var mainSprite = partsOfPaintings[0].Item1.CropEntirePainting();
        StartCoroutine(CanvasController.classCanvasController.ShowImage(mainSprite,
            partsOfPaintings[0].Item1.dimension));
    }
    private void OnLoadDone(AsyncOperationHandle<IList<Sprite>> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            paintings = new List<Sprite>(asyncOperationHandle.Result.ToArray());
            while (normalMaps.Count != paintings.Count || cubesPrefab[^1] == null) {}
            ChooseImages();
            SpawnCubes();
            pauseController.SetActive(true);
        }
        else 
            Debug.Log("Failed to load paintings!");
    }
    private void ChooseImages()
    {
        paintingIndexes = GenerateNumbers(paintingIndexes, CubeSides, paintings.Count);
        for (var i = 0; i < CubeSides; i++)
        {
            partsOfPaintings[i] = new Tuple<ImageCreator, ImageCreator>(
                new ImageCreator(paintings[paintingIndexes.ToArray()[i]], cubeQty),
                new ImageCreator(normalMaps[paintingIndexes.ToArray()[i]], cubeQty));
            partsOfPaintings[i].Item1.CropPaintingByParts();
            partsOfPaintings[i].Item2.CropPaintingByParts();
        }
    }
    private void SpawnCubes()
    {
        cubeSpawn = GenerateNumbers(cubeSpawn, cubeQty.Item1 * cubeQty.Item2, pointsOfSpawn.Count);
        for (var cubeIndex = 0; cubeIndex < cubesPrefab.Length; cubeIndex++)
        {
            SetTexture(cubeIndex);
            cubesGameObjects[cubeIndex] = Instantiate(cubesPrefab[cubeIndex], pointsOfSpawn[cubeSpawn.ToArray()[cubeIndex]].position, 
                pointsOfSpawn[cubeSpawn.ToArray()[cubeIndex]].rotation);
            cubesGameObjects[cubeIndex].transform.Rotate(
                Random.Range(0, 4) * 90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90
                );
            cubesGameObjects[cubeIndex].name = $"PartOfPainting ({cubeIndex})";
            cubesGameObjects[cubeIndex].transform.SetParent(cubesParent.transform);
        }
    }
    private void SetTexture(int cubeIndex)
    {
        for (var side = 0; side < CubeSides; side++)
        {
            var imageMaterial = new Material(cubeShader)
            {
                mainTexture = partsOfPaintings[side].Item1.GetPartsOfPainting()[cubeIndex].texture
            };
            imageMaterial.EnableKeyword("_NORMALMAP");
            imageMaterial.SetTexture(BumpMap, partsOfPaintings[side].Item2.GetPartsOfPainting()[cubeIndex].texture);
            cubesPrefab[cubeIndex].transform.GetChild(0).GetChild(side).GetComponent<Renderer>().material =
                imageMaterial;
        }
    }
    private HashSet<int> GenerateNumbers(HashSet<int> hashSet, int size, int range)
    {
        while (hashSet.Count < size)
            hashSet.Add(Random.Range(0, range));
        return new HashSet<int>(hashSet);
    }
}