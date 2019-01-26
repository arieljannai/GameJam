using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionsMethods;

public class GameShape : MonoBehaviour
{
    public Players playerNumber;
    public float fadeOutSpeed;

    private Vector2 gameLineStart;
    private Vector2 gameLineEnd;
    private List<Vector2> points;
    private List<Vector2> pointsToDraw;
    private LineRenderer lrShape;
    private List<GameObject> markingDots;
    private StandaloneMarkingPoint mdFinalPoint;
    private StandaloneMarkingPoint mdStrechingLine;
    private bool isReadyForDrawing = false;
    private bool canDraw = true;
    private bool shouldDraw = false;
    private bool finishedDrawing = false;
    private bool isActive = false;
    private bool isTutorial = false;
    private bool shouldTryAgain = false;
    private bool disableWhenDone = false;
    private bool shouldFadeOut = false;
    private float strechingLineDrawingSpeed;
    private float strechingLineLength;
    private float wrappingPartLineLength;
    private float formLength;
    private float successRate;
    private int pointsAmount;
    private int wrappingLineTotalPoints = 120;
    private int idxHandleLineDrawing = 0;
    private string keyDrawLine;
    private Dictionary<string, Transform> transformsDict;

    public enum Players
    {
        P1 = 1,
        P2 = 2
    }

    #region Unity Events
    
    // Start is called before the first frame update
    void Start()
    {
        this.InitObjects();

        Transform dots = this.transform.Find("Dots");
        Transform tMarkingDots = dots.Find("Marking_Dots");
        this.transformsDict.Add("Dots", dots);
        this.transformsDict.Add("Marking_Dots", tMarkingDots);
        this.mdFinalPoint = new StandaloneMarkingPoint(dots.Find("Final_Point").gameObject, this.playerNumber, null);
        this.mdStrechingLine = new StandaloneMarkingPoint(dots.Find("Streching_Line").gameObject, this.playerNumber);

        for (int i = 0; i < tMarkingDots.childCount; i++)
        {
            this.markingDots.Add(tMarkingDots.GetChild(i).gameObject);
        }

        this.lrShape = this.markingDots[0].GetComponent<LineRenderer>();
        this.lrShape.startWidth = GameController.Instance.linesWidth;
        this.lrShape.endWidth = GameController.Instance.linesWidth;
        this.markingDots.ForEach(g => points.Add(g.transform.position));
        this.SetMarkingPointsOpacity(0);

        this.CalcFormLength();

        this.pointsAmount = this.points.Count;

        this.gameLineStart = this.points[0];
        this.gameLineEnd = this.gameLineStart;

        this.strechingLineDrawingSpeed = GameController.Instance.lineStrechingSpeed;
        this.keyDrawLine = this.playerNumber == Players.P1 ? GameController.Instance.player1DrawLineKey : GameController.Instance.player2DrawLineKey;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                this.ResetShape();
                this.canDraw = true;
                // TODO: reset everything
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.isReadyForDrawing = true;
            }

            if (this.shouldDraw)
            {
                this.HandleLineDrawing();
            }

