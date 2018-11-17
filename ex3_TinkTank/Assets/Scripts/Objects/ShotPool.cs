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
            this.player1AvailableShots.Insert(i, goTemp);

            goTemp = Instantiate(this.shotPrefab, objectPoolPosition, Quaternion.identity);
            goTemp.GetComponent<Shot>().SetOwner(GameManager.Instance.player2);
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
            Debug.Log("aaaa" + this.player1AvailableShots);
            Debug.Log("bbbb" + this.shotsPoolSize);
            if (this.player1AvailableShots.Count > 0)
            {
                GameObject shot = this.player1AvailableShots.Find(x => x);
                this.player1AvailableShots.Remove(shot);
                this.player1ShotsInUse.Add(shot);
                shot.transform.position = GameManager.Instance.player1.transform.Find("Barrel1").position;
                shot.SetActive(true);
                shot.GetComponent<Rigidbody2D>().AddForce(1000*Vector3.forward * Time.deltaTime);
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
}
