using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoolScript : MonoBehaviour {

    public float mTurnSpeed = 10f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// this.gameObject.transform.Rotate(Vector3.up * Time.deltaTime * this.mTurnSpeed);
		// this.gameObject.transform.localscale = new Vector3(2,2,2);
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.name == "Sheep")
		{
			SheepScript.Instance.AddPoint();
			SheepScript.Instance.RemoveWool();
			Destroy(this.gameObject);
		}
    }
}
