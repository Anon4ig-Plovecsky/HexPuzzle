using UnityEngine.EventSystems;
using Valve.VR.Extras;
using UnityEngine;

namespace UI.TestedModules
{
    public class LaserHand : MonoBehaviour
    {
        private SteamVR_LaserPointer _steamVrLaserPointer;

        public bool Active
        {
            set => _steamVrLaserPointer.active = value;
        }

        private void Awake()
        {
            _steamVrLaserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
            if (_steamVrLaserPointer is null)
                return;
            
            _steamVrLaserPointer.PointerIn += OnPointerIn;
            _steamVrLaserPointer.PointerOut += OnPointerOut;
            _steamVrLaserPointer.PointerClick += OnPointerClick;
        }

        private bool _isSorted;
        private void OnPointerIn(object sender, PointerEventArgs e)
        {
            var pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();

            pointerEnterHandler?.OnPointerEnter(new PointerEventData(EventSystem.current));
        }
        private void OnPointerClick(object sender, PointerEventArgs e)
        {
            var pointerClickHandler = e.target.GetComponent<IPointerClickHandler>();

            pointerClickHandler?.OnPointerClick(new PointerEventData(EventSystem.current));
        }
        private void OnPointerOut(object sender, PointerEventArgs e)
        {
            var pointerExitHandler = e.target.GetComponent<IPointerExitHandler>();

            pointerExitHandler?.OnPointerExit(new PointerEventData(EventSystem.current));
        }
    }
}