using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

    public string left, right, up, down, fire;
    public float mSpeed = 1000f;
    private Rigidbody2D tankObject;

    void Awake()
    {
        this.tankObject = this.GetComponent<Rigidbody2D>();
    }

    void Start ()
    {

    }
	
	void Update ()
    {
        if (Input.GetKey(up))
        {
            this.tankObject.AddForce(transform.up * this.mSpeed * Time.deltaTime);

        }

        if (Input.GetKey(down))
        {
            this.tankObject.AddForce(-transform.up * this.mSpeed * Time.deltaTime);
        }

        if (Input.GetKey(fire))
        {
            this.Fire();
        }
    }

    void Fire()
    {
        ShotPool.Instance.Fire(this);
    }
}
