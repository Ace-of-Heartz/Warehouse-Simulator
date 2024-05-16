using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using WarehouseSimulator.Model.PB;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.View
{
    public class BindingSetupManager : MonoBehaviour
    {
        #region Fields
        private Action _pauseAction;
        private Action _resumeAction;
        private Action _toggleButtonFocusableAction;
        #endregion
        
        #region Public methods 
        /// <summary>
        /// Setup for all bindings for the Simulation's UI
        /// </summary>
        /// <param name="man"></param>
        public void SetupSimBinding(SimulationManager man)
        {
            var doc = SceneHandler.GetDocOfID(1);
            var abortButton = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomRight")
                .Q("ButtonBox")
                .Q<Button>("Button_Abort");
            var startPBButton = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomRight")
                .Q("ButtonBox")
                .Q<Button>("Button_StartPB");

            
            Action startPBAction = () =>
            {
                if (man.SimulationData._isFinished)
                {
                    SceneHandler.GetInstance().SetCurrentScene(2);
                    SceneManager.LoadScene(SceneHandler.GetInstance().CurrentScene);
                    UIMessageManager.GetInstance().SetUIDocument(SceneHandler.GetInstance().CurrentDoc);
                }
            };
            startPBButton.clickable.clicked += startPBAction;
            
            
            Action<MessageBoxResponse> onDone = (response) =>
            {
                if (response == MessageBoxResponse.CONFIRMED)
                {
                    SceneHandler.GetInstance().SetCurrentScene(0);
                    SceneManager.LoadScene(SceneHandler.GetInstance().CurrentScene);
                    
                    UIMessageManager.GetInstance().SetUIDocument(SceneHandler.GetInstance().CurrentDoc);
                }
            };
            
            SetupSceneSwitchButton(abortButton,onDone,"Abort simulation?");
            
            var progressBar = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<ProgressBar>("StepsProgressBar");
            var progressLabel = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Label>("StepProgressLabel");
            var maxProgressLabel = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Label>("MaxStepProgressLabel");
           
            SetupStepsProgressBar(maxProgressLabel,progressLabel,progressBar,man.SimulationData._maxStepAmount);
        
            var goalAddButton = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("RightSideBar")
                .Q("GoalInputPanel")
                .Q<Button>("GoalInputAddButton");
            var goalInputField = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("RightSideBar")
                .Q("GoalInputPanel")
                .Q<Vector2IntField>("GoalInputPositionField");
            
            SetupDynamicGoalAddition(goalAddButton,goalInputField,man);
            
            
        }

                /// <summary>
        /// Setup for all bindings for the Playback's UI
        /// </summary>
        /// <param name="man"></param>
        public void SetupPlaybackBinding(PlaybackManager man)
        {
            
            var doc = SceneHandler.GetDocOfID(2);

            var progressBar = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<ProgressBar>("StepsProgressBar");
            var progressLabel = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Label>("StepProgressLabel");
            var maxProgressLabel = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Label>("MaxStepProgressLabel");
            
            SetupStepsProgressBar(maxProgressLabel,progressLabel,progressBar,man.PlaybackData.MaxStepAmount);
 
            //FrameInputField Setup
            var frameInputField = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("LeftSideBar")
                .Q("SettingsPanel")
                .Q<UnsignedIntegerField>("FrameInputField");
            frameInputField.RegisterValueChangedCallback((_) =>
            {
                man.SetTimeTo((int) frameInputField.value);
            });
            
            //PlaybackSpeedSlider Setup
            var playbackSpeed = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("LeftSideBar")
                .Q("SettingsPanel")
                .Q<Slider>("PbSpeedSlider");
            playbackSpeed.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                man.PlaybackData.PlaybackSpeed = evt.newValue;
            });

            //PauseButton and ResumeButton Setup
            var pauseButton = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Button>("Button_Pause");
            var resumeButton = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Button>("Button_Resume");

            _pauseAction = () =>
            {
                if(!man.PlaybackData.IsPaused)
                    man.PlaybackData.ChangePauseState();
                resumeButton.style.display = DisplayStyle.Flex;
                pauseButton.style.display = DisplayStyle.None;
            };
            _resumeAction = () =>
            {
                if(man.PlaybackData.IsPaused)
                    man.PlaybackData.ChangePauseState();
                resumeButton.style.display = DisplayStyle.None;
                pauseButton.style.display = DisplayStyle.Flex;
            };
            _toggleButtonFocusableAction = () =>
            {
                pauseButton.SetEnabled(!resumeButton.enabledSelf);
                resumeButton.SetEnabled(!resumeButton.enabledSelf);
            };

            pauseButton.clickable.clicked += _pauseAction;
            resumeButton.clickable.clicked += _resumeAction;
            
            
            //StepBackButton Setup
            var stepBackButton = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Button>("Button_StepBack");
            stepBackButton.clickable.clicked += man.PreviousState;
            
            
            //StepForwardButton Setup
            var stepForwardButton = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Button>("Button_StepForward");
            stepForwardButton.clickable.clicked += man.NextState;

            //ExitButton Setup
            var exitButton = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("BottomBar")
                .Q("BottomRight")
                .Q<Button>("Button_Exit");
            
            Action<MessageBoxResponse> onDone = (response) =>
            {
                switch (response)
                {
                    case MessageBoxResponse.CONFIRMED:
                        _resumeAction();
                        _toggleButtonFocusableAction();
                        
                        pauseButton.clickable.clicked -= _pauseAction;
                        resumeButton.clickable.clicked -= _resumeAction;
                        exitButton.clickable.clicked -= _pauseAction;
                        exitButton.clickable.clicked -= _toggleButtonFocusableAction;

                        
                        SceneHandler.GetInstance().SetCurrentScene(0);
                        SceneManager.LoadScene(SceneHandler.GetInstance().CurrentScene);
                        UIMessageManager.GetInstance().SetUIDocument(SceneHandler.GetInstance().CurrentDoc);
                        break;
                    case MessageBoxResponse.CANCELED:
                        _resumeAction();
                        _toggleButtonFocusableAction();
                        
                        break;
                    default: 
                        throw new ArgumentOutOfRangeException();
                        
                }
            };
            SetupSceneSwitchButton(exitButton,onDone,"Exit playback?");
            exitButton.clickable.clicked += _pauseAction;
            exitButton.clickable.clicked += _toggleButtonFocusableAction;

            man.OnPlaybackEnd += () =>
            {
                _pauseAction();
            };
            
        }
        
        #endregion

        #region Private methods
        /// <summary>
        /// Setup for buttons used to switch scenes
        /// </summary>
        /// <param name="button"></param>
        private void SetupSceneSwitchButton(Button button, Action<MessageBoxResponse> onDone, string message = "")
        {
            button.clickable.clicked += () => UIMessageManager.GetInstance().MessageBox(message,
                onDone, 
                new SimpleMessageBoxTypeSelector(SimpleMessageBoxTypeSelector.MessageBoxType.CONFIRM_CANCEL)
            );
        }


        
        /// <summary>
        /// Setup for the progress bar used to display the current step
        /// </summary>
        /// <param name="maxProgressLabel"></param>
        /// <param name="progressLabel"></param>
        /// <param name="progressBar"></param>
        /// <param name="maxSteps"></param>
        private void SetupStepsProgressBar(Label maxProgressLabel,Label progressLabel,ProgressBar progressBar, float maxSteps)
        {
            progressBar.lowValue = 0;
            progressBar.highValue = maxSteps;
            progressBar.value = 0;
            
            progressLabel.text = "0";
            maxProgressLabel.text = maxSteps.ToString();
        }
        
        /// <summary>
        /// Setup for the button used to add a new goal dynamically
        /// </summary>
        /// <param name="button"></param>
        /// <param name="coordinatesField"></param>
        /// <param name="man"></param>
        private void SetupDynamicGoalAddition(Button button,Vector2IntField coordinatesField, SimulationManager man)
        {

            button.clickable.clicked += () =>
            {
                try
                {
                    man.SimGoalManager.AddNewGoal(coordinatesField.value, man.Map);
                    UIMessageManager.GetInstance().MessageBox("Goal added successfully at " + coordinatesField.value,
                        response => { },
                        new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK));
                }
                catch (ArgumentException e)
                {
                    UIMessageManager.GetInstance().MessageBox(e.Message,
                        response => { },
                        new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK));
                }
            };
        }
        #endregion
        
    }
}