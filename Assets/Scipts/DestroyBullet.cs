using UnityEngine;
using System.Collections;

public class DestroyBullet : MonoBehaviour
{
    private float numberOfSeconds = 4.0f;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(destroyBullet(numberOfSeconds));
	}

    void OnTriggerEnter()
    {
        Destroy(gameObject);
    }

    IEnumerator destroyBullet(float _numSeconds)
    {
        yield return new WaitForSeconds(_numSeconds);
        Destroy(gameObject);
    }
}
