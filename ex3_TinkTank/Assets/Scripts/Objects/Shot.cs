using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public delegate void OnShotRecycleDelegate(GameObject player);
    public OnShotRecycleDelegate onShotRecycle;

    private Tank owner;
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void SetOwner(Tank player)
    {
        this.owner = player;
    }

    public void SetOwner(GameObject player)
    {
        this.owner = player.GetComponent<Tank>();
    }

    public Tank GetOwner() { return this.owner; }

    public void OnShotRecycle()
    {
        if (onShotRecycle != null)
        {
            //onShotRecycle.Invoke()
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        // Move the ball
        if (collider.name == "Ball")
        {
            // TODO: fix the moving direction
            GameManager.Instance.Ball.GetComponent<Rigidbody2D>().AddForce(GameManager.Instance.Ball.transform.right * GameManager.Instance.mBallMovingSpeed * Time.deltaTime);
        }
        
        // Ignore if another shot is the trigger, dispose otherwise
        if (collider.tag != "Shot" && (!collider.transform.IsChildOf(this.owner.transform)))
        {
            ShotPool.Instance.RecycleShot(this);
        }
    }
}
