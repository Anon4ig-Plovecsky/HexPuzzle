using LevelsController.TestedModules;
using Unity.VisualScripting;
using UI.TestedModules;
using System.Linq;
using UnityEngine;

namespace LevelsController
{
    /// <summary>
    /// A class that transports the player to the starting point of the location
    /// </summary>
    public class PlayerSpawn : MonoBehaviour
    {
        private static bool _isPlayerSpawned;
        [SerializeField] private GameObject prefabObjPlayer;
        
        private void Start()
        {
            var objPlayer = GameObject.Find(CommonKeys.Names.Player);
            if (objPlayer.IsUnityNull())
            {
                Debug.Log("Player not found, failed to move player to starting position");

                var levelInfoTransfer = LevelInfoTransfer.GetInstance();
                if (levelInfoTransfer.IsTestMode)
                    return;
                
                // When you first start, create a player
                if (!_isPlayerSpawned)
                {
                    var objSpawn = Instantiate(prefabObjPlayer, Vector3.zero, Quaternion.identity);
                    objSpawn.name = CommonKeys.Names.Player;
                    _isPlayerSpawned = true;
                    return;
                }
                
                var objPlayerDebug = FindObjectsOfType<GameObject>(true).First(obj => 
                    obj.name.Equals(CommonKeys.Names.PlayerDebug));
                if(!objPlayerDebug.IsUnityNull())
                    objPlayerDebug.SetActive(true);
                
                return;
            }

            // Reactivating soundsController
            var objRightHand = objPlayer.transform.GetChild(0).GetChild(2);
            if (objRightHand is null)
                return;
            var laserHand = objRightHand.GetComponent<LaserHand>();
            if (laserHand is null)
            {
                Debug.Log("Failed to get LaserHand from RightHand");
                return;
            }
            laserHand.ReactivateSoundsController();
            
            objPlayer.transform.position = transform.position;
            objPlayer.transform.rotation = transform.rotation;
        }
    }
}