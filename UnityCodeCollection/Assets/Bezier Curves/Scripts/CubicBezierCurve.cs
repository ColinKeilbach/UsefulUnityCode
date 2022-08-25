using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{
    public class CubicBezierCurve : BezierCurve
    {
        //Control Points
        private Vector3 P0;
        private Vector3 P1;
        private Vector3 P2;
        private Vector3 P3;

        protected override void OnStart()
        {
            if (points.Count != 4)
                throw new System.Exception("CubicBezierCurve needs exactly 4 control points.");

            P0 = points[0];
            P1 = points[1];
            P2 = points[2];
            P3 = points[3];
        }

        private float Square(float x) => x * x;
        private float Cube(float x) => x * x * x;

        public override Vector3 GetCurvePosition(float t)
        {
            Vector3 Q0 = Cube(1 - t) * P0;
            Vector3 Q1 = 3 * Square(1 - t) * t * P1;
            Vector3 Q2 = 3 * (1 - t) * Square(t) * P2;
            Vector3 Q3 = Cube(t) * P3;
            Vector3 result = Q0 + Q1 + Q2 + Q3;

            return result;
        }
    }
}