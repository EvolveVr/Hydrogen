using UnityEngine;
using System.Collections;

public class TargetManager : MonoBehaviour
{
    //This script still needs alot more work done.
    TargetPool _getTarget;
    float _spawnInterval;
    void Awake()
    {
        
        _getTarget = gameObject.GetComponent<TargetPool>();
    }
	void Start()
    {
        _spawnInterval = 0.4f;
        StartCoroutine(spawnTarget());
        
    }
  
    IEnumerator spawnTarget()
    {
        //Grabs object form pool
        GameObject bullet = _getTarget.getTarget();
        //Sets the position of it
        if (_getTarget.CurrentWave == 1)
        {
            //Was thinking could set these spawn points in an array so it's cleaner to change which to use depending on current wave
            bullet.transform.position = new Vector3(5.0f, 0, 0);
        }
        //Sets it to active
        bullet.SetActive(true);

        yield return new WaitForSeconds(_spawnInterval);
        bullet = _getTarget.getTarget();
        bullet.transform.position = new Vector3(10.0f, 0, 0);
        bullet.SetActive(true);
        /*yield return new WaitForSeconds(_spawnInterval);
        bullet = _getTarget.getTarget();
        //bullet.transform.position += new Vector3(10.0f, 0,-15.0f);
        bullet.SetActive(true);*/
       
    }
}
