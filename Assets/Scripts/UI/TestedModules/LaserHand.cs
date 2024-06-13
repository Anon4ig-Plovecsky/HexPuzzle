using CommonScripts.TestedModules;
using UnityEngine.EventSystems;
using Valve.VR.Extras;
using UnityEngine;
using Valve.VR;

namespace UI.TestedModules
{
    public class LaserHand : MonoBehaviour
    {
        // Controller
        private SteamVR_LaserPointer _steamVrLaserPointer;
        [SerializeField] private SteamVR_Action_Boolean actionBooleanClick;
        [SerializeField] private SteamVR_Action_Pose actionPoseHand;

        private const SteamVR_Input_Sources InputSource = SteamVR_Input_Sources.RightHand;
        private SoundsController _soundsController;
        private GameObject _laserPointer;
        private bool _bInScrollView;
        private bool _bIsDrag;
        
        // Sounds
        private AudioClip _audioLaserHover;
        private AudioClip _audioLaserClick;
        
        //Scrolling
        private GameObject _objContentView;
        
        // Controller position
        private Vector3 _firstPosition;

        public bool Active
        {
            set
            {
                if (_steamVrLaserPointer is null)
                    return;
                _laserPointer ??= _steamVrLaserPointer.transform.GetChild(5).gameObject;
                _laserPointer ??= _steamVrLaserPointer.transform.GetChild(5).gameObject;
                if(_laserPointer != null)
                    _laserPointer.SetActive(value);
            } 
        }

        /// <summary>
        /// Helper that overrides soundsController for player after scene change
        /// </summary>
        public void ReactivateSoundsController()
        {
            // When transferred to another scene, player is not destroyed, so to use it need
            // to catch the receipt of _soundsController the first time
            var objSoundsController = GameObject.Find(CommonKeys.Names.SoundsController);
            if (objSoundsController is null)
                return;
            _soundsController = objSoundsController.GetComponent<SoundsController>();
        }

        private async void OnEnable()
        {
            // Retrieving Audio Variables
            var asyncTask = CommonKeys.LoadResource<AudioClip>(CommonKeys.StrAudioNames.LaserHover);
            await asyncTask;
            _audioLaserHover = asyncTask.Result;
            asyncTask = CommonKeys.LoadResource<AudioClip>(CommonKeys.StrAudioNames.LaserClick);
            await asyncTask;
            _audioLaserClick = asyncTask.Result;
            var objSoundsController = GameObject.Find(CommonKeys.Names.SoundsController);
            if (objSoundsController is null)
                return;
            _soundsController = objSoundsController.GetComponent<SoundsController>();
            
            _steamVrLaserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
            if (_steamVrLaserPointer is null)
                return;
            
            _steamVrLaserPointer.PointerIn += OnPointerIn;
            _steamVrLaserPointer.PointerOut += OnPointerOut;
            _steamVrLaserPointer.PointerClick += OnPointerClick;
        }
        private void OnPointerIn(object sender, PointerEventArgs e)
        {
            var pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();

            if(pointerEnterHandler != null)
            {
                pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));
                _soundsController.PlaySound(SoundsController.AudioSourceType.SfxSource, _audioLaserHover);
            }
            
            // Checking if we are interacting with the ScrollView
            if (e.target.CompareTag(CommonKeys.Names.ScrollElement))
            {
                var scrollItem = e.target.gameObject.GetComponent<IScrollItem>();
                if (scrollItem is null)
                    return;

                _objContentView = scrollItem.GetContentView();
                _bInScrollView = true;
            }
        }
        private void OnPointerClick(object sender, PointerEventArgs e)
        {
            // If we work with ScrollView, then we do not pay attention to clicks
            if (_bIsDrag)
                return;
            
            var pointerClickHandler = e.target.GetComponent<IPointerClickHandler>();

            if (pointerClickHandler == null) 
                return;
            pointerClickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
            _soundsController.PlaySound(SoundsController.AudioSourceType.SfxSource, _audioLaserClick);
        }
        private void OnPointerOut(object sender, PointerEventArgs e)
        {
            if (_bInScrollView)
                _bInScrollView = false;
            
            var pointerExitHandler = e.target.GetComponent<IPointerExitHandler>();

            pointerExitHandler?.OnPointerExit(new PointerEventData(EventSystem.current));
        }

        /// <summary>
        /// If we hold down the key when hovering over the ScrollView or its element,
        /// then we move the ScrollView relative to the position
        /// </summary>
        private void Update()
        {
            // If you release the key, remove the move flag
            if (_bIsDrag && !actionBooleanClick.GetState(InputSource))
                _bIsDrag = false;
            
            // Note: If leaves the boundaries of the ScrollView, then drag is not disabled. 
            // P.S. It seems like everything works without disabling drag, because
            // objects do not physically disappear and laserPoint continues to catch
            // them even after they disappear beyond the mask
            if (!_bInScrollView || !actionBooleanClick.GetState(InputSource))
            {
                if(!_firstPosition.Equals(Vector3.negativeInfinity))
                    _firstPosition = Vector3.negativeInfinity;
                return;
            }

            // Save the original position of the controller
            if (_firstPosition.Equals(Vector3.negativeInfinity))
                _firstPosition = actionPoseHand.GetLocalPosition(InputSource);

            // If you have moved the active cursor a sufficient number of millimeters, then enable ScrollView dragging
            if (!_bIsDrag && Mathf.Abs(_firstPosition.y - actionPoseHand.GetLocalPosition(InputSource).y) > CommonKeys.DragLength)
                _bIsDrag = true;

            // Scrolling
            if (!_bIsDrag)
                return;
            var lastPosePosition = actionPoseHand.GetLastLocalPosition(InputSource);
            var currentPosePosition = actionPoseHand.GetLocalPosition(InputSource);
            var dPos = currentPosePosition.y - lastPosePosition.y;
            _objContentView.transform.localPosition += new Vector3(0, dPos * 2.0f, 0);
        }

        private void OnDisable()
        {
            _steamVrLaserPointer.PointerClick -= OnPointerClick;
            _steamVrLaserPointer.PointerOut -= OnPointerOut;
            _steamVrLaserPointer.PointerIn -= OnPointerIn;
        }
    }
}