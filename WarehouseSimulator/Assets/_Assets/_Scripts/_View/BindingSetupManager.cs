using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.View
{
    public class BindingSetupManager : MonoBehaviour
    {
        public void SetupSimBinding(SimulationData data)
        {
            var doc = SceneHandler.GetDocOfID(1);
            var button = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomRight")
                .Q("ButtonBox")
                .Q<Button>("Button_Abort");
            SetupAbortFor(button);
            
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
            var currStepsProperty = new SerializedObject(data).FindProperty("m_currentStep");
            var maxStepsProperty = new SerializedObject(data).FindProperty("m_maxStepAmount");
            SetupStepsProgressBar(maxProgressLabel,progressLabel,progressBar,currStepsProperty,maxStepsProperty);
        
        }

        private void SetupAbortFor(Button button)
        {
            button.clickable.clicked += () => UIMessageManager.GetInstance().MessageBox("Abort simulation?",
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

        public void SetupPlaybackBinding()
        {
            var doc = SceneHandler.GetDocOfID(2);
        }

        public void SetupMainMenuBinding()
        {
            var doc = SceneHandler.GetDocOfID(0);
        }
    }
}