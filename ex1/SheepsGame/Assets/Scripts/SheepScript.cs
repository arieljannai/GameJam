using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheepScript : MonoBehaviour
{
	private static int NUM_OF_WOOLS = 49; // Note that there's one more - the original one.
	private Rigidbody2D sheepObject;
	private int mPoints;
	private int mWoolsLeft;
    public GameObject PinkSheep;

	public float mSpeed = 200f;
    public Text mPointsText;
    public Text mGameEnded;
    public GameObject mWools;

	public static SheepScript Instance;

	void Awake()
    {
        this.mPoints = 0;
		this.mWoolsLeft = NUM_OF_WOOLS + 1;
    }

	public void AddPoint()
	{
		this.mPoints++;
		this.UpdatePointsText();
	}

	public void RemoveWool()
	{
		this.mWoolsLeft--;
		if (this.mWoolsLeft == 0)
		{
			EndGame();
		}
	}

	// Use this for initialization
	void Start ()
	{
		this.sheepObject = GetComponent<Rigidbody2D>();

		for (int i = 0; i < NUM_OF_WOOLS; i++)
        {
            // Create a diamond
            GameObject woolGO = Instantiate(Resources.Load("Wool")) as GameObject;
            woolGO.transform.SetParent(mWools.transform);

            float xPos = Random.Range(-11f, 11f);
            float yPos = Random.Range(-9f, 8f);

            woolGO.transform.position = new Vector3(xPos, yPos, 0);

            float randAngle = Random.Range(0, 200f);
            woolGO.transform.eulerAngles = new Vector3(0, 0, randAngle);
        }

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.Log("destroying this: " + this.name);
			Destroy(this);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.sheepObject.MoveRotation(this.sheepObject.rotation + 5);
            // this.sheepObject.AddForce(Vector2.left * this.mSpeed * Time.deltaTime);
        }
		
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.sheepObject.MoveRotation(this.sheepObject.rotation - 5);
            // this.sheepObject.AddForce(Vector2.right * this.mSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
		    this.sheepObject.AddForce(transform.up * this.mSpeed * Time.deltaTime);
            
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
		    this.sheepObject.AddForce(-transform.up * this.mSpeed * Time.deltaTime);
        }
	}

	private void UpdatePointsText()
    {
        mPointsText.text = "Wools: " + mPoints;
    }

	private void EndGame()
	{
        this.PinkSheep.SetActive(true);
        this.mGameEnded.gameObject.SetActive(true);
        Debug.Log("Bye!");
	}
}
