using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class MainLevelsController : MonoBehaviour
    {
        private List<Button> _btnMainLevelList = new();
        
        // Start is called before the first frame update
        void Start()
        {
            // Initially, leave the panel active in order to load all levels into the panel in advance
            gameObject.SetActive(false);
            
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
