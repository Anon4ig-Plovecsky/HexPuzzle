using Unity.VisualScripting;
using System.Linq;
using LevelsController.TestedModules;
using UnityEngine;

namespace LevelsController
{
    /// <summary>
    /// A class that transports the player to the starting point of the location
    /// </summary>
    public class PlayerSpawn : MonoBehaviour
    {
        private void Start()
        {
            var objPlayer = GameObject.Find(CommonKeys.Names.Player);
            if (objPlayer.IsUnityNull())
            {
                Debug.Log("Player not found, failed to move player to starting position");

                var levelInfoTransfer = LevelInfoTransfer.GetInstance();
                if (levelInfoTransfer.IsTestMode)
                    return;
                
                var objPlayerDebug = FindObjectsOfType<GameObject>(true).First(obj => 
                    obj.name.Equals(CommonKeys.Names.PlayerDebug));
                if(!objPlayerDebug.IsUnityNull())
                    objPlayerDebug.SetActive(true);
                
                return;
            }

            objPlayer.transform.position = transform.position;
            objPlayer.transform.rotation = transform.rotation;
        }
    }
}