using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPool : Singleton<ShotPool> {

    public GameObject shotPrefab;
    public int shotsPoolSize = 10;

    protected ShotPool() { }

    private List<GameObject> player1AvailableShots;
    private List<GameObject> player2AvailableShots;
    private List<GameObject> player1ShotsInUse;
    private List<GameObject> player2ShotsInUse;
    //private int player1ShotsUsage = 0;
    //private int player2ShotsUsage = 0;
    private Vector2 objectPoolPosition = new Vector2(-20f, -20f);
    
    void Awake()
    {
        this.player1AvailableShots = new List<GameObject>(this.shotsPoolSize);
        this.player2AvailableShots = new List<GameObject>(this.shotsPoolSize);
        this.player1ShotsInUse = new List<GameObject>(this.shotsPoolSize);
        this.player2ShotsInUse = new List<GameObject>(this.shotsPoolSize);
    }

	void Start ()
    {
        GameObject goTemp = null;
 
        for (int i = 0; i < this.shotsPoolSize; i++)
        {
            goTemp = Instantiate(this.shotPrefab, objectPoolPosition, Quaternion.identity);
            goTemp.GetComponent<Shot>().SetOwner(GameManager.Instance.player1);
            goTemp.gameObject.SetActive(false);
            this.player1AvailableShots.Insert(i, goTemp);

            goTemp = Instantiate(this.shotPrefab, objectPoolPosition, Quaternion.identity);
            goTemp.GetComponent<Shot>().SetOwner(GameManager.Instance.player2);
            goTemp.gameObject.SetActive(false);
            this.player2AvailableShots.Insert(i, goTemp);
        }
	}
	
	void Update ()
    {
		
	}

    public void Fire(Tank player)
    {
        if (player.name == GameManager.Instance.player1.name)
        {
            Debug.Log("Player1: Trying to shoot");
            Debug.Log("aaaa" + this.player1AvailableShots.Count);
            Debug.Log("bbbb" + this.shotsPoolSize);
            if (this.player1AvailableShots.Count > 0)
            {
                GameObject shot = this.player1AvailableShots.Find(x => x);
                this.player1AvailableShots.Remove(shot);
                this.player1ShotsInUse.Add(shot);
                shot.transform.position = GameManager.Instance.player1.transform.Find("Barrel1").position;
                Transform barrel1 = GameManager.Instance.player1.transform.Find("Barrel1");
                //barrel1.transform.Rotate(Vector3.forward, 40);
                shot.transform.LookAt(barrel1);
                shot.transform.rotation = barrel1.rotation;
                shot.SetActive(true);
                //shot.GetComponent<Rigidbody2D>().transform.rotation = GameManager.Instance.player1.transform.Find("Barrel1").rotation;
                shot.GetComponent<Rigidbody2D>().AddForce(10000 * Vector3.left * Time.deltaTime);
            }
            else
            {
                Debug.Log("Player1: Not enough shots");
            }
            
        }
        else if (player.name == GameManager.Instance.player2.name)
        {
            
        }
    }

    public void RecycleShot(Shot shot)
    {
        shot.transform.position = this.objectPoolPosition;
        shot.gameObject.SetActive(false);

        if (shot.GetOwner().name == GameManager.Instance.player1.name)
        {
            Debug.Log(this.player1AvailableShots.Count);
            this.player1AvailableShots.Add(shot.gameObject);
            Debug.Log(this.player1AvailableShots.Count);
            Debug.Log(this.player1ShotsInUse.Count);
            this.player1ShotsInUse.Remove(shot.gameObject);
            Debug.Log(this.player1ShotsInUse.Count);
        }
        else if (shot.GetOwner().name == GameManager.Instance.player2.name)
        {
            
        }
    }
}
