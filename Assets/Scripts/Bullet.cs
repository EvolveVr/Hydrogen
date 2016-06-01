using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private int _damage; //the damage that will be added into the total gun damage
    public int damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    //methods for bullets
    // code here....

    public void hit()
    {

    }
}