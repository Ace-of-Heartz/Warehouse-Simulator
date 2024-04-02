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
        private TextMeshPro id;

        private UnityMap _mapie;

        // Start is called before the first frame update
        void Start()
        {
            transform.position = _mapie.GetWorldPosition(_roboModel.GridPosition);
            id.text = _roboModel.Id.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 oldPos = transform.position;
            Vector3 newPos = _mapie.GetWorldPosition(_roboModel.GridPosition);
            if (oldPos != newPos)
            {
                transform.position = Vector3.Lerp(oldPos, newPos, Time.deltaTime * 5);
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

            id.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void MyThingies(Robot dis, UnityMap dat)
        {
            _roboModel = dis;
            _mapie = dat;
        }
    }
}    