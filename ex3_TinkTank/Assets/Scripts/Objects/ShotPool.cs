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
            //Physics2D.IgnoreCollision(goTemp.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            this.player1AvailableShots.Insert(i, goTemp);

            goTemp = Instantiate(this.shotPrefab, objectPoolPosition, Quaternion.identity);
            goTemp.GetComponent<Shot>().SetOwner(GameManager.Instance.player2);
            goTemp.gameObject.SetActive(false);
            //Physics2D.IgnoreCollision(goTemp.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            this.player2AvailableShots.Insert(i, goTemp);
        }
	}
	
	void Update ()
    {
		
	}

    public void Fire(Tank player)
    {
        List<GameObject> availableShots = null, shotsInUse = null;
        Transform barrel = player.objBarrel.gameObject.transform;
        Vector3 vDirection = Vector3.forward;
        bool correctPlayer = true;

        if (player.name == GameManager.Instance.player1.name)
        {
            availableShots = this.player1AvailableShots;
            shotsInUse = this.player1ShotsInUse;
            vDirection = -barrel.transform.right;
        }
        else if (player.name == GameManager.Instance.player2.name)
        {
            availableShots = this.player2AvailableShots;
            shotsInUse = this.player2ShotsInUse;
            vDirection = barrel.transform.right;
        }
        // Not supposed to arrive here
        else
        {
            correctPlayer = false;
        }

        if (correctPlayer)
        {
            Debug.Log(player.name + ": Trying to shoot");
            if (availableShots.Count > 0)
            {
                GameObject shot = availableShots.Find(x => x);
                availableShots.Remove(shot);
                shotsInUse.Add(shot);
                shot.transform.position = barrel.position;
                shot.transform.rotation = barrel.rotation;
                shot.SetActive(true);
                // TODO: fix the moving direction
                shot.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                shot.GetComponent<Rigidbody2D>().AddForce(GameManager.Instance.mBulletSpeed * vDirection * Time.deltaTime);
            }
            else
            {
                Debug.Log(player.name + ": Not enough shots");
            }
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
