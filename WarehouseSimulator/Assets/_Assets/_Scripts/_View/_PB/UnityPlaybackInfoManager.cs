using UnityEngine;
using UnityEngine.UIElements;
using WarehouseSimulator.Model.PB;

namespace WarehouseSimulator.View.Playback
{
    public class UnityPlaybackInfoManager : MonoBehaviour
    {
        #region Fields
        private UnsignedIntegerField _frameInputField;
        private Label _stepProgressLabel;
        private ProgressBar _stepProgressBar;
        private PlaybackData _playbackData;
        #endregion
        
        #region Unity Methods
        /// <summary>
        /// Initializes the playback info manager
        /// </summary>
        private void Start()
        {
            
            var doc = SceneHandler.GetInstance().CurrentDoc;
            
            //ProgressBar Setup
            _stepProgressBar = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<ProgressBar>("StepsProgressBar");
            _stepProgressLabel = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Label>("StepProgressLabel");
            _frameInputField = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("LeftSideBar")
                .Q("SettingsPanel")
                .Q<UnsignedIntegerField>("FrameInputField");

            _playbackData = this.GetComponentInParent<UnityPlaybackManager>().playbackData;
        }

        /// <summary>
        /// Updates the playback info per frame
        /// </summary>
        private void Update()
        {

            _stepProgressBar.value = _playbackData.CurrentStep;
            _stepProgressLabel.text = $"{_playbackData.CurrentStep}";
            _frameInputField.value = (uint) _playbackData.CurrentStep;
        }
        #endregion
    }
}