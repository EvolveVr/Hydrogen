using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager gameManager;
    public float playerPoints;
    void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
