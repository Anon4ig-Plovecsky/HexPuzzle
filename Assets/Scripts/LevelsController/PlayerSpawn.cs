using Unity.VisualScripting;
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
                return;
            }

            objPlayer.transform.position = transform.position;
            objPlayer.transform.rotation = transform.rotation;
        }
    }
}