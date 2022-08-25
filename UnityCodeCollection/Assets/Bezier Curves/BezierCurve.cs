using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{

    public class BezierCurve : MonoBehaviour
    {
        const int maxRecommendedPoints = 10;
        const float lengthPrecision = 0.05f;

        private List<Vector3> points = new List<Vector3>();
        [SerializeField]
        private float length;

        public float Length
        {
            get { return length; }
            private set { length = value; }
        }

        private void Awake()
        {
#if UNITY_EDITOR
            showGUI = false; // Prevents the GUI from changing anything in play mode
#endif

            UpdateList();
            UpdateLength(lengthPrecision);

            if (points.Count > maxRecommendedPoints)
                Debug.LogWarning("BezierCurve is using more than " + maxRecommendedPoints + " points");
        }

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
        private bool showGUI = true;

        private const float gizmoPrecision = 0.05f;
        private Vector3 gizmoPos;

        private void OnDrawGizmos()
        {
            if (showGUI)
            {
                UpdateList();

                for (float t = gizmoPrecision; t <= 1; t += gizmoPrecision)
                {
                    gizmoPos = GetCurvePosition(t);

                    Gizmos.color = Color.grey;
                    Gizmos.DrawSphere(gizmoPos, 0.25f);
                }

                foreach (Transform controlPoint in transform)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(controlPoint.position, 0.25f);
                }
            }
        }

#endif

        #endregion
    }
}