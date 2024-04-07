using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UI;

namespace LevelsController
{
    public class CubeGenerator : MonoBehaviour
    {
        // Input data
        [SerializeField] private AssetLabelReference assetLabelReferenceNormalMap;
        [SerializeField] private AssetLabelReference assetLabelReferenceTexture;
        [SerializeField] private GameObject pauseController;
        private readonly Tuple<int, int> _cubeQty = new(3, 2);      //[SerializeField]
    
        private GameObject _cubesParent;
    
        //Cubes
        private readonly List<Transform> _pointsOfSpawn = new();    // Point objects for cube spawning
        private HashSet<int> _hashIndexesOfSpawn = new();           // Indexes of spawn points to be used
    
        private GameObject[] _cubesGameObjects;                     
        private GameObject[] _cubesPrefab;
    
        private Shader _cubeShader;
    
        //Paintings
        private readonly Tuple<ImageCreator, ImageCreator>[] _partsOfPaintings = new Tuple<ImageCreator, ImageCreator>[CommonKeys.CubeSides]; // [0] - Top
        private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");
        private HashSet<int> _paintingIndexes = new();
        private bool _mainPaintingIsCrop;
        private List<Sprite> _normalMaps; //[0] - mainPainting
        private List<Sprite> _paintings; //[0] - mainNormalMap
        private void Start()
        {
            _cubesParent = GameObject.Find("Cubes");
            _cubeShader = Shader.Find("Standard");
        
            _cubesPrefab = new GameObject[_cubeQty.Item1 * _cubeQty.Item2];
            _cubesGameObjects = new GameObject[_cubesPrefab.Length];
        
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(CommonKeys.Addressable.PartOfPainting);
            asyncOperationHandle.Completed += delegate
            {
                if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
                    return;
                
                for (var i = 0; i < _cubesPrefab.Length; i++)
                    _cubesPrefab[i] = asyncOperationHandle.Result;
            };
            var asyncOperationListHandleNormalMap = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceNormalMap, _ => {});
            asyncOperationListHandleNormalMap.Completed += delegate
            {
                if(asyncOperationListHandleNormalMap.Status == AsyncOperationStatus.Succeeded)
                    _normalMaps = new List<Sprite>(asyncOperationListHandleNormalMap.Result.ToArray());
                else 
                    Debug.Log("Failed to load Normal Maps!");
            };
            var pointsOfSpawnParent = GameObject.Find("PointsOfSpawn");
            for(var i = 0; i < pointsOfSpawnParent.transform.childCount; i++)
                _pointsOfSpawn.Add(pointsOfSpawnParent.transform.GetChild(i));
            var asyncOperationIListHandleTexture = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceTexture, _ => {});
            asyncOperationIListHandleTexture.Completed += delegate { OnLoadDone(asyncOperationIListHandleTexture); };
        }
        private void Update()
        {
            if (CanvasController.ClassCanvasController is null || _mainPaintingIsCrop) return;
            _mainPaintingIsCrop = true;
            var mainSprite = _partsOfPaintings[0].Item1.CropEntirePainting();
            StartCoroutine(CanvasController.ClassCanvasController.ShowImage(mainSprite,
                _partsOfPaintings[0].Item1.Dimension));
        }
        private void OnLoadDone(AsyncOperationHandle<IList<Sprite>> asyncOperationHandle)
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _paintings = new List<Sprite>(asyncOperationHandle.Result.ToArray());
                while (_normalMaps.Count != _paintings.Count || _cubesPrefab[^1] == null) {}
                ChooseImages();
                SpawnCubes();
                pauseController.SetActive(true);
            }
            else 
                Debug.Log("Failed to load paintings!");
        }
        private void ChooseImages()
        {
            _paintingIndexes = GenerateNumbers(_paintingIndexes, CommonKeys.CubeSides, _paintings.Count);
            for (var i = 0; i < CommonKeys.CubeSides; i++)
            {
                _partsOfPaintings[i] = new Tuple<ImageCreator, ImageCreator>(
                    new ImageCreator(_paintings[_paintingIndexes.ToArray()[i]], _cubeQty),
                    new ImageCreator(_normalMaps[_paintingIndexes.ToArray()[i]], _cubeQty));
                _partsOfPaintings[i].Item1.CropPaintingByParts();
                _partsOfPaintings[i].Item2.CropPaintingByParts();
            }
        }
        private void SpawnCubes()
        {
            _hashIndexesOfSpawn = GenerateNumbers(_hashIndexesOfSpawn, _cubeQty.Item1 * _cubeQty.Item2, _pointsOfSpawn.Count);
            for (var cubeIndex = 0; cubeIndex < _cubesPrefab.Length; cubeIndex++)
            {
                SetTexture(cubeIndex);
                _cubesGameObjects[cubeIndex] = Instantiate(_cubesPrefab[cubeIndex], _pointsOfSpawn[_hashIndexesOfSpawn.ToArray()[cubeIndex]].position, 
                    _pointsOfSpawn[_hashIndexesOfSpawn.ToArray()[cubeIndex]].rotation);
                _cubesGameObjects[cubeIndex].transform.Rotate(
                    Random.Range(0, 4) * 90, Random.Range(0, 4) * 90, Random.Range(0, 4) * 90
                );
                _cubesGameObjects[cubeIndex].name = $"PartOfPainting ({cubeIndex})";
                _cubesGameObjects[cubeIndex].transform.SetParent(_cubesParent.transform);
            }
        }
        private void SetTexture(int cubeIndex)
        {
            for (var side = 0; side < CommonKeys.CubeSides; side++)
            {
                var imageMaterial = new Material(_cubeShader)
                {
                    mainTexture = _partsOfPaintings[side].Item1.GetPartsOfPainting()[cubeIndex].texture
                };
                imageMaterial.EnableKeyword("_NORMALMAP");
                imageMaterial.SetTexture(BumpMap, _partsOfPaintings[side].Item2.GetPartsOfPainting()[cubeIndex].texture);
                _cubesPrefab[cubeIndex].transform.GetChild(0).GetChild(side).GetComponent<Renderer>().material =
                    imageMaterial;
            }
        }
        private static HashSet<int> GenerateNumbers(ISet<int> hashSet, int size, int range)
        {
            while (hashSet.Count < size)
                hashSet.Add(Random.Range(0, range));
            return new HashSet<int>(hashSet);
        }
    }
}