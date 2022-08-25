using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{
    public class BezierCurve : MonoBehaviour
    {
        const int maxRecommendedPoints = 10;
        const float lengthPrecision = 0.05f;

        protected List<Vector3> points = new List<Vector3>();
        protected float length;

        public float Length
        {
            get { return length; }
            protected set { length = value; }
        }

        private void Awake()
        {
#if UNITY_EDITOR
            showCurve = false; // Prevents the GUI from changing anything in play mode
#endif

            UpdateList();
            UpdateLength(lengthPrecision);

            switch (points.Count)
            {
                case 3:
                    Debug.LogWarning("BezierCurves with 3 control points should be QuadraticBezierCurves.");
                    break;
                case 4:
                    Debug.LogWarning("BezierCurves with 4 control points should be CubicBezierCurves.");
                    break;
            }

            if (points.Count > maxRecommendedPoints)
                Debug.LogWarning("BezierCurve is using more than " + maxRecommendedPoints + " control points.");
        }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnStart() { }

        #region Update Functions (Called on Awake)

        private void UpdateList()
        {
            points.Clear();

            foreach (Transform child in transform)
            {
                points.Add(child.position);
            }
        }

        private void UpdateLength(float precision)
        {
            List<Vector3> pointsOnCurve = new List<Vector3>();

            for (float t = 0; t <= 1; t += precision)
            {
                pointsOnCurve.Add(GetCurvePosition(t));
            }

            float l = 0;

            for (int i = 0; i < pointsOnCurve.Count - 1; i++)
            {
                l += Vector3.Distance(pointsOnCurve[i], pointsOnCurve[i + 1]);
            }

            length = l;
        }

        #endregion

        public virtual Vector3 GetCurvePosition(float t)
        {
            return GetCurvePosition(points, t);
        }
        private Vector3 GetCurvePosition(List<Vector3> points, float t)
        {
            List<Vector3> newPoints = new List<Vector3>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 newPoint = Vector3.Lerp(points[i], points[i + 1], t);
                newPoints.Add(newPoint);
            }

            if (newPoints.Count > 1)
                return GetCurvePosition(newPoints, t);
            else
                return newPoints[0];
        }

        #region Draw Gizmos (Editor Only)

#if UNITY_EDITOR

        [Header("Editor Only")]
        [SerializeField]
        private bool showCurve = true;
        [SerializeField]
        private bool showControlPoints = true;

        private const float gizmoPrecision = 0.05f;
        private Vector3 lastGizmoPos;
        private Vector3 gizmoPos;

        private void OnDrawGizmos()
        {
            if (showCurve)
            {
                UpdateList();
                OnStart();


                lastGizmoPos = GetCurvePosition(0);

                for (float t = gizmoPrecision; t <= 1 + gizmoPrecision; t += gizmoPrecision)
                {
                    gizmoPos = GetCurvePosition(t);

                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(lastGizmoPos, gizmoPos);

                    lastGizmoPos = gizmoPos;
                }

                if (showControlPoints)
                {
                    Transform lastControlPoint = null;

                    foreach (Transform controlPoint in transform)
                    {
                        Gizmos.color = Color.gray;
                        if (lastControlPoint != null)
                            Gizmos.DrawLine(lastControlPoint.position, controlPoint.position);

                        lastControlPoint = controlPoint;

                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(lastControlPoint.position, 0.05f);
                    }
                }
            }
        }

#endif

        #endregion
    }
}