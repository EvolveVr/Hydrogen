using UnityEngine;
using System.Collections;
/// <summary>
/// Interface for all items that deal damage to targets
/// </summary>
public interface Projectile{

    float penetration { set; }
    //value - amount to reduce penetration by

    Vector3 position { get; }
    //returns the position of acting point on the projectile
    
}
