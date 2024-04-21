using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    public class BindingSetupManager : MonoBehaviour
    {
        public void SetupSimBinding()
        {
            var doc = SceneHandler.GetDocOfID(1);
            var button = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomRight")
                .Q("ButtonBox")
                .Q<Button>("Button_Abort");
            SetupAbortFor(button);
            
            

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
            infoPanel
        }
        
        private void SetupStepsProgressBar(VisualElement progressBar)
        {
            progressBar
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