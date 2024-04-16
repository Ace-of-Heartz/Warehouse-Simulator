using UnityEngine;
using UnityEngine.UIElements;

namespace WarehouseSimulator.View
{
    public class BindingSetupManager
    {
        public void SetupBindings()
        {
            SetupMainMenuBinding();
            SetupSimBinding();
            SetupPlaybackBinding();
        }

        private void SetupSimBinding()
        {
            var doc = SceneHandler.GetDocOfID(1);
            doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomRight")
                .Q("ButtonBox")
                .Q<Button>("Button_Abort")
                .clickable.clicked += () =>
            {
                UIMessageManager.GetInstance().MessageBox(
                    "Abort simulation?",
                    response =>
                    {
                        switch (response)
                        {
                            case MessageBoxResponse.CONFIRMED:

                                //TODO: Deallocate stuff from simulation
                                SceneHandler.GetInstance().SetCurrentScene(0);
                                break;
                            default:
                                Debug.Log(response);
                                break;

                        }
                    },
                    SimpleMessageBoxTypeSelector.MessageBoxType.CONFIRM_CANCEL
                    );
            };

        }

        private void SetupPlaybackBinding()
        {
            var doc = SceneHandler.GetDocOfID(2);
        }

        private void SetupMainMenuBinding()
        {
            var doc = SceneHandler.GetDocOfID(0);
        }
    }
}