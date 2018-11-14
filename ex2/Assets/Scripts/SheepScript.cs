using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheepScript : MonoBehaviour
{
    public string left, right, up, down;
    private Rigidbody2D sheepObject;
    public float mSpeed = 2000f;
    private int mPoints;

    void Awake()
    {
        this.mPoints = 0;
    }

    void Start()
    {
        this.sheepObject = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(left))
        {
            this.sheepObject.MoveRotation(this.sheepObject.rotation + 5);
        }

        if (Input.GetKey(right))
        {
            this.sheepObject.MoveRotation(this.sheepObject.rotation - 5);
        }

        if (Input.GetKey(up))
        {
            this.sheepObject.AddForce(transform.up * this.mSpeed * Time.deltaTime);

        }

        if (Input.GetKey(down))
        {
            this.sheepObject.AddForce(-transform.up * this.mSpeed * Time.deltaTime);
        }
    }

    public int Points()
    {
        return this.mPoints;
    }

    public void AddPoint()
    {
        this.mPoints++;
    }
}
