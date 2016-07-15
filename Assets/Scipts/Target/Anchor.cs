using UnityEngine;
using System.Collections;

namespace Hydrogen {

    public class Anchor : MonoBehaviour
    {
        //List of targets to spawn
        private Target[] _targetList;
        //difficulty level of the anchor
        private int _difficulty;

        // Use this for initialization
        void Start()
        {
            //TODO: Spawn targets
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: move anchor in a simple
        }

        public void setTargetList(Target[] newList)
        {
            _targetList = newList;
        }

        public void setInitial(Vector3 initialPosition, Vector3 initialMovement,Movement movementPattern)
        {

        }



        public void setInitial(Anchor copy)
        {

        }
    }
}