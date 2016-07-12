using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Hydrogen
{

    public delegate Vector3 Movement(Transform position, Transform initialPosition, Vector3 direction);
    
    public class MovementManager
    {
        public Movement generateTargetMovement(float minDistance,float maxDistance,float speed,float eccentricity)
        {
            return (Transform a, Transform b, Vector3 d) =>
            {
                
                Vector3.RotateTowards(a.position, b.position, Time.deltaTime * speed, (a.position - b.position).magnitude);
                //TODO: make actual movement algorithm
                d *= -1;
                return d;
                
            };
        }
        
        public Movement generateAnchorMovement(Transform initialPosition,float distance,float speed,float ecentricity)
        {
            return (Transform a, Transform b, Vector3 d) =>
            {
                //TODO: make actual movement algorithm
                return new Vector3();
            };
        }

    }
}