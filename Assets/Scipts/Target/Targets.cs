using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour
{


    //Here's how I set it up, I created a sphere, then 3 empty gameobjects parented to the sphere, that have their own colliders so if hit's center then it entered the
    //bullseye empty gameobject's trigger collider, all of the colliders are pretty much at the same spot, the size is only difference, I made the inner radius and
    //bullseye radius box colliders, and outer radius spherecollider because box was reaching places outside of target too. The type of colliders don't really matter
    //since we're changing the targets anyway. Idk if this is best way, or a good way, but it works.
  public enum differentPositionsHitOnTarget
    {
        OUTER_RADIUS,
        INNER_RADIUS,
        BULLSEYE

    }
    differentPositionsHitOnTarget _areaHit;
    int _currentPoints;
    int _currentWave;
    bool _roundStart;
 

    //Made this a generic since int would be target reaction to give points and gameobject would be deactivating the objects.
    public delegate void targetReaction<T>(T bullet);
    targetReaction<GameObject> _playerShoot;
    


    public void setEventOnHit(targetReaction<int> onHit, differentPositionsHitOnTarget areaHit)
    {
      

        switch (areaHit)
        {
            case differentPositionsHitOnTarget.BULLSEYE:
                onHit(5);
                break;
            case differentPositionsHitOnTarget.INNER_RADIUS:
                onHit(3);
                break;
            case differentPositionsHitOnTarget.OUTER_RADIUS:
                onHit(1);
                break;
        }
        
    }

  


    void OnTriggerEnter(Collider hit)
    {

        if (hit.gameObject.name == "bullet")
        {
            //Temporary since we changed targets, but concept should be same
            
            switch (gameObject.name)
            {
                case "BullsEye":

                    _playerShoot += (GameObject x) => { Debug.Log("bullseye"); _areaHit = differentPositionsHitOnTarget.BULLSEYE; };
                    break;
                case "InnerRadius":
                 
                    _playerShoot += (GameObject x) => { Debug.Log("Close"); _areaHit = differentPositionsHitOnTarget.INNER_RADIUS; };
                    break;
                case "OuterRadius":

                    _playerShoot += (GameObject x) => { Debug.Log("You blind?"); _areaHit = differentPositionsHitOnTarget.OUTER_RADIUS; };
                    break;
            }

            //This adds the method to call in sequence to getting where bullet hit to set the target GameObject active to false
            _playerShoot += (GameObject x) => { x.SetActive(false); };
            setEventOnHit((int x) => { _currentPoints += x; Debug.Log("earned"+ x + "points"); }, _areaHit);

        }
        
    }
    //I check after it exits trigger, to make sure it processed all the colliders
    void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.name == "bullet")
        {
            hit.gameObject.SetActive(false);
           _playerShoot(gameObject);  
        }

    }
 
}


    

