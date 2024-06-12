using UnityEngine.ResourceManagement.AsyncOperations;
using LevelsController.TestedModules;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using Unity.VisualScripting;
using UI.TestedModules;
using System.Linq;
using UnityEngine;
using System;

namespace LevelsController
{
    public class CubeGenerator : MonoBehaviour
    {
        // Receiving level information from another scene
        private LevelInfoTransfer _levelInfo;
        
        // Input data
        [SerializeField] private AssetLabelReference assetLabelReferenceNormalMap;
        [SerializeField] private AssetLabelReference assetLabelReferenceTexture;
        [SerializeField] private GameObject pauseController;
        private Tuple<int, int> _gridSize;                                          // Grid size
    
        private GameObject _cubesParent;
    
        //Cubes
        private readonly List<Transform> _pointsOfSpawn = new();    // Point objects for cube spawning
        private HashSet<int> _hashIndexesOfSpawn = new();           // Indexes of spawn points to be used
    
        private GameObject[] _cubesGameObjects;                     
        private GameObject _prefabCube;
    
        private Shader _cubeShader;
    
        //Paintings
        private readonly Tuple<ImageCreator, ImageCreator>[] _partsOfPaintings = new Tuple<ImageCreator, ImageCreator>[CommonKeys.CubeSides]; // [0] - Top
        private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");
        private HashSet<int> _paintingIndexes = new();
        private bool _mainPaintingIsCrop;
        private List<Sprite> _normalMaps; //[0] - mainPainting
        private List<Sprite> _paintings;  //[0] - mainNormalMap
        private string _strMainPainting;
        private async void Start()
        {
            _cubesParent = GameObject.Find("Cubes");
            _cubeShader = Shader.Find("Standard");
            
            _levelInfo = LevelInfoTransfer.GetInstance();
            _gridSize = _levelInfo.GridSize;
            if (_gridSize is null || _gridSize.Item1 == 0 || _gridSize.Item2 == 0)
            {
                Debug.Log("Failed to get grid size");
                _gridSize = new Tuple<int, int>(2, 3);
            }
        
            _cubesGameObjects = new GameObject[_gridSize.Item1 * _gridSize.Item2];

            var asyncTask = CommonKeys.LoadResource<GameObject>(CommonKeys.Addressable.PartOfPainting);
            await asyncTask;
            _prefabCube = asyncTask.Result;
            
            var asyncOperationListHandleNormalMap = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceNormalMap, _ => {});
            asyncOperationListHandleNormalMap.Completed += delegate
            {
                if(asyncOperationListHandleNormalMap.Status == AsyncOperationStatus.Succeeded)
                    _normalMaps = new List<Sprite>(asyncOperationListHandleNormalMap.Result.ToArray());
                else 
                    Debug.Log("Failed to load Normal Maps!");
            };
            await asyncOperationListHandleNormalMap.Task;
            
            var pointsOfSpawnParent = GameObject.Find(CommonKeys.Names.PointsOfSpawn);
            for(var i = 0; i < pointsOfSpawnParent.transform.childCount; i++)
                _pointsOfSpawn.Add(pointsOfSpawnParent.transform.GetChild(i));
            var asyncOperationIListHandleTexture = Addressables.LoadAssetsAsync<Sprite>(assetLabelReferenceTexture, _ => {});
            asyncOperationIListHandleTexture.Completed += delegate { OnLoadDone(asyncOperationIListHandleTexture); };
        }
        private void Update()
        {
            if (CanvasController.ClassCanvasController is null || _mainPaintingIsCrop) 
                return;
            
            _mainPaintingIsCrop = true;
            var mainSprite = _partsOfPaintings[0].Item1.CropEntirePainting();
            StartCoroutine(CanvasController.ClassCanvasController.
                ShowImage(mainSprite, _partsOfPaintings[0].Item1.Dimension, _strMainPainting));
        }
        private void OnLoadDone(AsyncOperationHandle<IList<Sprite>> asyncOperationHandle)
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _paintings = new List<Sprite>(asyncOperationHandle.Result.ToArray());
                if (_normalMaps == null || _paintings == null || 
                    _normalMaps.Count != _paintings.Count || _prefabCube.IsUnityNull())
                {
                    Debug.Log("Insufficient resources to spawn cubes");
                    return;
                }

