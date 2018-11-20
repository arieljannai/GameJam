using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float mSpeed = 100;

	void Start()
	{
		
	}
	
	void Update()
	{
        if (Input.GetKey(KeyCode.Keypad4))
        {
            this.GetComponent<Rigidbody2D>().AddForce(-transform.right * this.mSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad6))
        {
            this.GetComponent<Rigidbody2D>().AddForce(transform.right * this.mSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad8))
        {
            this.GetComponent<Rigidbody2D>().AddForce(transform.up * this.mSpeed * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.Keypad2))
        {
            this.GetComponent<Rigidbody2D>().AddForce(-transform.up * this.mSpeed * Time.deltaTime);
        }
    }
}
