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
    //private GameObject winner;
    private Dictionary<GameObject, int> points;
    private Vector3 origPosPlayer1, origPosPlayer2, origPosBall;
    public static int[] finalPoints = new int[2];
    private AudioSource aSource;

    protected GameManager() { }

	void Start()
	{
        this.points = new Dictionary<GameObject, int>(2);
        this.points.Add(player1, 0);
        this.points.Add(player2, 0);
        aSource = gameObject.GetComponent<AudioSource>();
}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Welcome");
        }

        if (this.isGameOver)
        {
            SceneManager.LoadScene("GameOver");
        }

        // 0 - Reset scene cheat
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E))
        {
            this.points[player1] = 10;
            this.points[player2] = 10;
            this.UpdateScore();
        }
	}

    public int AddPoint(GameObject player)
    {
        this.lastLoserPlayer = this.GetOtherPlayer(player);
        this.points[player] += 1;
        Debug.Log(player.name + " points: " + this.points[player]);
        aSource.Play(0);
        this.UpdateScore();

        if (this.points[player1] == this.winningScore || this.points[player2] == this.winningScore)
        {
            this.isGameOver = true;
            //this.winner = this.points[player1] == this.winningScore ? player1 : player2;
            GameManager.finalPoints[0] = this.points[player1];
            GameManager.finalPoints[1] = this.points[player2];
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

    public void ResetLocations()
    {
        this.ball.GetComponent<Rigidbody2D>().position = Vector3.zero;
        this.ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        this.ball.GetComponent<Rigidbody2D>().angularVelocity = 0;

        this.player1.GetComponent<Rigidbody2D>().position = Vector3.zero;
        this.player1.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        this.player1.GetComponent<Rigidbody2D>().angularVelocity = 0;
        this.player1.GetComponent<Rigidbody2D>().rotation = 0;

        this.player2.GetComponent<Rigidbody2D>().position = Vector3.zero;
        this.player2.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        this.player2.GetComponent<Rigidbody2D>().angularVelocity = 0;
        this.player2.GetComponent<Rigidbody2D>().rotation = 0;
    }
}
