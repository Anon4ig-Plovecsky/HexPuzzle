using CommonScripts.TestedModules;
using UnityEngine;

namespace LevelsController
{
    public class ObjectCollision : MonoBehaviour
    {
        [SerializeField] private SoundsController.ObjectType objectType;
        private SoundsController _soundsController;

        public void Start()
        {
            var objSoundsController = GameObject.Find(CommonKeys.Names.SoundsController);
            if (objSoundsController is null)
                return;
            _soundsController = objSoundsController.GetComponent<SoundsController>();
        }
        
        /// <summary>
        /// If an object collides with other objects, it plays an impact sound
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            _soundsController.PlaySound(objectType);
        }
    }
}