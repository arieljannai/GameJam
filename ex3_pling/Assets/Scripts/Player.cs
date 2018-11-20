using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public string up, down;
    public float movingSpeed, collisionForce;
    private Vector3 circleFieldPosition;
    private Vector3 forward, back;
    private Rigidbody2D player;
    private int forwardDirection;

    void Awake()
    {
        this.player = this.GetComponent<Rigidbody2D>();
        this.player.centerOfMass = Vector2.zero;

        GameObject[] borders = GameObject.FindGameObjectsWithTag("Border");

        for (int iB = 0; iB < borders.Length; iB++)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), borders[iB].GetComponent<Collider2D>());
        }

        if (this.name == GameManager.Instance.player1.name)
        {
            this.forwardDirection = 1;
            this.forward = Vector3.forward;
            this.back = Vector3.back;
        }
        else if (this.name == GameManager.Instance.player2.name)
        {
            this.forwardDirection = -1;
            this.forward = Vector3.back;
            this.back = Vector3.forward;
        }
    }

    void Start()
    {
        this.circleFieldPosition = GameManager.Instance.circleField.transform.position;
    }
	
	void FixedUpdate()
    {
        if (Input.GetKey(up))
        {
            this.player.AddTorque(this.forwardDirection * this.movingSpeed * Time.deltaTime);
        }

        if (Input.GetKey(down))
        {
            this.player.AddTorque(-this.forwardDirection * this.movingSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKey(up))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddTorque(this.forwardDirection * collisionForce * this.movingSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(down))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddTorque(-this.forwardDirection * collisionForce * this.movingSpeed * Time.deltaTime);
            }
        }
    }
}
