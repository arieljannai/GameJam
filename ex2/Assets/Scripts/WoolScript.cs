using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WoolScript : MonoBehaviour {

    public float mTurnSpeed = 20f;
    private float mResizeDirection;
    private Vector3 mRotateDirection;
    private float mMinSize = 0.3f;
    private float mMaxSize = 1.02f;
    private float mResizeRatio = 0.01f;

    void Start ()
    {
        //this.gameObject.transform.localScale.Set(Random.Range(mMinSize, mMaxSize), Random.Range(mMinSize, mMaxSize), 1);
        float randStartSize = Random.Range(mMinSize, mMaxSize);
        this.gameObject.transform.localScale = new Vector3(randStartSize, randStartSize, 1);
        this.mResizeDirection = Random.value < 0.5f ? -1 : 1;
        this.mRotateDirection = Random.value < 0.5f ? Vector3.forward : Vector3.back;
    }
	
	void Update () {
        // this.gameObject.transform.Rotate(this.mRotateDirection * Time.deltaTime * this.mTurnSpeed);
        this.gameObject.transform.localScale = this.getNewScale(this.gameObject.transform.localScale);
    }

    private Vector3 getNewScale(Vector3 current)
    {
        if (current.x >= mMaxSize || current.x <= mMinSize)
        {
            this.mResizeDirection *= -1;
        }

        return new Vector3(
            current.x + (this.mResizeDirection * this.mResizeRatio),
            current.y + (this.mResizeDirection * this.mResizeRatio),
            current.z);
    }

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Sheep"))
        {
            GameManager.Instance.soundWoolCollected.GetComponent<AudioSource>().Play();
            GameManager.Instance.AddPoint(other.name);
            GameManager.Instance.RemoveWool();
            //SheepScript.Instance.AddPoint();
            //SheepScript.Instance.RemoveWool();
            Destroy(this.gameObject);
        }
    }
}
