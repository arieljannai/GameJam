using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float mSpeed = 100;
    public float waitTimeBeforeReappear;
    private Rigidbody2D ball;
    private Vector3 nextMovingDirection;
    private Vector3 farPosition = new Vector3(-20, -20, 0);

	void Start()
	{
        this.nextMovingDirection = GameManager.Instance.lastLoserPlayer.transform.right;
        this.ball = this.GetComponent<Rigidbody2D>();
        this.ball.AddForce(this.nextMovingDirection * this.mSpeed * Time.deltaTime);
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
        this.ball.position = Vector3.zero;
        this.nextMovingDirection = GameManager.Instance.lastLoserPlayer.transform.right;
        this.ball.velocity = Vector3.zero;
        this.ball.angularVelocity = 0;
        this.ball.AddForce(this.nextMovingDirection * this.mSpeed * Time.deltaTime);
    }
}
