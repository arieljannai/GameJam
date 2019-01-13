using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionsMethods;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    #region AppConfiguration
    public Color player1Color;
    public Color player2Color;
    public float linesWidth;
    public float lineStrechingSpeed;
    public string player1DrawLineKey;
    public string player2DrawLineKey;
    public GameObject worldRedLand;
    public GameObject worldGreenLand;
    public GameObject constellationsRed;
    public GameObject constellationsGreen;
    public GameObject tutorialRedShape;
    public GameObject tutorialGreenShape;
    public int nShapesCount;
    public bool playingWithTutorial;
    public List<GameObject> shiftTexts;
    public List<GameObject> holdTexts;
    #endregion

    private List<GameObject> listWorldRedland;
    private List<GameObject> listWorldGreenland;
    private List<GameObject> listRedConstellations;
    private List<GameObject> listGreenConstellations;
    private List<GameObject> listTutorialShapes;
    private List<List<GameObject>> listWorldLands;
    private List<List<GameObject>> listConstellations;
    private GameObject currentRedShape;
    private GameObject currentGreenShape;
    private GameShape currentRedScript;
    private GameShape currentGreenScript;
    private int[] points;
    private int idxRed = 0;
    private int idxGreen = 1;
    private int currentShapeIndex;
    private bool isGameOver = false;
    private bool isReadyForNextRound = false;
    private bool isTutorialOver = false;

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
        if (!this.playingWithTutorial)
        {
            this.isTutorialOver = true;
        }

        Debug.Log("GameController.Start()");
        this.listWorldRedland = this.worldRedLand.GetChildrenGameObjects();
        this.listWorldGreenland = this.worldGreenLand.GetChildrenGameObjects();
        this.listRedConstellations = this.constellationsRed.GetChildrenGameObjects();
        this.listGreenConstellations = this.constellationsGreen.GetChildrenGameObjects();

        this.listTutorialShapes = new List<GameObject>(NUM_OF_PLAYERS) { this.tutorialRedShape, this.tutorialGreenShape };
        this.listWorldLands = new List<List<GameObject>>(NUM_OF_PLAYERS) { this.listWorldRedland, this.listWorldGreenland };
        this.listConstellations = new List<List<GameObject>>(NUM_OF_PLAYERS) { this.listRedConstellations, this.listGreenConstellations };

        this.points = new int[NUM_OF_PLAYERS];

        this.UpdateCurrents();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentRedScript.IsReadyForDrawing && this.currentGreenScript.IsReadyForDrawing)
        {
            this.currentRedScript.ShouldDraw = true;
            this.currentGreenScript.ShouldDraw = true;

            this.currentRedScript.IsReadyForDrawing = false;
            this.currentGreenScript.IsReadyForDrawing = false;

            this.ReplaceShiftHold(idxGreen);
            this.ReplaceShiftHold(idxRed);

            this.HandleRoundWinner();
        }

        if (this.isReadyForNextRound)
        {
            this.IncreaseShapesAndUpdatesCurrents();
            this.isReadyForNextRound = false;
        }
    }

    private void HandleRoundWinner()
    {
        int res = GameShape.Compare(this.currentRedScript, this.currentGreenScript);
        int winner, loser;

        if (res == 0)
        {
            Debug.Log("Tie! Nobody gets a point.");
            return;
        }

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

        
        if (this.isTutorialOver)
        {
            this.points[winner]++;
            this.listWorldLands[winner][this.currentShapeIndex].SetOpacity(255);
        }
        else
        {
            this.isTutorialOver = true;
        }

        Debug.Log(string.Format("Player {0} won this round. {0}: {1}; {2}: {3}",
            (GameShape.Players)(winner + 1), this.points[winner], (GameShape.Players)(loser + 1), this.points[loser]));
    }

    private void IncreaseShapesAndUpdatesCurrents()
    {
        if (this.currentShapeIndex == this.nShapesCount - 1)
        {
            this.isGameOver = true;
        }
        else
        {
            this.currentShapeIndex++;
            this.UpdateCurrents();
        }
    }

    private void UpdateCurrents()
    {
        if (this.isTutorialOver)
        {
            this.currentRedShape = this.listRedConstellations[this.currentShapeIndex];
            this.currentGreenShape = this.listGreenConstellations[this.currentShapeIndex];
        }
        else
        {
            this.currentRedShape = this.tutorialRedShape;
            this.currentGreenShape = this.tutorialGreenShape;
        }
        
        this.currentRedScript = this.currentRedShape.GetComponent(typeof(GameShape)) as GameShape;
        this.currentRedShape.SetOpacity(255);
        this.DisableShapesExcept(this.listRedConstellations, this.currentRedShape);

        this.currentGreenScript = this.currentGreenShape.GetComponent(typeof(GameShape)) as GameShape;
        this.currentGreenShape.SetOpacity(255);
        this.DisableShapesExcept(this.listGreenConstellations, this.currentGreenShape);
    }

    private void DisableShapesExcept(List<GameObject> list, GameObject obj)
    {
        this.SetOpacityExcept(list, 0, obj);
        list.ForEach(go => go.SetActive(go == obj));
    }

    private void SetOpacityExcept(List<GameObject> list, float opacity, GameObject obj)
    {
        foreach (GameObject go in list)
        {
            if (go != obj)
            {
                go.SetOpacity(opacity);
            }
        }
    }

    private void SetOpacityExcept(List<GameObject> list, float opacity, params GameObject[] objects)
    {
        HashSet<GameObject> hs = new HashSet<GameObject>(objects);

        foreach (GameObject go in list)
        {
            if (!hs.Contains(go))
            {
                go.SetOpacity(opacity);
            }
        }
    }

    private void ReplaceShiftHold(int idxPlayer)
    {
        SpriteRenderer goShift = this.shiftTexts[idxPlayer].GetComponent<SpriteRenderer>();
        GameObject goHold = this.holdTexts[idxPlayer];

        goShift.enabled = !goShift.enabled;
        float oHold = goHold.GetOpacity();
        goHold.SetOpacity((float)1 - oHold);
    }
}
