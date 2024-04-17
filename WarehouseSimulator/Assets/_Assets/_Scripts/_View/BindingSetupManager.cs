using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    public class BindingSetupManager
    {
        public static void SetupBindings()
        {
            SetupMainMenuBinding();
            SetupSimBinding();
            SetupPlaybackBinding();
        }

        private static void SetupSimBinding()
        {
            var doc = SceneHandler.GetDocOfID(1);
            doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomRight")
                .Q("ButtonBox")
                .Q<Button>("Button_Abort")
                .clickable.clicked += () => UIMessageManager.GetInstance().MessageBox("Abort simulation?",
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

        private static void SetupPlaybackBinding()
        {
            var doc = SceneHandler.GetDocOfID(2);
        }

        private static void SetupMainMenuBinding()
        {
            var doc = SceneHandler.GetDocOfID(0);
        }
    }
}