            if (Input.GetKey(this.keyDrawLine) && this.canDraw)
            {
                this.gameLineEnd += new Vector2(0f, this.strechingLineDrawingSpeed);
                this.mdStrechingLine.Renderer.SetPositions(new Vector3[] { this.gameLineStart, this.gameLineEnd });
                this.finishedDrawing = true;
            }
            else
            {
                if (this.finishedDrawing)
                {
                    this.canDraw = false;
                    this.CalculateLinesToDraw();
                    this.isReadyForDrawing = true;
                    this.finishedDrawing = false;
                    EventManager.Instance.TriggerEvent(EventManager.EVENT__STARTED_DRAWING, this.GetPlayerIndex());
                }
            }
        }
    }

    #endregion

    #region Properties

    public bool IsReadyForDrawing
    {
        get { return this.isReadyForDrawing; }
        set { this.isReadyForDrawing = value; }
    }

    public bool ShouldDraw
    {
        get { return this.shouldDraw; }
        set { this.shouldDraw = value; }
    }

    public bool IsActive
    {
        get { return this.isActive; }
        set { this.isActive = value; }
    }

    public bool IsTutorial
    {
        get { return this.isTutorial; }
        set { this.isTutorial = value; }
    }

    public bool ShouldTryAgain
    {
        get { return this.shouldTryAgain; }
        set { this.shouldTryAgain = value; }
    }

    public bool DisableWhenDone
    {
        get { return this.disableWhenDone; }
        set { this.disableWhenDone = value; }
    }

    public float GetSuccessRate
    {
        get { return this.successRate; }
    } 

    #endregion

    public static int Compare(GameShape gs1, GameShape gs2)
    {
        float gs1SR = gs1.GetSuccessRate > 1 ? -1 : gs1.GetSuccessRate;
        float gs2SR = gs2.GetSuccessRate > 1 ? -1 : gs2.GetSuccessRate;

        if (gs1SR < gs2SR)
        {
            return -1;
        }
        else if (gs1SR > gs2SR)
        {
            return 1;
        }

        return 0;
    }

    void InitEverything()
    {
        
    }

    private void HandleLineDrawing()
    {
        int i = this.idxHandleLineDrawing;
        Vector3[] positions;


        if (i < this.pointsToDraw.Count - 1)
        {   
            positions = this.pointsToDraw.GetRange(0, i + 2).ToVector3Array();

            this.lrShape.positionCount = positions.Length;
            this.lrShape.SetPositions(positions);

            Vector3 vStrechingLine = this.mdStrechingLine.Point.transform.position;
            Vector3 vStrechingLineEnd = new Vector3(vStrechingLine.x, vStrechingLine.y + (this.strechingLineLength - this.wrappingPartLineLength * (i+2)));
            positions = new Vector3[] { vStrechingLine, vStrechingLineEnd};
            this.mdStrechingLine.Renderer.SetPositions(positions);
            
            i++;
        }
        else
        {
            Vector3 vStrechingLine = this.mdStrechingLine.Point.transform.position;
            this.mdStrechingLine.Renderer.SetPositions(new Vector3[] { vStrechingLine, vStrechingLine });

            this.isReadyForDrawing = false;
            this.shouldDraw = false;

            if (this.disableWhenDone)
            {
                //this.gameObject.SetOpacity(0);
                this.gameObject.SetActive(false);
                //this.shouldFadeOut = true;

            }

            this.ResetShape();
            EventManager.Instance.TriggerEvent(EventManager.EVENT__FINISHED_DRAWING, this.GetPlayerIndex());
        }

        this.idxHandleLineDrawing = i;
    }

    private void CalculateLinesToDraw()
    {
        //Log(string.Format("this.gameLineStart:{0}, this.gameLineEnd:{1}", this.gameLineStart, this.gameLineEnd));
        this.strechingLineLength = Vector2.Distance(this.gameLineStart, this.gameLineEnd);

        if (this.isTutorial)
        {
            if ((this.strechingLineLength / this.formLength) >= 0.5)
            {
                this.strechingLineLength = this.formLength;
            }
            else
            {
                this.shouldTryAgain = true;
            }
        }

        float dLineLength = this.strechingLineLength;
        this.successRate = dLineLength / this.formLength;
        Log("line length: " + dLineLength);
        Log("form length: " + this.formLength);
        Log("success rate: " + this.successRate);


        if (dLineLength != 0)
        {
            this.AddPointToDraw(this.gameLineStart);

            for (int i = 0; i < this.pointsAmount; i++)
            {
                int idx = i % this.pointsAmount;
                int idxP1 = (i + 1) % this.pointsAmount;
                float distance = Vector2.Distance(this.points[idx], this.points[idxP1]);

                dLineLength -= distance;

                if (dLineLength >= 0)
                {
                    this.AddPointToDraw(this.points[idxP1]);

                    if (i == this.pointsAmount - 1)
                    {
                        if (dLineLength > 0)
                        {
                            Vector2 newV = new Vector2(this.points[idxP1].x, this.points[idxP1].y + dLineLength);
                            this.mdFinalPoint.Point.transform.position = newV;
                            this.AddPointToDraw(newV);
                        }
                    }
                }
                else
                {
                    Vector2 v1 = this.points[idx];
                    Vector2 v2 = this.points[idxP1];
                    float percentage = -dLineLength / distance;
                    dLineLength = 0;
                    Vector2 newV = RelativeDistance(v1, v2, percentage);

                    //Log(string.Format("v1:{0}, v2:{1}, percentage:{2}, distance:{3}, newV:{4}", v1, v2, percentage, distance, newV));

                    this.mdFinalPoint.Point.transform.position = newV;
                    this.AddPointToDraw(newV);

                    break;
                }
            }
            
            //this.lrShape.positionCount = this.pointsToDraw.Count;
            //this.lrShape.SetPositions(this.toVector3Arr(this.pointsToDraw));
        }
    }

    private Vector2 RelativeDistance(Vector2 v1, Vector2 v2, float d)
    {
        return new Vector2(RelativeDistance(v1.x, v2.x, d), RelativeDistance(v1.y, v2.y, d));
    }

    private float RelativeDistance(float p1, float p2, float d)
    {
        return (d * p1) + (1 - d) * p2;
    }

    private void MarkLocation(Vector2 v)
    {
        this.MarkLocation(v, Color.yellow, 10);
    }

    private void MarkLocation(Vector2 v, Color color, float duration)
    {
        Debug.DrawLine(v, v + new Vector2(0.01f, 0.01f), color, duration);
        Debug.DrawLine(v, v + new Vector2(-0.01f, 0.01f), color, duration);
        Debug.DrawLine(v, v + new Vector2(0.01f, -0.01f), color, duration);
        Debug.DrawLine(v, v + new Vector2(-0.01f, -0.01f), color, duration);
    }
    
    private void Log(object s)
    {
        Debug.Log(this.playerNumber + ": " + s);
    }

    private void AddPointToDraw(Vector2 v)
    {
        int nCurrPoints = this.pointsToDraw.Count;
        if (nCurrPoints > 1)
        {
            Vector2 vPrev = this.pointsToDraw[nCurrPoints - 1];
            float distance = Vector2.Distance(vPrev, v);
            float percentage = this.wrappingPartLineLength / distance;
            
            for (float i = 0; i < distance; i += this.wrappingPartLineLength)
            {
                vPrev = this.RelativeDistance(vPrev, v, percentage);
                this.pointsToDraw.Add(vPrev);
            }
        }

        this.pointsToDraw.Add(v);
    }

    private string VectorString(Vector2 v)
    {
        return string.Format("({0}, {1})", v.x, v.y);
    }

    public void ResetShape()
    {
        this.InitEverything();
        this.idxHandleLineDrawing = 0;
        this.isReadyForDrawing = false;
        this.gameLineEnd = this.gameLineStart;
        this.pointsToDraw.Clear();
        this.mdStrechingLine.Renderer.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
        this.lrShape.SetPositions(new Vector3[] { this.points[0], this.points[0] });
        this.lrShape.positionCount = 2;
    }

    private void InitObjects()
    {
        this.transformsDict = new Dictionary<string, Transform>();
        this.markingDots = new List<GameObject>();
        this.points = new List<Vector2>();
        this.pointsToDraw = new List<Vector2>();
    }

    private void CalcFormLength()
    {
        this.formLength = Vector2.Distance(this.points[this.points.Count - 1], this.points[0]);
        for (int i = 0; i < this.points.Count - 1; i++)
        {
            this.formLength += Vector2.Distance(this.points[i], this.points[i + 1]);
        }

        this.wrappingPartLineLength = this.formLength / (this.wrappingLineTotalPoints - this.points.Count);
    }

    private Vector3[] ToVector3Arr(List<Vector2> l)
    {
        Vector3[] arr = new Vector3[l.Count];
        int i = 0;

        foreach (Vector2 v in l)
        {
            arr[i] = v;
            i++;
        }

        return arr;
    }

    private void SetMarkingPointsOpacity(float opacity)
    {
        this.markingDots.ForEach(x => x.SetOpacity(opacity));
        this.mdFinalPoint.Point.SetOpacity(opacity);
        this.mdStrechingLine.Point.SetOpacity(opacity);
    }

    public int GetPlayerIndex()
    {
        return this.playerNumber == Players.P1 ? 0 : 1;
    }

    #region Events

    public void Constellation_FadeOut_End(string endState)
    {
        EventManager.Instance.TriggerEvent(EventManager.EVENT__CONSTELLATION_END, endState);
    }

    #endregion
}
