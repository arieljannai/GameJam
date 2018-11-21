using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeManager : Singleton<WelcomeManager> {

    //void Start()
    //{
    //    DontDestroyOnLoad(this);
    //}

	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
	}
}
