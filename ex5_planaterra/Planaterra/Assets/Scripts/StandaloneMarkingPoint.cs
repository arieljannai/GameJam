using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class StandaloneMarkingPoint
    {
        private GameObject point;
        private LineRenderer lineRenderer;
        private GameShape.Players owner;

        public StandaloneMarkingPoint(GameObject point, GameShape.Players owner) : this(point, owner, point.GetComponent<LineRenderer>()) { }
        
        public StandaloneMarkingPoint(GameObject point, GameShape.Players owner, LineRenderer lr)
        {
            this.point = point;
            this.owner = owner;

            if (lr != null)
            {
                this.lineRenderer = lr;
                this.lineRenderer.startWidth = GameController.Instance.linesWidth;
                this.lineRenderer.endWidth = GameController.Instance.linesWidth;
            }
        }

        public GameObject Point
        {
            get { return this.point; }
        }

        public LineRenderer Renderer
        {
            get { return this.lineRenderer; }
        }

        public GameShape.Players Owner
        {
            get { return this.owner; }
        }
    }
}
