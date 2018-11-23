using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : Singleton<GameOverManager> {

    public Text finalScore1;
    public Text finalScore2;

    protected GameOverManager() { }

    void Start()
    {
        this.finalScore1.text = GameManager.finalPoints[0].ToString();
        this.finalScore2.text = GameManager.finalPoints[1].ToString();
    }

    void Update()
	{
        if (Input.GetKey(KeyCode.Space))
        {
            DestroyImmediate(GameManager.Instance);
            SceneManager.LoadScene("Main");
        }
    }
}
