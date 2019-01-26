using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionsMethods;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    #region AppConfiguration
    [Header("Players/Shapes Settings")]
    public Color player1Color;
    public Color player2Color;
    public string player1DrawLineKey;
    public string player2DrawLineKey;
    public float linesWidth;
    public float lineStrechingSpeed;
    public int nShapesCount;
    public bool playingWithTutorial;
    [Tooltip("Size should match the number of active shapes (N Shapes Count) + tutorial shape")]
    public List<int> minimunSuccessRate;
    [Header("Hierarchy")]
    public GameObject UI;
    public GameObject turtleDisc;
    public GameObject openScreen;
    public GameObject worldRedLand;
    public GameObject worldGreenLand;
    public GameObject constellationsRed;
    public GameObject constellationsGreen;
    public GameObject tutorialRedShape;
    public GameObject tutorialGreenShape;
    public List<GameObject> shiftTexts;
    public List<GameObject> holdTexts;
    public List<GameObject> shiftToHold;
    public List<GameObject> winnersNotice;
    public List<GameObject> PlayersMarkers;
    [Header("Waiting Times")]
    public float stayOnWinningStateTime;
    #endregion

    private List<GameObject> listWorldRedland;
    private List<GameObject> listWorldGreenland;
    private List<GameObject> listRedConstellations;
    private List<GameObject> listGreenConstellations;
    private List<GameObject> listTutorialShapes;
    private List<List<GameObject>> listWorldLands;
    private List<List<GameObject>> listConstellations;
    private List<GameObject> listCurrentShapes;
    //private GameObject currentRedShape;
    //private GameObject currentGreenShape;
    private List<GameShape> listCurrentScripts;
    //private GameShape currentRedScript;
    //private GameShape currentGreenScript;
    private int[] points;
    private int idxRed = 0;
    private int idxGreen = 1;
    private int currentShapeIndex = 0;
    private int lastWinner = -1;
    private bool isGameOver = false;
    private bool isReadyForNextRound = false;
    private bool isTutorialOver = false;
    private bool isWaitingForTimer = false;
    private float timer;
    private float tempTimer;

    private readonly int NUM_OF_PLAYERS = 2;

    void Awake()
    {
        Debug.Log("GameController.Awake()");
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.StartListening(EventManager.EVENT__TUTORIAL_END, OnTutorialEnd);
        EventManager.Instance.StartListening(EventManager.EVENT__CONSTELLATION_END, OnConstellationEnd);
        EventManager.Instance.StartListening(EventManager.EVENT__LAND_ON, OnLandOn);
        EventManager.Instance.StartListening(EventManager.EVENT__STARTED_DRAWING, OnShapeStartDrawing);
        EventManager.Instance.StartListening(EventManager.EVENT__FINISHED_DRAWING, OnShapeFinishDrawing);

        if (!this.playingWithTutorial)
        {
            this.isTutorialOver = true;
        }

        Debug.Log("GameController.Start()");
        this.listWorldRedland = this.worldRedLand.GetChildrenGameObjects(this.nShapesCount);
        this.listWorldGreenland = this.worldGreenLand.GetChildrenGameObjects(this.nShapesCount);
        this.listRedConstellations = this.constellationsRed.GetChildrenGameObjects(this.nShapesCount);
        this.listGreenConstellations = this.constellationsGreen.GetChildrenGameObjects(this.nShapesCount);

        this.listTutorialShapes = new List<GameObject>(NUM_OF_PLAYERS) { this.tutorialRedShape, this.tutorialGreenShape };
        this.listWorldLands = new List<List<GameObject>>(NUM_OF_PLAYERS) { this.listWorldRedland, this.listWorldGreenland };
        this.listConstellations = new List<List<GameObject>>(NUM_OF_PLAYERS) { this.listRedConstellations, this.listGreenConstellations };
        this.listCurrentShapes = new List<GameObject>(NUM_OF_PLAYERS) { null, null };
        this.listCurrentScripts = new List<GameShape>(NUM_OF_PLAYERS) { null, null };

        this.points = new int[NUM_OF_PLAYERS];

        this.UpdateCurrents();
    }

    // Update is called once per frame
    void Update()
    {
        this.timer += Time.deltaTime;

        if (!this.isGameOver)
        {
            if ((this.listCurrentScripts[this.idxRed] && this.listCurrentScripts[this.idxGreen] && 
                this.listCurrentScripts[this.idxRed].IsReadyForDrawing && this.listCurrentScripts[this.idxGreen].IsReadyForDrawing))
            {
                this.listCurrentScripts[this.idxRed].ShouldDraw = true;
                this.listCurrentScripts[this.idxGreen].ShouldDraw = true;

                this.listCurrentScripts[this.idxRed].IsReadyForDrawing = false;
                this.listCurrentScripts[this.idxGreen].IsReadyForDrawing = false;

                this.HideTutorialScreen();

                this.HandleRoundWinner();
            }

            //Debug.Log(this.timer);

            if (this.isWaitingForTimer && this.timer >= this.stayOnWinningStateTime)
            {
                this.isReadyForNextRound = true;
                this.isWaitingForTimer = false;
                this.winnersNotice[this.idxRed].Animator().SetState(0);
                this.winnersNotice[this.idxGreen].Animator().SetState(0);
                this.listCurrentShapes[this.idxRed].SetActive(false);
                this.listCurrentShapes[this.idxGreen].SetActive(false);
            }

            if (this.isReadyForNextRound)
            {
                this.UpdatesCurrentsAndIncreaseShapes();
                this.isReadyForNextRound = false;
            }

            if (!this.isTutorialOver)
            {
                if (Input.GetKey(this.player1DrawLineKey))
                {
                    this.shiftToHold[this.idxRed].Animator().SetInteger("State", 1);
                    //this.shiftToHold[this.idxRed].Animator().CrossFade("RedShift_2", 2);
                }

                if (Input.GetKey(this.player2DrawLineKey))
                {
                    this.shiftToHold[this.idxGreen].Animator().SetInteger("State", 1);
                }
            } 
        }
        else
        {
            this.HandleGameOver();
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            this.NewGame();
        }
    }

    private void HandleRoundWinner()
    {
        if (this.isTutorialOver)
        {
            int res = GameShape.Compare(this.listCurrentScripts[this.idxRed], this.listCurrentScripts[this.idxGreen]);
            int winner, loser;

            if (res == 0)
            {
                Debug.Log("Tie! Nobody gets a point.");
                this.lastWinner = -1;
            }
            else
            {
                if (res < 0)
                {
                    winner = idxGreen;
                    loser = idxRed;
                    //this.listCurrentShapes[this.idxGreen].Animator().SetState(3);
                    //this.listCurrentShapes[this.idxGreen].Animator().CrossFade("GConstellation_ShapeWin", 0);
                    ////this.listCurrentShapes[this.idxRed].Animator().SetState(2);
                    //this.listCurrentShapes[this.idxRed].Animator().CrossFade("RConstellation_FadeOut", 0);
                }
                else
                {
                    winner = idxRed;
                    loser = idxGreen;
                    ////this.listCurrentShapes[this.idxRed].Animator().SetState(3);
                    //this.listCurrentShapes[this.idxRed].Animator().CrossFade("RConstellation_ShapeWin", 0);
                    ////this.listCurrentShapes[this.idxGreen].Animator().SetState(2);
                    //this.listCurrentShapes[this.idxGreen].Animator().CrossFade("GConstellation_FadeOut", 0);
                }

                this.lastWinner = winner;
                this.points[winner]++;

                //this.listWorldLands[winner][this.currentShapeIndex].SetOpacity(255);
                Debug.Log(string.Format("Player {0} won this round. {0}: {1}; {2}: {3}",
                    (GameShape.Players)(winner + 1), this.points[winner], (GameShape.Players)(loser + 1), this.points[loser]));
            }
        }
    }

    private void UpdatesCurrentsAndIncreaseShapes()
    {
        if (this.currentShapeIndex == this.nShapesCount)
        {
            this.isGameOver = true;
        }
        else
        {
            this.UpdateCurrents();
            this.currentShapeIndex++;
        }
    }

    private void UpdateCurrents()
    {
        if (this.listCurrentShapes[this.idxRed] != null) this.listCurrentShapes[this.idxRed].SetActive(false);
        if (this.listCurrentScripts[this.idxRed] != null) this.listCurrentScripts[this.idxRed].IsActive = false;
        if (this.listCurrentShapes[this.idxGreen] != null) this.listCurrentShapes[this.idxGreen].SetActive(false);
        if (this.listCurrentScripts[this.idxGreen] != null) this.listCurrentScripts[this.idxGreen].IsActive = false; 

        if (this.isTutorialOver)
        {
            this.listCurrentShapes[this.idxRed] = this.listRedConstellations[this.currentShapeIndex];
            this.listCurrentShapes[this.idxRed].SetActive(true);
            this.listCurrentShapes[this.idxRed].Animator().CrossFade("RConstellation_FadeIn", 0);

            this.listCurrentShapes[this.idxGreen] = this.listGreenConstellations[this.currentShapeIndex];
            this.listCurrentShapes[this.idxGreen].SetActive(true);
            this.listCurrentShapes[this.idxGreen].Animator().CrossFade("GConstellation_FadeIn", 0);

        }
        else
        {
            this.listCurrentShapes[this.idxRed] = this.tutorialRedShape;
            this.listCurrentShapes[this.idxRed].SetActive(true);

            this.listCurrentShapes[this.idxGreen] = this.tutorialGreenShape;
            this.listCurrentShapes[this.idxGreen].SetActive(true);
        }

        this.listCurrentScripts[this.idxRed] = this.listCurrentShapes[this.idxRed].GetComponent(typeof(GameShape)) as GameShape;
        this.listCurrentScripts[this.idxRed].IsTutorial = !this.isTutorialOver;
        this.DisableShapesExcept(this.listRedConstellations, this.listCurrentShapes[this.idxRed], this.idxRed);
        this.listCurrentScripts[this.idxRed].IsActive = true;

        this.listCurrentScripts[this.idxGreen] = this.listCurrentShapes[this.idxGreen].GetComponent(typeof(GameShape)) as GameShape;
        this.listCurrentScripts[this.idxGreen].IsTutorial = !this.isTutorialOver;
        this.DisableShapesExcept(this.listGreenConstellations, this.listCurrentShapes[this.idxGreen], this.idxGreen);
        this.listCurrentScripts[this.idxGreen].IsActive = true;

        this.PlayersMarkers[this.idxRed].SetActive(true);
        this.PlayersMarkers[this.idxGreen].SetActive(true);
    }

    private void DisableShape(GameObject obj)
    {
        //obj.SetOpacity(0);
        obj.SetActive(false);
    }

    private void DisableShapesExcept(List<GameObject> list, GameObject obj, int player)
    {
        //this.SetOpacityExcept(list, 0, obj);

        //if (this.isTutorialOver)
        //{
        //    if (player == this.idxRed)
        //    {
        //        obj.Animator().CrossFade("RConstellation_Off", 0);
        //    }
        //    else if (player == this.idxGreen)
        //    {
        //        obj.Animator().CrossFade("GConstellation_Off", 0);
        //    }
        //}

        //list.ForEach(go => go.GetComponent<SpriteRenderer>().enabled = (go == obj));
        list.ForEach(go => (go.GetComponent(typeof(GameShape)) as GameShape).IsActive = (go == obj));

        //list.ForEach(go => go.SetActive(go == obj));

        //if (this.isTutorialOver)
        //{
        //    list.ForEach(go => go.GetComponent<Animator>().enabled = (go == obj));
        //}
    }

    //private void SetOpacityExcept(List<GameObject> list, float opacity, GameObject obj)
    //{
    //    foreach (GameObject go in list)
    //    {
    //        if (go != obj)
    //        {
    //            go.SetOpacity(opacity);
    //        }
    //    }
    //}

    //private void SetOpacityExcept(List<GameObject> list, float opacity, params GameObject[] objects)
    //{
    //    HashSet<GameObject> hs = new HashSet<GameObject>(objects);

    //    foreach (GameObject go in list)
    //    {
    //        if (!hs.Contains(go))
    //        {
    //            go.SetOpacity(opacity);
    //        }
    //    }
    //}
    
    private bool WaitTime(float time)
    {
        return (time >= this.timer);
    }

    private void NewGame()
    {
        SceneManager.LoadScene(0);
    }

    #region Handle Screen Changes

    private void OnTutorialEnd(object obj)
    {
        this.isTutorialOver = true;
        this.isReadyForNextRound = true;
        this.ShowMainScreen();
    }

    private void OnConstellationEnd(object obj)
    {
        string endState = obj as string;
    }

    private void OnLandOn(object obj)
    {
        //this.isReadyForNextRound = true;
        this.isWaitingForTimer = true;
        this.timer = 0;
    }

    private void OnShapeFinishDrawing(object obj)
    {
        int playerNumber = (int)obj;

        if (this.lastWinner == playerNumber)
        {
            // TODO: the minus one doesn't supposed to be here, should fix timing of other things
            this.listWorldLands[this.lastWinner][this.currentShapeIndex - 1].Animator().SetState(1);
            this.winnersNotice[this.lastWinner].Animator().SetState(1);

            string sConstellationLetter = playerNumber == this.idxRed ? "R" : "G";
            string sOtherConstellationLetter = playerNumber == this.idxRed ? "G" : "R";

            //this.listCurrentShapes[this.lastWinner].Animator().SetState(3);
            this.listCurrentShapes[this.lastWinner].Animator().CrossFade(sConstellationLetter + "Constellation_ShapeWin", 2);

            int otherPlayer = this.lastWinner == this.idxRed ? this.idxGreen : this.idxRed;

            //this.listCurrentShapes[otherPlayer].Animator().SetState(2);
            this.listCurrentShapes[otherPlayer].Animator().CrossFade(sOtherConstellationLetter + "Constellation_FadeOut", 0);



            //if (this.lastWinner == this.idxRed)
            //{
            //    this.listCurrentShapes[this.idxRed].Animator().SetState(3);
            //    this.listCurrentShapes[this.idxRed].Animator().CrossFade("RConstellation_ShapeWin", 2);
            //    //this.listCurrentShapes[this.idxGreen].Animator().SetState(2);
            //    //this.listCurrentShapes[this.idxGreen].Animator().CrossFade("GConstellation_FadeOut", 0);
            //}
            //else if (this.lastWinner == this.idxGreen)
            //{
            //    this.listCurrentShapes[this.idxGreen].Animator().SetState(3);
            //    this.listCurrentShapes[this.idxGreen].Animator().CrossFade("GConstellation_ShapeWin", 2);
            //    //this.listCurrentShapes[this.idxRed].Animator().SetState(2);
            //    //this.listCurrentShapes[this.idxRed].Animator().CrossFade("RConstellation_FadeOut", 0);
            //}
        }
    }

    private void OnShapeStartDrawing(object obj)
    {
        int playerNumber = (int)obj;
        //Debug.Log("hhh" + obj);
        //this.PlayersMarkers[playerNumber].SetOpacity(0);
        //this.PlayersMarkers[playerNumber].SetActive(false);

        //this.shiftToHold[playerNumber].Animator().SetState(0);
        this.shiftToHold[this.idxRed].Animator().SetState(2);
        this.shiftToHold[this.idxRed].Animator().CrossFade("RedShift_1", 2);
        this.shiftToHold[this.idxGreen].Animator().SetState(2);
        this.shiftToHold[this.idxGreen].Animator().CrossFade("GreenShift_1", 2);
    }

    private void HandleGameOver()
    {
        if (this.lastWinner == this.idxRed || this.lastWinner == this.idxGreen)
        {
            this.winnersNotice[this.lastWinner].Animator().SetState(1);
            this.lastWinner = -1;
        }
    }

    private void HideTutorialScreen()
    {
        this.shiftToHold[this.idxRed].Animator().SetState(2);
        this.shiftToHold[this.idxRed].Animator().CrossFade("RedShift_FadeOut", 2);
        this.shiftToHold[this.idxGreen].Animator().SetState(2);
        this.shiftToHold[this.idxGreen].Animator().CrossFade("GreenShift_FadeOut", 2);
        this.openScreen.Animator().SetState(1);
        
        //DisableShape(this.tutorialRedShape);
        //DisableShape(this.tutorialGreenShape);
    }

    private void ShowMainScreen()
    {
        this.turtleDisc.Animator().SetState(1);
    }
    
    #endregion
}
