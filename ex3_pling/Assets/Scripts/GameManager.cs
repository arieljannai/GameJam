using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    public GameObject player1, player2, ball, circleField;
    public GameObject lastLoserPlayer;
    public Text player1Score, player2Score;
    public float winningScore;
    private bool isGameOver = false;
    private GameObject winner;
    private Dictionary<GameObject, int> points;

	void Start()
	{
        this.points = new Dictionary<GameObject, int>(2);
        this.points.Add(player1, 0);
        this.points.Add(player2, 0);
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Welcome");
        }
	}

    public int AddPoint(GameObject player)
    {
        this.lastLoserPlayer = this.GetOtherPlayer(player);
        this.points[player] += 1;
        Debug.Log(player.name + " point: " + this.points[player]);
        this.UpdateScore();

        if (this.points[player1] == this.winningScore || this.points[player2] == this.winningScore)
        {
            this.isGameOver = true;
            this.winner = this.points[player1] == this.winningScore ? player1 : player2;
        }

        return this.points[player];
    }

    private GameObject GetOtherPlayer(GameObject player)
    {
        return player == this.player1 ? this.player2 : this.player1;
    }

    private void UpdateScore()
    {
        this.player1Score.text = this.points[this.player1].ToString();
        this.player2Score.text = this.points[this.player2].ToString();
    }

    public bool IsGameOver()
    {
        return this.isGameOver;
    }
}
