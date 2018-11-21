using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : Singleton<GameOverManager> {

    public Text finalScore;

    //void Start()
    //{
    //    DontDestroyOnLoad(this);
    //}

    void Update()
	{
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Welcome");
        }
    }
}
