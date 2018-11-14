using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }
    private static int NUM_OF_WOOLS = 20; // Note that there's one more - the original one.
    private string ONE_WINNER = " is the winnner!\n\nPress 0 for\nanother round";
    private string GAME_END_TIE = "It's a tie!\nPress 0 for\nanother round";

    public GameObject mWools;
    public GameObject PinkSheep;
    public GameObject soundWoolCollected, soundTheme, soundWinning;
    public SheepScript sheep1, sheep2;
    public Text mPointsText;
    public Text mGameEnded;
    private int mWoolsLeft;

    void Awake()
    {
        this.mWoolsLeft = NUM_OF_WOOLS + 1;
    }

    void Start()
    {
        for (int i = 0; i < NUM_OF_WOOLS; i++)
        {
            GameObject woolGO = Instantiate(Resources.Load("Wool")) as GameObject;

            // I haven't connected it to the parent, since it caused problems when I wanted to delete the parent.
            // What could be a better design for this part?
            // woolGO.transform.SetParent(mWools.transform);

            float xPos = Random.Range(-11f, 11f);
            float yPos = Random.Range(-9f, 8f);

            woolGO.transform.position = new Vector3(xPos, yPos, 0);

            float randAngle = Random.Range(0, 200f);
            woolGO.transform.eulerAngles = new Vector3(0, 0, randAngle);
        }

    }

    void Update()
    {

    }

    private void UpdatePointsText()
    {
        this.mPointsText.text = "Sheep 1 Wools: " + sheep1.Points() + "\nSheep 2 Wools: " + sheep2.Points();
    }

    public void AddPoint(string name)
    {
        SheepScript sheep = null;
        if (name == "Sheep1") sheep = sheep1;
        if (name == "Sheep2") sheep = sheep2;

        sheep.AddPoint();

        UpdatePointsText();
    }

    public void RemoveWool()
    {
        this.mWoolsLeft--;
        if (this.mWoolsLeft == 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // Showing the pink sheep and the winning text
        this.PinkSheep.SetActive(true);
        this.mPointsText.gameObject.SetActive(false);
        this.mGameEnded.gameObject.SetActive(true);
        this.soundTheme.gameObject.SetActive(false);
        this.soundWinning.gameObject.SetActive(true);
        this.soundWinning.GetComponent<AudioSource>().Play();
        Destroy(this.sheep1.gameObject);
        Destroy(this.sheep2.gameObject);

        if (sheep1.Points() > sheep2.Points())
        {
            this.mGameEnded.text = "Sheep 1" + ONE_WINNER;
        }
        else if (sheep2.Points() > sheep1.Points())
        {
            this.mGameEnded.text = "Sheep 2" + ONE_WINNER;
        }
        else
        {
            this.mGameEnded.text = GAME_END_TIE;
        }
    }
}