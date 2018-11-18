using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

    public string left, right, up, down, fire;
    public float mSpeed = 1000f;
    public GameObject objBase;
    public GameObject objBody;
    public GameObject objBarrel;
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

        if (Input.GetKey(left))
        {
            this.objBarrel.transform.RotateAround(this.objBarrel.transform.position, Vector3.forward, 20 * Time.deltaTime);
        }

        if (Input.GetKey(right))
        {
            this.objBarrel.transform.RotateAround(this.objBarrel.transform.position, Vector3.back, 20 * Time.deltaTime);
        }

        if (Input.GetKeyDown(fire))
        {
            this.Fire();
        }
    }

    void Fire()
    {
        ShotPool.Instance.Fire(this);
    }
}
