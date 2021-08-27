using System;
using AdvancedUtilities.Cameras.Components;
using UnityEngine;

namespace AdvancedUtilities.Cameras
{
    /// <summary>
    /// A camera controller that can hold a position and look at a target.
    /// When using this, don't make the camera a child of the target.
    /// </summary>
    [Serializable]
    public class OverplanetCameraController : AdvancedUtilities.Cameras.CameraController
    {
        public TargetComponent Target;

        /// <summary>
        /// The distance from the target in the last update.
        /// </summary>
        private float _previousDistance;

        protected override void AddCameraComponents()
        {
            AddCameraComponent(Target);
        }

        void Start()
        {
            CameraTransform.Position = this.transform.position; // First move it to the holding position
        }

        void Update()
        {
            UpdateCamera();
            CameraTransform.ApplyTo(Camera); // Apply the virtual transform to the actual transform
        }

        public override void UpdateCamera()
        {
            CameraTransform.Position = this.transform.position;
            CameraTransform.Rotation = Quaternion.Euler(Target.Target.rotation.eulerAngles.x + 90, Target.Target.rotation.eulerAngles.y, Target.Target.rotation.eulerAngles.z);
        }
    }
}
