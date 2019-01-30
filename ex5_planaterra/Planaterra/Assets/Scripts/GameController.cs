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
    private List<GameShape> listCurrentScripts;
    private List<bool> listPlayerReadyToContinue;
    private List<bool> listShouldRetryShape;
    private List<bool> listShapeFailedTooMuch;
    private List<bool> listShapeFailedTooLittle;
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

    #region Unity Events

    void Awake()
    {
        Debug.Log("GameController.Awake()");
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.StartListening(EventManager.EVENT__CONSTELLATION_END, OnConstellationEnd);
        EventManager.Instance.StartListening(EventManager.EVENT__LAND_ON, OnLandOn);

        EventManager.Instance.StartListening(EventManager.EVENT__FINISHED_STRECHING, OnShapeFinishStreching);
        EventManager.Instance.StartListening(EventManager.EVENT__STARTED_DRAWING, OnShapeStartDrawing);
        EventManager.Instance.StartListening(EventManager.EVENT__FINISHED_DRAWING, OnShapeFinishDrawing);
        EventManager.Instance.StartListening(EventManager.EVENT__FINISHED_SHOWING_LINE, OnShapeFinishShowingLine);

        EventManager.Instance.StartListening(EventManager.EVENT__FAILED_SHAPE_TOO_MUCH, OnShapeFailedTooMuch);
        EventManager.Instance.StartListening(EventManager.EVENT__FAILED_SHAPE_TOO_LITTLE, OnShapeFailedTooLIttle);

        EventManager.Instance.StartListening(EventManager.EVENT__TUTORIAL_END, OnTutorialEnd);
        EventManager.Instance.StartListening(EventManager.EVENT__TUTORIAL_TURTLE_VISIBLE, OnTutorialTurtleVisible);



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
        this.listPlayerReadyToContinue = new List<bool>(NUM_OF_PLAYERS) { false, false };
        this.listShouldRetryShape = new List<bool>(NUM_OF_PLAYERS) { false, false };
        this.listShapeFailedTooMuch = new List<bool>(NUM_OF_PLAYERS) { false, false };
        this.listShapeFailedTooLittle = new List<bool>(NUM_OF_PLAYERS) { false, false };

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

                if (!this.isTutorialOver && this.CanFinishRound())
                {
                    StartCoroutine(this.WaitAndRun(this.stayOnWinningStateTime, this.HideTutorialScreen));
                }
                
                //this.listCurrentScripts[this.idxRed].ResetShape();
                //this.listCurrentScripts[this.idxGreen].ResetShape();

                if (this.isTutorialOver)
                {
                    if (this.CanFinishRound())
                    {
                        this.HandleRoundWinner();
                    }

                }
            }

            //Debug.Log(this.timer);

            if (this.isWaitingForTimer && this.timer >= this.stayOnWinningStateTime)
            {
                this.isReadyForNextRound = true;
                this.isWaitingForTimer = false;
                
                if (this.isTutorialOver)
                {
                    bool bothFailedTooMuch = this.listShapeFailedTooMuch[this.idxRed] && this.listShapeFailedTooMuch[this.idxGreen];
                    bool bothFailedTooLittle = this.listShapeFailedTooLittle[this.idxRed] && this.listShapeFailedTooLittle[this.idxGreen];

                    if (bothFailedTooMuch || bothFailedTooLittle)
                    {
                        this.listCurrentScripts[this.idxRed].ResetShape();
                        //this.listCurrentShapes[this.idxRed].Animator().SetState(0);

                        this.listCurrentScripts[this.idxGreen].ResetShape();
                        //this.listCurrentShapes[this.idxGreen].Animator().SetState(0);

                        //this.UpdateCurrents();
                        // TODO: shake form animation
                        // TODO: try again
                        // TODO: constant shape
                    }
                    else
                    {
                        this.winnersNotice[this.idxRed].Animator().SetState(0);
                        this.winnersNotice[this.idxGreen].Animator().SetState(0);
                        this.listCurrentShapes[this.idxRed].SetActive(false);
                        this.listCurrentShapes[this.idxGreen].SetActive(false);
                    }
                }
                else
                {
                    if (this.listShapeFailedTooMuch[this.idxRed] || this.listShapeFailedTooLittle[this.idxRed])
                    {
                        this.listCurrentScripts[this.idxRed].ResetShape();
                        //this.listCurrentShapes[this.idxRed].Animator().SetState(0);

                        this.listCurrentScripts[this.idxGreen].ResetShape();
                        //this.listCurrentShapes[this.idxGreen].Animator().SetState(0);

                        //this.UpdateCurrents();
                        // TODO: shake form animation
                        // TODO: try again
                        // TODO: constant shape
                    }
                }

                
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
                    this.shiftToHold[this.idxRed].Animator().SetState(Animators.SHIFT__TO_HOLD);
                }

                if (Input.GetKey(this.player2DrawLineKey))
                {
                    this.shiftToHold[this.idxGreen].Animator().SetState(Animators.SHIFT__TO_HOLD);
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

    #endregion

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
                }
                else
                {
                    winner = idxRed;
                    loser = idxGreen;
                }

                this.lastWinner = winner;
                this.points[winner]++;
                
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
            //this.listCurrentShapes[this.idxRed] = this.shiftToHold[this.idxRed];
            this.listCurrentShapes[this.idxRed].SetActive(true);

            this.listCurrentShapes[this.idxGreen] = this.tutorialGreenShape;
            //this.listCurrentShapes[this.idxGreen] = this.shiftToHold[this.idxGreen];
            this.listCurrentShapes[this.idxGreen].SetActive(true);
        }

        this.listCurrentScripts[this.idxRed] = this.listCurrentShapes[this.idxRed].GetComponent(typeof(GameShape)) as GameShape;
        //this.listCurrentScripts[this.idxRed].IsTutorial = !this.isTutorialOver;
        this.DisableShapesExcept(this.listRedConstellations, this.listCurrentShapes[this.idxRed], this.idxRed);
        this.listCurrentScripts[this.idxRed].IsActive = true;
        this.listCurrentScripts[this.idxRed].MinimumSuccessRate = this.GetCurrentMinimumSuccessRate(this.idxRed);
        this.listShapeFailedTooMuch[this.idxRed] = false;
        this.listShapeFailedTooLittle[this.idxRed] = false;

        this.listCurrentScripts[this.idxGreen] = this.listCurrentShapes[this.idxGreen].GetComponent(typeof(GameShape)) as GameShape;
        //this.listCurrentScripts[this.idxGreen].IsTutorial = !this.isTutorialOver;
        this.DisableShapesExcept(this.listGreenConstellations, this.listCurrentShapes[this.idxGreen], this.idxGreen);
        this.listCurrentScripts[this.idxGreen].IsActive = true;
        this.listCurrentScripts[this.idxGreen].MinimumSuccessRate = this.GetCurrentMinimumSuccessRate(this.idxGreen);
        this.listShapeFailedTooMuch[this.idxGreen] = false;
        this.listShapeFailedTooLittle[this.idxGreen] = false;

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
    
    private bool WaitTime(float time)
    {
        return (time >= this.timer);
    }

    private void NewGame()
    {
        SceneManager.LoadScene(0);
    }

    private double GetCurrentMinimumSuccessRate(int playerIndex)
    {
        int currIdx = this.isTutorialOver ? this.currentShapeIndex + 1 : 0;
        return this.minimunSuccessRate[currIdx] / 100.0;
    }

    private bool IsPassingSuccessRate(int playerIndex)
    {
        return this.listCurrentScripts[playerIndex].GetSuccessRate >= this.GetCurrentMinimumSuccessRate(playerIndex);
    }

    private bool CanFinishRound()
    {
        bool bothFailedTooMuch = this.listShapeFailedTooMuch[this.idxRed] && this.listShapeFailedTooMuch[this.idxGreen];
        bool bothFailedTooLittle = this.listShapeFailedTooLittle[this.idxRed] && this.listShapeFailedTooLittle[this.idxGreen];

        if (!this.isTutorialOver)
        {
            return !bothFailedTooMuch && !bothFailedTooLittle; 
        }
        else
        {
            return !(bothFailedTooMuch && bothFailedTooLittle);
        }
    }

    private IEnumerator WaitAndRun(float waitSeconds, System.Action action)
    {
        yield return new WaitForSeconds(waitSeconds);
        Debug.Log("Waited " + waitSeconds + " seconds. Now running: " + action);
        action();
    }

    private IEnumerator WaitAndRun(float waitSeconds, System.Action<object> action, object triggerValue)
    {
        yield return new WaitForSeconds(waitSeconds);
        Debug.Log("Waited " + waitSeconds + " seconds. Now running: " + action);
        action(triggerValue);
    }

    private IEnumerator WaitAndRun(float waitSeconds, System.Action<string, object> action, string eventName, object triggerValue)
    {
        yield return new WaitForSeconds(waitSeconds);
        Debug.Log("Waited " + waitSeconds + " seconds. Now running: " + action);
        action(eventName, triggerValue);
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
        //this.isWaitingForTimer = true;
        //this.timer = 0;
    }

    private void OnShapeFinishStreching(object obj)
    {
        int playerNumber = (int)obj;
        this.shiftToHold[playerNumber].Animator().SetState(Animators.SHIFT__FADE_OUT);
    }
    
    private void OnShapeStartDrawing(object obj)
    {
        int playerNumber = (int)obj;
    }

    private void OnShapeFinishDrawing(object obj)
    {
        int playerNumber = (int)obj;

        this.listPlayerReadyToContinue[playerNumber] = true;

        if (this.lastWinner > -1 && this.listPlayerReadyToContinue[this.idxRed] && this.listPlayerReadyToContinue[this.idxGreen])
        {
            Debug.Log("finished drawing " + playerNumber);

            this.listWorldLands[this.lastWinner][this.currentShapeIndex - 1].Animator().SetState(Animators.LAND__ON);
            this.winnersNotice[this.lastWinner].Animator().SetState(1);

            this.listPlayerReadyToContinue[this.idxRed] = false;
            this.listPlayerReadyToContinue[this.idxGreen] = false;

            StartCoroutine(WaitAndRun(this.stayOnWinningStateTime, OnShapeFinishShowingLine, this.idxRed));
            StartCoroutine(WaitAndRun(this.stayOnWinningStateTime, OnShapeFinishShowingLine, this.idxGreen));
        }
    }

    private void OnShapeFinishShowingLine(object obj)
    {
        int playerNumber = (int)obj;
        this.listPlayerReadyToContinue[playerNumber] = true;
        
        if (this.lastWinner > -1 && this.listPlayerReadyToContinue[this.idxRed] && this.listPlayerReadyToContinue[this.idxGreen])
        {
            Debug.Log(Helpers.GetCurrentMethod());
            
            string sConstellationLetter = playerNumber == this.idxRed ? "R" : "G";
            string sOtherConstellationLetter = playerNumber == this.idxRed ? "G" : "R";

            Debug.Log("last winner: " + this.lastWinner);
            this.listCurrentShapes[this.lastWinner].Animator().SetState(Animators.CONSTELLATION__SHAPE_WIN);
            //this.listCurrentShapes[this.lastWinner].Animator().CrossFade(sConstellationLetter + "Constellation_ShapeWin", 2);

            int otherPlayer = this.lastWinner == this.idxRed ? this.idxGreen : this.idxRed;

            this.listCurrentShapes[otherPlayer].Animator().SetState(Animators.CONSTELLATION__FADE_OUT);
            //this.listCurrentShapes[otherPlayer].Animator().CrossFade(sOtherConstellationLetter + "Constellation_FadeOut", 0);

            this.listPlayerReadyToContinue[this.idxRed] = false;
            this.listPlayerReadyToContinue[this.idxGreen] = false;

            this.isWaitingForTimer = true;
            this.timer = 0;


            this.listCurrentScripts[this.idxRed].ResetShape();
            this.listCurrentScripts[this.idxGreen].ResetShape();
        }
    }

    private void OnShapeFailedTooMuch(object obj)
    {
        int playerNumber = (int)obj;
        this.listShapeFailedTooMuch[playerNumber] = true;
    }

    private void OnShapeFailedTooLIttle(object obj)
    {
        int playerNumber = (int)obj;
        this.listShapeFailedTooLittle[playerNumber] = true;
    }

    private void OnTutorialTurtleVisible(object obj)
    {

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
        this.listCurrentScripts[this.idxRed].ResetShape();
        this.listCurrentScripts[this.idxGreen].ResetShape();
        this.shiftToHold[this.idxRed].Animator().SetState(Animators.SHIFT__FADE_OUT);
        this.shiftToHold[this.idxGreen].Animator().SetState(Animators.SHIFT__FADE_OUT);
        //StartCoroutine(this.WaitAndRun(1, this.listCurrentScripts[this.idxRed].ResetShape));
        //StartCoroutine(this.WaitAndRun(1, this.listCurrentScripts[this.idxGreen].ResetShape));
        this.openScreen.Animator().SetState(Animators.OPEN_SCREEN__FADE_OUT);
        
        //DisableShape(this.tutorialRedShape);
        //DisableShape(this.tutorialGreenShape);
    }

    private void ShowMainScreen()
    {
        this.turtleDisc.Animator().SetState(Animators.TURTLE_DISC__FADE_IN);
    }
    
    #endregion
}
