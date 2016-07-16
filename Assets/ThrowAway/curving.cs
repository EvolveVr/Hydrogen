using UnityEngine;
using System.Collections;

public class curving : MonoBehaviour {
    private Vector3 initPos;
    public Transform target;
    private float speed = 2.0f;
    public float maxRadians = 1.0f;
    public Vector3 dir;
    public Vector3 translation;
    Vector3 targetDir;
    Vector3 targetDestination;
    float radius;
    bool movingTowards;

    void Start()
    {
        target.position += new Vector3(0, Mathf.Sin(Time.time * 5.0f) * 2.0f, 0.0f);
        movingTowards = true;
        radius = target.GetComponent<SphereCollider>().radius;
        targetDir = target.position - transform.position;
        initPos = transform.position + new Vector3(0, Mathf.Sin(Time.time * 5.0f), 0.0f);


    }
    Vector3 rotateTowards(Vector3 current, Vector3 target)
    {
        Vector3 newRotation = Vector3.RotateTowards(current, target, Time.deltaTime * Mathf.PI, 0.0f);
        return newRotation;
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector3 newDir = new Vector3();

        if (movingTowards)
        {

            transform.position = Vector3.MoveTowards(transform.position, target.position, step); 

            targetDir = target.position - transform.position;
            newDir = rotateTowards(transform.position, target.position);
        }  
        else if (!movingTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position, initPos, step);

            targetDir = initPos - transform.position;
            newDir = rotateTowards(transform.position, initPos);
        }
        
        
        transform.rotation = Quaternion.LookRotation(newDir);
        Debug.DrawRay(transform.position, newDir, Color.red);
        
        
        
       
       
        
    }

    void OnTriggerEnter(Collider other)
    {
        movingTowards = false;
    }
    void OnTriggerExit(Collider hit)
    {
        movingTowards = true;   
        
    }
}
