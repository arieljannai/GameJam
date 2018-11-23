using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeManager : Singleton<WelcomeManager> {

    private static bool isFirstRound = true;

    protected WelcomeManager() { }

    void Update()
	{
		if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
	}
}
