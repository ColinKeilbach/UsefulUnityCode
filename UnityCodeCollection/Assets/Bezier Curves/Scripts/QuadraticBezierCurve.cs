using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{
    public class QuadraticBezierCurve : BezierCurve
    {
        //Control Points
        private Vector3 P0;
        private Vector3 P1;
        private Vector3 P2;

        protected override void OnStart()
        {
            if (points.Count != 3)
                throw new System.Exception("QuadraticBezierCurve needs exactly 3 control points.");

            P0 = points[0];
            P1 = points[1];
            P2 = points[2];
        }

        private float Square(float x) => x * x;

        public override Vector3 GetCurvePosition(float t)
        {
            Vector3 Q0 = Square(1 - t) * P0;
            Vector3 Q1 = 2 * (1 - t) * t * P1;
            Vector3 Q2 = Square(t) * P2;
            Vector3 result = Q0 + Q1 + Q2;

            return result;
        }
    }
}
