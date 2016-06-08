using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager gameManager;

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
