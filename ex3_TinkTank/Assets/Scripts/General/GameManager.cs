using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public GameObject player1;
    public GameObject player2;

    protected GameManager() { }

	void Start ()
    {
        Debug.Log("start");
	}

    void Update ()
    {
		
	}
}
