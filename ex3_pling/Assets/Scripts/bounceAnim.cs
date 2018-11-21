﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounceAnim : MonoBehaviour {
    public Animator anim;
    public string gameTag;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == gameTag)
        {
            anim.SetTrigger("bounce");
        }

    }
}
