using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabToken : MonoBehaviour
{
    public InputActionReference action;
    public float _amplitude = 1.0f;
    public float _duration = 0.1f;

    public Transform indexFingerJoin1;
    public Transform thumbFingerJoint1;
    public GameObject WhiteToken;
    public GameObject BlackToken;

    public Material HighlightMat;

    public static bool isGrabbed = false;
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

            //rumble.SendImpulse(_amplitude, _duration);

            // Check hand is inside pile
            if (isInsideBlackPile)
            {
                // Grabbing a token with fingers
                if (isGrabbed)
                {
                    isGrabbed = false;

                    Debug.Log("Rotation finger:  index: " + indexFingerJoin1.rotation.x);
                    Debug.Log("Rotation finger:  thumb: " + indexFingerJoin1.rotation.x);
                    // Undo flexed fingers
                    indexFingerJoin1.Rotate(-55, 0, 0);
                    thumbFingerJoint1.Rotate(-30, 0, 0);
                    Debug.Log("Done Rotating finger:  index: " + indexFingerJoin1.rotation.x);
                    Debug.Log("Done Rotating finger:  thumb: " + indexFingerJoin1.rotation.x);
                    // Hide the token
                    BlackToken.SetActive(false);
                }
                else
                {
                    isGrabbed = true;
                    // Flex fingers in to grab token
                    Debug.Log("Rotation finger:  index: " + indexFingerJoin1.rotation.x);
                    Debug.Log("Rotation finger:  thumb: " + indexFingerJoin1.rotation.x);
                    indexFingerJoin1.Rotate(55, 0, 0);
                    thumbFingerJoint1.Rotate(30, 0, 0);
                    Debug.Log("Done Rotating finger:  index: " + indexFingerJoin1.rotation.x);
                    Debug.Log("Done Rotating finger:  thumb: " + indexFingerJoin1.rotation.x);
                    // Show the token
                    BlackToken.SetActive(true);
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
