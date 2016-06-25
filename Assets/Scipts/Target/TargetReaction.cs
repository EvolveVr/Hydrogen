using UnityEngine;
using System.Collections;

public class TargetReaction : MonoBehaviour {


    public int _health;
    public int _maxhealth;
    void Start()
    {
        _maxhealth = 1;
        _health = _maxhealth;
    }
    public int currentHealth
    {
        get { return _health; }
        set { _health = value; }
    }
    //Won't be in Target Reaction in future just here for testing
    
    void Update()
    {
        if (currentHealth <= 0)
        {
           //If health of target == 0 then despawn it
            gameObject.SetActive(false);
        }

    }
 
    void OnTriggerEnter(Collider hit)
    {
   
        if (hit.gameObject.tag == "bullet")
        {

            //Getting the distance between where bullet landed and center of target
            double distance = Mathf.Sqrt(Mathf.Pow(hit.transform.position.x - transform.position.x, 2) + Mathf.Pow(hit.transform.position.y - transform.position.y, 2));
          
            //Depending on distance from origin, it will take away certain amout of health and give certain amount of points.
            if (distance <= 0.7f)
            {
                currentHealth = 0;
            }
            else if (distance > 0.7f && distance <= 1.2f)
            {
                currentHealth -= 5;
            }
            else
            {
                currentHealth -= 3;
            }

            //Sets the bullet to inactive
            hit.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
