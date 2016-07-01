using UnityEngine;
using System.Collections;
/// <summary>
/// This script is for handling what happens when targets are hit
/// </summary>
public class TargetReaction : MonoBehaviour
{
    private GameManager _gainedPoints;
    private int _penetrationThreshHold;
    private void Awake()
    {
        _gainedPoints = GameObject.Find("GameManager").GetComponent<GameManager>();
       
    }
    private void Start()
    { 
        _penetrationThreshHold = 5;
    } 
    private void OnTriggerEnter(Collider hit)
    {
        //Checks if collider that entered the trigger area belongs to an object tagged with bullet
        if (hit.gameObject.tag == "bullet")
        {

            //Getting the distance between where bullet landed and center of target
            double distance = Mathf.Sqrt(Mathf.Pow(hit.transform.position.x - transform.position.x, 2) + Mathf.Pow(hit.transform.position.y - transform.position.y, 2) + Mathf.Pow(hit.transform.position.z - transform.position.z,2));
            //Debug.Log(distance);
          
            
            //Depending on distance from origin, it will give certain amount of points.
           
            //Numbers would need to be changed, but this works for most part, if 0 then absolute bullseye
            //might actually need to change my method a bit, since the numbers are almost everywhere none reach above 1.3f though
            if(distance == 0)
            {
                _gainedPoints.playerPoints = 20;
            }
            if (distance <= 0.35f && distance != 0)
            {
                
                _gainedPoints.playerPoints = 10;
            }
            else if (distance > 0.35 && distance <= 0.6f)
            {
                _gainedPoints.playerPoints = 5;
            
            }
            else
            {
                _gainedPoints.playerPoints = 2;
            }

            hit.GetComponent<Bullet>().penetration = _penetrationThreshHold;
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