                ChooseImages();
                SpawnCubes();
                pauseController.SetActive(true);
                return;
            }
            
            Debug.Log("Failed to load paintings!");
        }
        
        /// <summary>
        /// Loads images specified from LevelInfoTransfer; if missing or insufficient, randomly loads them from resources
        /// </summary>
        private void ChooseImages()
        {
            var iCounter = 0;

            // Adding images by given names when loading the scene
            var listStrImagesNames = _levelInfo.ImageNameList;
            if (listStrImagesNames is null || listStrImagesNames.Count == 0)
                Debug.Log("The ImageNameList array in LevelInfoTransfer is empty or not initialized");
            else
            {
                _strMainPainting = listStrImagesNames[0];
                foreach (var iFoundRes in listStrImagesNames
                             .Select(imageName => _paintings.FindIndex(
                                 sprite => sprite.name.Equals(imageName)))
                             .Where(iFoundRes => iFoundRes != -1))
                    _partsOfPaintings[iCounter++] = CreatePartOfPainting(_paintings[iFoundRes],
                        _normalMaps[iFoundRes]);
            }

            // If the array is completely full - return
            if(CommonKeys.CubeSides == iCounter)
                return;
            
            // Removing from the list textures that have already been used
            if(listStrImagesNames != null && listStrImagesNames.Count != 0)
            {
                _paintings.RemoveAll(sprite => listStrImagesNames.Contains(sprite.name));
                _normalMaps.RemoveAll(sprite => listStrImagesNames.Contains(sprite.name));
            }

            _paintingIndexes = CommonKeys.GenerateNumbers( CommonKeys.CubeSides - iCounter, _paintings.Count);
            for (var i = iCounter; i < CommonKeys.CubeSides; i++)
            {
                var iPaintingIndex = _paintingIndexes.ToArray()[i - iCounter];
                if (i == 0)
                    _strMainPainting = _paintings[iPaintingIndex].name;
                _partsOfPaintings[i] = CreatePartOfPainting(_paintings[iPaintingIndex],
                    _normalMaps[_paintingIndexes.ToArray()[i - iCounter]]);
            }
        }
        
        /// <summary>
        /// Creates fragments of the selected image and normal maps
        /// </summary>
        /// <param name="painting">Sprite image texture</param>
        /// <param name="normalMap">Sprite image normal map</param>
        /// <returns>Returns a pair of sets of image tiles and a normal map</returns>
        private Tuple<ImageCreator, ImageCreator> CreatePartOfPainting(Sprite painting, Sprite normalMap)
        {
            var partsOfPainting = new Tuple<ImageCreator, ImageCreator>(
                new ImageCreator(painting, _gridSize),
                new ImageCreator(normalMap, _gridSize));
            partsOfPainting.Item1.CropPaintingByParts();
            partsOfPainting.Item2.CropPaintingByParts();
            return partsOfPainting;
        }
        
        private void SpawnCubes()
        {
            _hashIndexesOfSpawn = CommonKeys.GenerateNumbers(_gridSize.Item1 * _gridSize.Item2, _pointsOfSpawn.Count);
            for (var cubeIndex = 0; cubeIndex < _cubesGameObjects.Length; cubeIndex++)
            {
                _cubesGameObjects[cubeIndex] = Instantiate(_prefabCube, _pointsOfSpawn[_hashIndexesOfSpawn.ToArray()[cubeIndex]].position, 
                    _pointsOfSpawn[_hashIndexesOfSpawn.ToArray()[cubeIndex]].rotation);
                SetTexture(cubeIndex);
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
                _cubesGameObjects[cubeIndex].transform.GetChild(0).GetChild(side).GetComponent<Renderer>().material =
                    imageMaterial;
            }
        }
    }
}