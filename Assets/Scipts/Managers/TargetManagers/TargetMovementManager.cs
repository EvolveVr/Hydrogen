using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Hydrogen
{

    public delegate Vector3 Movement(Transform position, Transform targetPosition, Vector3 direction);
    public delegate float Pattern(float amplitude, float frequency, float narrowness);
    //I'll put other functions then return them  instead of a lamda or another list/dictionary of lambdas
    public class MovementManager
    {
        public static Pattern[] _patternList = new Pattern[]
        {
            //Sin wave
            //This is used for blood dripping pattern, with waves left to right and the frequency being slower than overall movement. and that movement on y axis
            //This is used for mountains, sharper increases more triangular at the top but still circular on slope.
            //
        
            //Blood Dripping pattern
            
        

        };
        public Movement generateTargetMovement(float minDistance,float maxDistance,float speed,float eccentricity = -1.0f)
        {
            Vector3 newDirection = new Vector3();
     

            float speedDeltaRoation = Time.deltaTime * Mathf.PI;
            //Will depend but a is position and b is target posistion in this case
            return (Transform a, Transform b, Vector3 d) =>
            {
                //Opposite of direction will be curve of object
                if (eccentricity == 0)
                {

                }
                Vector3 targetDirection = b.position - a.position;
                //So when this is the case that it's hitting end of trigger, then do this.
                if (eccentricity > 0.0f && eccentricity < 1.0f)
                {

                    newDirection = Vector3.RotateTowards(a.forward, targetDirection, speedDeltaRoation, 0.0f);
                }
                
                //TODO: make actual movement algorithm
                return newDirection;
                
            };
        }
       



        public Movement generateAnchorMovement(Transform initialPosition,float distance,float speed,float ecentricity)
        {
            //Anchor movement will be linear
            return (Transform a, Transform b, Vector3 d) =>
            {
                //TODO: make actual movement algorithm
                return new Vector3();
            };
        }



    }
}