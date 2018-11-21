﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float mSpeed = 100;
    public float waitTimeBeforeReappear;
    public float waitTimeBeforeStart;
    private Rigidbody2D ball;
    private int nextMovingDirection = 1;
    private Vector3 farPosition = new Vector3(-20, -20, 0);

	void Start()
	{
        StartCoroutine(this.WaitSeconds(this.waitTimeBeforeStart));
        this.ball = this.GetComponent<Rigidbody2D>();
        this.ball.AddForce(this.nextMovingDirection * Vector3.right * this.mSpeed * Time.deltaTime);
    }
	
	void Update()
	{
        if (Input.GetKey(KeyCode.Keypad4))
        {
            this.ball.AddForce(-transform.right * this.mSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad6))
        {
            this.ball.AddForce(transform.right * this.mSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad8))
        {
            this.ball.AddForce(transform.up * this.mSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad2))
        {
            this.ball.AddForce(-transform.up * this.mSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "VerticalBorder")
        {
            // Checks in which side the ball passed
            GameObject player = (collision.gameObject.transform.position.x > 0) ? GameManager.Instance.player2 : GameManager.Instance.player1;
            GameManager.Instance.AddPoint(player);

            this.ball.position = this.farPosition;

            StartCoroutine(this.RecycleBall());
        }
    }

    IEnumerator RecycleBall()
    {
        yield return new WaitForSeconds(this.waitTimeBeforeReappear);
        //this.ball.position = Vector3.zero;
        //this.nextMovingDirection = GameManager.Instance.lastLoserPlayer == GameManager.Instance.player1 ? 1 : -1;
        //this.ball.velocity = Vector3.zero;
        //this.ball.angularVelocity = 0;
        GameManager.Instance.ResetLocations();
        this.ball.AddForce(this.nextMovingDirection * Vector3.right * this.mSpeed * Time.deltaTime);
    }

    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
