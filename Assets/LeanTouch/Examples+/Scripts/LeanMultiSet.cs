using UnityEngine;
using UnityEngine.Events;
using GoogleARCore.Examples.HelloAR;

namespace Lean.Touch
{
    // This component allows you to get the center and delta of all fingers
    public class LeanMultiSet : MonoBehaviour
    {
        public enum DeltaCoordinatesType
        {
            Screen,
            Scaled
        }

        // Event signature
        [System.Serializable] public class Vector2Event : UnityEvent<Vector2> { }

        [Tooltip("Ignore fingers with StartedOverGui?")]
        public bool IgnoreStartedOverGui = true;

        [Tooltip("Ignore fingers with IsOverGui?")]
        public bool IgnoreIsOverGui;

        [Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
        public int RequiredFingerCount;

        [Tooltip("If the finger didn't move, ignore it?")]
        public bool IgnoreIfStatic;

        [Tooltip("If RequiredSelectable.IsSelected is false, ignore?")]
        public LeanSelectable RequiredSelectable;

        [Tooltip("The coordinate space of the OnSetDelta values")]
        public DeltaCoordinatesType DeltaCoordinates;

        public Vector2Event OnSetCenter;

        public Vector2Event OnSetDelta;
        public float publicdeltax;
        public float publicdeltay;


#if UNITY_EDITOR
        protected virtual void Reset()
        {
            Start();
        }
#endif

        protected virtual void Start()
        {
            if (RequiredSelectable == null)
            {
                RequiredSelectable = GetComponent<LeanSelectable>();
            }
        }

        protected virtual void Update()
        {
            // Get fingers
            var fingers = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount, RequiredSelectable);

            if (fingers.Count > 0)
            {
                // Get delta
                var delta = LeanGesture.GetScreenDelta(fingers);

                //Debug.Log("This is ORIGIN(" + delta.x + "," + delta.y + ")"); //origin will graduately comes down to zero because there is no move (no delta)
                var DELTAX = delta.x;
                var DELTAY = delta.y;
                publicdeltax = DELTAX; //this variable is public so that it can be used in other class
                publicdeltay = DELTAY;


                // Ignore?
                if (delta.sqrMagnitude == 0.0f)
                {
                    return;
                }

                // Scale?
                if (DeltaCoordinates == DeltaCoordinatesType.Scaled)
                {
                    delta *= LeanTouch.ScalingFactor;
                }

                // Call events
                if (OnSetCenter != null)
                {
                    OnSetCenter.Invoke(LeanGesture.GetScreenCenter(fingers));
                }

                if (OnSetDelta != null)
                {
                    OnSetDelta.Invoke(delta);
                }
            }
            else
            { //Debug.Log("DIDNT TOUCH"); }
            }
        }




    }
}
