using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTrigger : MonoBehaviour {
    private AudioSource asource;
    public string gameTag;
	// Use this for initialization
	void Start () {
        asource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == gameTag)
        {
            asource.Play(0);
        }

    }
}
