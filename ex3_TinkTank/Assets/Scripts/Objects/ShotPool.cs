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
            if (this.player1AvailableShots.Count > 0)
            {
                GameObject shot = this.player1AvailableShots.Find(x => x);
                this.player1AvailableShots.Remove(shot);
                this.player1ShotsInUse.Add(shot);
                shot.transform.position = player.objBarrel.transform.position;
                Transform barrel1 = player.objBarrel.gameObject.transform;
                shot.transform.LookAt(barrel1);
                shot.transform.rotation = barrel1.rotation;
                shot.SetActive(true);
                // TODO: fix the moving direction
                shot.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                shot.GetComponent<Rigidbody2D>().AddForce(10000f * barrel1.transform.up * Time.deltaTime);
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
            this.player1AvailableShots.Add(shot.gameObject);
            this.player1ShotsInUse.Remove(shot.gameObject);
        }
        else if (shot.GetOwner().name == GameManager.Instance.player2.name)
        {
            this.player2AvailableShots.Add(shot.gameObject);
            this.player2ShotsInUse.Remove(shot.gameObject);
        }
    }
}
