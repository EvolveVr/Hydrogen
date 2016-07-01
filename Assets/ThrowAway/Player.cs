using UnityEngine;
using System.Collections;
/// <summary>
/// THROW AWAY
/// This class was for me to practice shooting the targets
/// 
/// </summary>
public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    TargetPool getTarget;
    Bullet shoot;
    // Use this for initialization
    void Start()
    {
        getTarget = GameObject.Find("TargetManager").GetComponent<TargetPool>();
    
        StartCoroutine(spreadBullet());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") < 0)
        {
            gameObject.transform.Rotate(Time.deltaTime * Vector3.down, 90.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            gameObject.transform.Rotate(Time.deltaTime * Vector3.up, 90.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse Y") > 0)
        {
            gameObject.transform.Rotate(Time.deltaTime * Vector3.left, 45.0f * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse Y") < 0)
        {
            gameObject.transform.Rotate(Time.deltaTime * Vector3.right, 45.0f * Time.deltaTime);
        }
        if (Input.GetButtonDown("Fire1"))
        {
         

            StartCoroutine(shootBullet());
            StartCoroutine(spreadBullet());

        }

    }

    IEnumerator spreadBullet()
    {
        yield return new WaitForSeconds(0.4f);
        //   shoot.getGun.transform.Translate(new Vector3(0, 0.025f,0));



    }
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") * 50.0f * Time.deltaTime;
        GameObject.Find("Retical").GetComponent<Rigidbody>().AddTorque(transform.right * x);
    }
    IEnumerator shootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = GameObject.Find("Retical").transform.position + Vector3.forward;
        
        yield return new WaitForSeconds(0.2f);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 20.0f, ForceMode.Impulse);

        yield return new WaitForSeconds(3.0f);
        Destroy(bullet);

    }
}