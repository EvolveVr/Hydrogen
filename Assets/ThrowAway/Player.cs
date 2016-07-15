using UnityEngine;
using System.Collections;
using Hydrogen;
/// <summary>
/// THROW AWAY
/// This class was for me to practice shooting the targets
/// 
/// </summary>
public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    Target sfsd;
    Bullet shoot;
    // Use this for initialization

    void Awake()
    {
    }
    void Start()
    {
     
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") < 0 && Input.GetAxis("Mouse Y") == 0)
        {
            gameObject.transform.Rotate(Vector3.down, 180.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse X") > 0 && Input.GetAxis("Mouse Y") == 0)
        {
            gameObject.transform.Rotate( Vector3.up, 180.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse Y") > 0 && Input.GetAxis("Mouse X") == 0)
        {
            gameObject.transform.Rotate(Vector3.left, 180.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse Y") < 0 && Input.GetAxis("Mouse X") == 0)
        {
            gameObject.transform.Rotate(Vector3.right, 180.0f * Time.deltaTime);
        }
        if (Input.GetButtonDown("Fire1"))
        {
         

            shootBullet();
           

        }
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 2);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(-Vector3.forward * Time.deltaTime * 2);
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-Vector3.right * Time.deltaTime * 2);
        }


    }

    
    void FixedUpdate()
    {
        
    }
    void shootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab); 
        bullet.transform.position = firePoint.position;
       
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 20.0f, ForceMode.Impulse);

      
    }
}