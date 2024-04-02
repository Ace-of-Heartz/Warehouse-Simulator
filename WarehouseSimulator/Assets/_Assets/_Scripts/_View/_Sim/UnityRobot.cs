using UnityEngine;
using WarehouseSimulator.Model.Sim;
using TMPro;
using WarehouseSimulator.Model.Enums;

namespace WarehouseSimulator.View.Sim
{
    public class UnityRobot : MonoBehaviour
    {
        private Robot _roboModel;
        [SerializeField]
        private TextMeshPro _id;

        // Start is called before the first frame update
        void Start()
        {
            transform.position = new(_roboModel.GridPosition.x, _roboModel.GridPosition.y);
            _id.text = _roboModel.Id.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 oldPos = transform.position;
            Vector3 newPos = new(_roboModel.GridPosition.x, _roboModel.GridPosition.y);
            if (oldPos != newPos)
            {
                transform.position = Vector3.Lerp(oldPos, newPos, Time.deltaTime);
            }

            Direction newRot = _roboModel.Heading;
            switch (newRot)
            {
                case Direction.North:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.East:
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case Direction.South:
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case Direction.West:
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }

            _id.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}    