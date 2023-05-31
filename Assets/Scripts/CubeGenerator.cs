using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private AssetLabelReference assetLabelReference;
    private readonly Tuple<int, int> cubeQty = new(3, 2); //[SerializeField]
    private GameObject cubesParent;
    //Cubes
    private readonly List<Transform> pointsOfSpawn = new();
    private readonly HashSet<int> cubeSpawn = new();
    private GameObject[] cubesGameObjects;
    private GameObject[] cubesPrefab;
    private Shader cubeShader;
    //Paintings
    private ImageCreator imageCreator;
    private List<Sprite> paintings;
    private Sprite mainPainting;
    private void Start()
    {
        cubesParent = GameObject.Find("Cubes");
        cubeShader = Shader.Find("Standard");
        cubesPrefab = new GameObject[cubeQty.Item1 * cubeQty.Item2];
        cubesGameObjects = new GameObject[cubesPrefab.Length];
        var asyncOperationIListHandle = Addressables.LoadAssetsAsync<Sprite>(assetLabelReference, _ => {});
        asyncOperationIListHandle.Completed += delegate { OnLoadDone(asyncOperationIListHandle); };
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
            ChooseImage();
            SpawnCubes();
        }
        else 
            Debug.Log("Failed to load paintings!");
    }
    private void ChooseImage()
    {
        mainPainting = paintings[Random.Range(0, paintings.Count)];
        imageCreator = new ImageCreator(mainPainting, cubeQty);
        imageCreator.CropPainting();
    }
    private void SpawnCubes()
    {
        while(cubeSpawn.Count < cubeQty.Item1 * cubeQty.Item2)
            cubeSpawn.Add(Random.Range(0, pointsOfSpawn.Count));
        for (var i = 0; i < cubesPrefab.Length; i++)
        {
            var material = new Material(cubeShader)
            {
                mainTexture = imageCreator.GetPartsOfPainting()[i].texture
            };
            cubesPrefab[i].transform.GetChild(0).GetComponent<Renderer>().material = material;
            cubesGameObjects[i] = Instantiate(cubesPrefab[i], pointsOfSpawn[cubeSpawn.ToArray()[i]].position, 
                pointsOfSpawn[cubeSpawn.ToArray()[i]].rotation);
            cubesGameObjects[i].name = $"PartOfPainting ({i})";
            cubesGameObjects[i].transform.SetParent(cubesParent.transform);
        }
    }
}