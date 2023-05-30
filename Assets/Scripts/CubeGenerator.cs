using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private AssetLabelReference assetLabelReference; 
    private readonly Tuple<int, int> cubeQty = new(3, 2); //[SerializeField]
    private ImageCreator imageCreator;
    private List<Sprite> paintings;
    private Sprite mainPainting;
    private void Start()
    {
        var asyncOperationHandle = Addressables.LoadAssetsAsync<Sprite>(assetLabelReference, _ => {});
        asyncOperationHandle.Completed += delegate { OnLoadDone(asyncOperationHandle); };
    }
    private void OnLoadDone(AsyncOperationHandle<IList<Sprite>> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            paintings = new List<Sprite>(asyncOperationHandle.Result.ToArray());
            ChooseImage();
        }
        else 
            Debug.Log("Failed to load paintings!");
    }
    private void ChooseImage()
    {
        mainPainting = paintings[Random.Range(0, paintings.Count)];
        imageCreator = new ImageCreator(mainPainting, cubeQty);
    }
}
