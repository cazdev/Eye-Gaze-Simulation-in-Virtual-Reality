using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace UnityEngine.XR.OpenXR.Samples.ControllerSample
{
    public class ActionToHaptics : MonoBehaviour
    {
        public InputActionReference action;
        public float _amplitude = 1.0f;
        public float _duration = 0.1f;

        public Transform indexFingerJoin1;
        public Transform indexFingerJoin2;
        public Transform thumbFingerJoint1;
        public GameObject WhiteToken;
        public GameObject BlackToken;

        public Material HighlightMat;

        public bool isGrabbed = false;
        private bool isInsideBlackPile = false;

        private void Start()
        {
            if (action == null)
                return;

            action.action.Enable();
            action.action.performed += (ctx) =>
            {
                var control = action.action.activeControl;
                if (null == control)
                    return;

                if (control.device is XRControllerWithRumble rumble)
                {
                    //rumble.SendImpulse(_amplitude, _duration);

                    // Check hand is inside pile
                    if (isInsideBlackPile)
                    {
                        // Grabbing a token with fingers
                        if (isGrabbed)
                        {
                            isGrabbed = false;
                            // Undo flexed fingers
                            indexFingerJoin1.Rotate(-55, 0, 0);
                            thumbFingerJoint1.Rotate(-30, 0, 0);
                            // Hide the token
                            BlackToken.SetActive(false);
                        }
                        else
                        {
                            isGrabbed = true;
                            // Flex fingers in to grab token
                            indexFingerJoin1.Rotate(55, 0, 0);
                            thumbFingerJoint1.Rotate(30, 0, 0);
                            // Show the token
                            BlackToken.SetActive(true);
                        }
                    }
                }
            };
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "BlackTokenPile")
            {
                isInsideBlackPile = true;
                other.gameObject.GetComponent<MeshRenderer>().material = HighlightMat;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "BlackTokenPile")
            {
                isInsideBlackPile = false;
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.clear; ;
            }
        }
    }
}
