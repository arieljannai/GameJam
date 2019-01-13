using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float x = transform.position.x;
		float y = transform.position.y;



//		if (x < -19f)
////			x = 36f;
//			x = 46f;


//		if (x > 46f)
////			x = -14.4f;
			//x = -19f;


		if (y < -15.0f)
			y = 9.7f;

        if (y > 18.7f)
            y = -10f;
        //		if (x < -19f || x > 46f)
        //			x = -x;
        //
        //		if (y < -5.25f || y > 5.25f)
        //			y = -y;

        transform.position = new Vector2 (x, y);
		
	}
}
