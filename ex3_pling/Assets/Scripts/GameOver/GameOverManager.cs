using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : Singleton<GameOverManager> {

    public Text finalScore;

	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Welcome");
        }
    }
}
