using UnityEngine;



namespace Lean.Touch
{
    // This modifies LeanManualRotate2D to be smooth
    public class LeanManualRotate2DSmooth : LeanManualRotate2D
    {
        [Tooltip("How quickly the rotation goes to the target value")]
        public float Dampening = 10.0f;

        [System.NonSerialized]
        private Quaternion remainingDelta = Quaternion.identity;
        




        public override void Rotate(Vector2 delta)
        {
            //Debug.Log("this is x " + delta.x + " this is y " + delta.y);

            // Rotate and increment by delta
            var oldRotation = transform.localRotation;

            base.Rotate(delta);

            remainingDelta *= Quaternion.Inverse(oldRotation) * transform.localRotation;

            // Revert
            transform.localRotation = oldRotation;
           
               

                if (Mathf.Abs(leanMultiSet.publicdeltax) >= Mathf.Abs(leanMultiSet.publicdeltay)) //this abs will let the object to move only one axis at the time
                {
                    transform.Rotate(AxisA, leanMultiSet.publicdeltax * AngleMultiplier, Space);
                }
                else
                {
                    transform.Rotate(AxisB, leanMultiSet.publicdeltay * AngleMultiplier, Space);
                }
           

           
        }

        protected virtual void Update()
        {
            var factor = LeanTouch.GetDampenFactor(Dampening, Time.deltaTime);
            var newDelta = Quaternion.Slerp(remainingDelta, Quaternion.identity, factor);

            transform.localRotation = transform.localRotation * Quaternion.Inverse(newDelta) * remainingDelta;

            remainingDelta = newDelta;
        }


    


    }

 
}