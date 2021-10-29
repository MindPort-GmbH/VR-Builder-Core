using UnityEngine;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Properties
{
    [RequireComponent(typeof(BezierSpline))]
    public class BezierSplinePathProperty : TrainingSceneObjectProperty, IPathProperty
    {
        private BezierSpline spline;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (spline == null)
            {
                spline = GetComponent<BezierSpline>();
            }
        }
       
        /// <inheritdoc/>
        public Vector3 GetPoint(float t)
        {
            return spline.GetPoint(t);
        }

        /// <inheritdoc/>
        public Vector3 GetDirection(float t)
        {
            return spline.GetDirection(t);
        }
    }
}
