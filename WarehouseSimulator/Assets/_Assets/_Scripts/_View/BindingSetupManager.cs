using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using WarehouseSimulator.Model.PB;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.View
{
    public class BindingSetupManager : MonoBehaviour
    {
        /// <summary>
        /// Setup for all bindings for the Simulation's UI
        /// </summary>
        /// <param name="man"></param>
        public void SetupSimBinding(SimulationManager man)
        {
            var doc = SceneHandler.GetDocOfID(1);
            var button = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomRight")
                .Q("ButtonBox")
                .Q<Button>("Button_Abort");
            SetupButtonWithMessageBox(button,"Abort simulation?");
            
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
            var currStepsProperty = new SerializedObject(man.SimulationData).FindProperty("m_currentStep");
            var maxStepsProperty = new SerializedObject(man.SimulationData).FindProperty("m_maxStepAmount");
            SetupStepsProgressBar(maxProgressLabel,progressLabel,progressBar,currStepsProperty,maxStepsProperty);
        
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
        /// 
        /// </summary>
        /// <param name="button"></param>
        private void SetupButtonWithMessageBox(Button button, string message = "")
        {
            button.clickable.clicked += () => UIMessageManager.GetInstance().MessageBox(message,
                response =>
                {
                    switch (response)
                    {
                        case MessageBoxResponse.CONFIRMED:
                            //TODO: Deallocate resources from Simulation
                            SceneHandler.GetInstance().SetCurrentScene(0);
                            SceneManager.LoadSceneAsync(SceneHandler.GetInstance().CurrentScene);

                            break;
                        default:
                            break;
                    }
                }, 
                new SimpleMessageBoxTypeSelector(SimpleMessageBoxTypeSelector.MessageBoxType.CONFIRM_CANCEL)
            );
        }

        private void SetupInfoPanel(VisualElement infoPanel)
        {
            throw new NotImplementedException();
        }
        
        private void SetupStepsProgressBar(Label maxProgressLabel,Label progressLabel,ProgressBar progressBar, SerializedProperty currStepsProperty, SerializedProperty maxStepsProperty)
        {
            progressBar.highValue = maxStepsProperty.intValue; 
            progressBar.BindProperty(currStepsProperty);
            progressLabel.BindProperty(currStepsProperty);
            maxProgressLabel.BindProperty(maxStepsProperty);
        }

        private void SetupDynamicGoalAddition(Button button,Vector2IntField coordinatesField, SimulationManager man)
        {

            button.clickable.clicked += () =>
            {
                try
                {
                    man.SimGoalManager.AddNewGoal(coordinatesField.value, man.Map);
                }
                catch (ArgumentException e)
                {
                    UIMessageManager.GetInstance().MessageBox(e.Message,
                        response => { },
                        new OneWayMessageBoxTypeSelector(OneWayMessageBoxTypeSelector.MessageBoxType.OK));
                }
                
                 
            };

        }

        public void SetupPlaybackBinding(PlaybackManager man)
        {
            
            var doc = SceneHandler.GetDocOfID(2);
            
            var data = new SerializedObject(man.PlaybackData);
            
            
            
            var currStepsProperty = data.FindProperty("_currentStep");
            var maxStepsProperty = data.FindProperty("_maxStepAmount");
            var playbackSpeedProperty = data.FindProperty("_playbackSpeed");

            //ProgressBar Setup
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


            SetupStepsProgressBar(maxProgressLabel,progressLabel,progressBar,currStepsProperty,maxStepsProperty);
 
            //FrameInputField Setup
            var frameInputField = doc.rootVisualElement
                .Q("PlaybackCanvas")
                .Q("LeftSideBar")
                .Q("SettingsPanel")
                .Q<UnsignedIntegerField>("FrameInputField");
            frameInputField.BindProperty(currStepsProperty);//TODO: this only modifies the property, not the pbManager
            
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

            pauseButton.clickable.clicked += () =>
            {
                man.PlaybackData.ChangePauseState();
                resumeButton.style.display = DisplayStyle.Flex;
                pauseButton.style.display = DisplayStyle.None;
            };
            resumeButton.clickable.clicked += () =>
            {
                man.PlaybackData.ChangePauseState();
                resumeButton.style.display = DisplayStyle.None;
                pauseButton.style.display = DisplayStyle.Flex;
            };
            
            
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
            
            SetupButtonWithMessageBox(exitButton,"Abort playback?");
        }

        public void SetupMainMenuBinding()
        {
            var doc = SceneHandler.GetDocOfID(0);
        }
    }
}