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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Shot" && !(collision.gameObject.tag == "Player" && collision.gameObject.name == this.owner.name))
        {
            Debug.Log(collision.gameObject.name + " : " + gameObject.name + " : " + Time.time);
            ShotPool.Instance.RecycleShot(this);
        }
    }
}
