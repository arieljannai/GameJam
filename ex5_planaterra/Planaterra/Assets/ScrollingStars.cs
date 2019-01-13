using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingStars : MonoBehaviour {

	public float starSPEED = 10;

//	Vector3 startPOS;


void Start(){
//	startPOS = transform.position;

}

void Update () {
		transform.Translate ((new Vector3 (0, -1, 0)) * starSPEED *  Time.deltaTime); 
//		if (transform.position.x < 230.6)
//		transform.position = startPOS;
}


}