using UnityEngine;
using UnityEngine.UIElements;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.View.Sim
{
    public class UnitySimulationInfoManager : MonoBehaviour
    {
        private SimulationData _simulationData;
     
        private Label _stepProgressLabel;
        private ProgressBar _stepProgressBar;   
        
        private void Start()
        {
            var doc = SceneHandler.GetInstance().CurrentDoc;
            
            //ProgressBar Setup
            _stepProgressBar = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<ProgressBar>("StepsProgressBar");
            _stepProgressLabel = doc.rootVisualElement
                .Q("SimulationCanvas")
                .Q("BottomBar")
                .Q("BottomCenter")
                .Q<Label>("StepProgressLabel");


            _simulationData = this.GetComponentInParent<UnitySimulationManager>().SimulationData;
        }
        
        private void Update()
        {
            _stepProgressBar.value = _simulationData._currentStep;
            _stepProgressLabel.text = $"{_simulationData._currentStep}";
        }
    }
}