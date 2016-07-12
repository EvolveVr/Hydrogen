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
        if (Input.GetAxis("Mouse X") < 0)
        {
            gameObject.transform.Rotate(Vector3.down, 45.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            gameObject.transform.Rotate( Vector3.up, 45.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse Y") > 0)
        {
            gameObject.transform.Rotate(Vector3.left, 45.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse Y") < 0)
        {
            gameObject.transform.Rotate(Vector3.right, 45.0f * Time.deltaTime);
        }
        if (Input.GetButtonDown("Fire1"))
        {
         

            StartCoroutine(shootBullet());
           

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
    IEnumerator shootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = firePoint.position;
        yield return new WaitForSeconds(0.2f);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 20.0f, ForceMode.Impulse);

        yield return new WaitForSeconds(3.0f);
        Destroy(bullet);

    }
}