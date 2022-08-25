using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{
    public class CirclularCurve : BezierCurve
    {
        private Vector3 center;
        private Vector3 radius;
        private Vector3 rotator;

        protected override void OnStart()
        {
            if (points.Count != 2)
                throw new System.Exception("CircularCurve needs exactly 2 control points.");

            center = transform.position;
            radius = points[0];
            rotator = points[1];
        }

        public override Vector3 GetCurvePosition(float t)
        {
            Vector3 normRadius = radius - center;
            Vector3 normRotator = rotator - center;
            normRotator.Normalize();
            normRotator *= normRadius.magnitude;

            float angle = Vector3.Angle(normRadius, normRotator);
            float scalarToFullCircle = 360 / angle;

            Vector3 result = Vector3.SlerpUnclamped(normRadius, normRotator, t * scalarToFullCircle) + center;

            return result;
        }
    }
}
