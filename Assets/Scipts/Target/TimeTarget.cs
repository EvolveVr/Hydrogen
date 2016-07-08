using UnityEngine;
using System.Collections;

public class TimeTarget : Target
{
    //Okay there are going to be two type of timetargets, so my structure is going to change a bit,
    //Brian is doing the time and numbers and when they'll spawn so I'll worry about functionality of targets, not when they'll spawn.
    protected override void Awake()
    {
        base.Awake();
    }
   

    protected override void Start ()
    {
        base.Start();
        //This start is straight up not being called wtf.
        //These will be increased over time for all targets, but for time targets it will always be double the everything of
        //point targets, magnitude, speed target, and speed of completing the wave.
        //WHY IS THIS NOT WORKING WTF.
     

        _movementVariables *= 2;
        bool obstacle = gameObject.CompareTag("Obstacle");
        if (obstacle)
        {
           
            GameObject obstaclePrefab = Resources.Load("Prefabs/TargetPrefabs/orbTarget") as GameObject;

            for (int i = 0; i < 3; i++)
            {
        
                GameObject obstacleObject = Instantiate(obstaclePrefab);
                obstacleObject.tag = "Obstacle";
                obstacleObject.transform.parent = transform;
                obstacleObject.AddComponent<OrbitTarget>();
                obstacleObject.SetActive(true);
                
            }
        }
	}

    protected override void OnTriggerEnter(Collider hit)
    {
        base.OnTriggerEnter(hit);
        //For time targets that have anchor as target, they will get tagged with obstacle.
        if (hit.gameObject.name == "bullet" && gameObject.tag != "obstacle")
        {
            _gainedFromTarget.addTimeLeftInRound = 5;
        }
    }
}
