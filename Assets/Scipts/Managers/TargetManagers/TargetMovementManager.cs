using UnityEngine;
using System.Collections;

namespace Hydrogen
{

    public delegate Vector3 Movement(Transform position, Vector3 movement);

    public class MovementManager
    {

        public Movement generateTargetMovement(float minDistance,float maxDistance,float speed,float eccentricity)
        {
            return (Transform a, Vector3 b) =>
            {
                //TODO: make actual movement algorithm
                return new Vector3();
            };
        }
        
        public Movement generateAnchorMovement(Transform initialPosition,float distance,float speed,float ecentricity)
        {
            return (Transform a, Vector3 b) =>
            {
                //TODO: make actual movement algorithm
                return new Vector3();
            };
        }

    }
}