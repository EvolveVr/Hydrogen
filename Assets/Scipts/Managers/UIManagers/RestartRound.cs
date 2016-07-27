using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class RestartRound : MonoBehaviour
{
	public void restartRound()
    {
        SceneManager.LoadScene("_enviornmentTest");
    }
}
