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
    private readonly HashSet<int> cubeSpawn = new();
    private GameObject[] cubesGameObjects;
    private GameObject[] cubesPrefab;
    private Shader cubeShader;
    //Paintings
    private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");
    private Tuple<ImageCreator, ImageCreator> imageCreator;
    private List<Sprite> normalMaps;
    private List<Sprite> paintings;
    private Sprite mainNormalMap;
    private Sprite mainPainting;
    private void Start()
    {
        cubesParent = GameObject.Find("Cubes");
        cubeShader = Shader.Find("Standard");
        cubesPrefab = new GameObject[cubeQty.Item1 * cubeQty.Item2];
        cubesGameObjects = new GameObject[cubesPrefab.Length];
        var asyncOperationListHandleNormalMap = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceNormalMap, _ => {});
        asyncOperationListHandleNormalMap.Completed += delegate
        {
            if(asyncOperationListHandleNormalMap.Status == AsyncOperationStatus.Succeeded)
                normalMaps = new List<Sprite>(asyncOperationListHandleNormalMap.Result.ToArray());
            else 
                Debug.Log("Failed to load Normal Maps!");
        };
        var asyncOperationIListHandleTexture = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceTexture, _ => {});
        asyncOperationIListHandleTexture.Completed += delegate { OnLoadDone(asyncOperationIListHandleTexture); };
        var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/PartOfPainting.prefab");
        asyncOperationHandle.Completed += delegate
        {
            if(asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                for (var i = 0; i < cubesPrefab.Length; i++)
                    cubesPrefab[i] = asyncOperationHandle.Result;
        };
        var pointsOfSpawnParent = GameObject.Find("PointsOfSpawn");
        for(var i = 0; i < pointsOfSpawnParent.transform.childCount; i++)
            pointsOfSpawn.Add(pointsOfSpawnParent.transform.GetChild(i));
    }
    private void OnLoadDone(AsyncOperationHandle<IList<Sprite>> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            paintings = new List<Sprite>(asyncOperationHandle.Result.ToArray());
            while (normalMaps.Count != paintings.Count) {}
            ChooseImage();
            SpawnCubes();
        }
        else 
            Debug.Log("Failed to load paintings!");
    }
    private void ChooseImage()
    {
        var mainIndex = Random.Range(0, paintings.Count);
        mainPainting = paintings[mainIndex];
        mainNormalMap = normalMaps[mainIndex];
        imageCreator = new Tuple<ImageCreator, ImageCreator>(new ImageCreator(mainPainting, cubeQty),
            new ImageCreator(mainNormalMap, cubeQty));
        imageCreator.Item1.CropPainting();
        imageCreator.Item2.CropPainting();
    }
    private void SpawnCubes()
    {
        while(cubeSpawn.Count < cubeQty.Item1 * cubeQty.Item2)
            cubeSpawn.Add(Random.Range(0, pointsOfSpawn.Count));
        for (var i = 0; i < cubesPrefab.Length; i++)
        {
            var material = new Material(cubeShader)
            {
                mainTexture = imageCreator.Item1.GetPartsOfPainting()[i].texture
                
            };
            material.EnableKeyword("_NORMALMAP");
            material.SetTexture(BumpMap, imageCreator.Item2.GetPartsOfPainting()[i].texture);
            cubesPrefab[i].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = material;
            cubesGameObjects[i] = Instantiate(cubesPrefab[i], pointsOfSpawn[cubeSpawn.ToArray()[i]].position, 
                pointsOfSpawn[cubeSpawn.ToArray()[i]].rotation);
            cubesGameObjects[i].transform.Rotate(
                Random.Range(0, 4) * 90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90
                );
            cubesGameObjects[i].name = $"PartOfPainting ({i})";
            cubesGameObjects[i].transform.SetParent(cubesParent.transform);
            cubesGameObjects[i].transform.GetChild(1).gameObject.SetActive(true);
        }
        pauseController.SetActive(true);
    }
}