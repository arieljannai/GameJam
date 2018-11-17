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

    public void OnShotRecycle()
    {
        if (onShotRecycle != null)
        {
            //onShotRecycle.Invoke()
        }
    }
}
