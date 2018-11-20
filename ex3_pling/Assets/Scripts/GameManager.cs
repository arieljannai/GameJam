using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public GameObject player1, player2, ball, circleField;
    public GameObject lastLoserPlayer;
    private Dictionary<GameObject, int> points;

	void Start()
	{
        this.points = new Dictionary<GameObject, int>(2);
        this.points.Add(player1, 0);
        this.points.Add(player2, 0);
	}
	
	void Update()
	{
		
	}

    public int AddPoint(GameObject player)
    {
        this.lastLoserPlayer = this.GetOtherPlayer(player);
        this.points[player] += 1;
        return this.points[player];
    }

    private GameObject GetOtherPlayer(GameObject player)
    {
        return player == this.player1 ? this.player2 : this.player1;
    }
}
