using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public static bool IsGrabbed = false;
    public static bool GrabbedBlackToken = false;
    public static bool GrabbedWhiteToken = false;

    private bool isInsideBlackPile = false;
    private bool isInsideWhitePile = false;

    private TextMeshProUGUI IsGameStartedText;

    private void Start()
    {
        IsGameStartedText = GameObject.Find("IsGameStartedText (TMP)").GetComponent<TextMeshProUGUI>();

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
                if (IsGrabbed)
                {
                    IsGrabbed = false;

                    // Undo flexed fingers
                    indexFingerJoin1.Rotate(-55, 0, 0);
                    thumbFingerJoint1.Rotate(-30, 0, 0);
                    // Hide the token
                    BlackToken.SetActive(false);
                }
                else
                {
                    IsGameStartedText.SetText("IsGameStarted: True");
                    IsGrabbed = true;
                    // Flex fingers in to grab token
                    indexFingerJoin1.Rotate(55, 0, 0);
                    thumbFingerJoint1.Rotate(30, 0, 0);
                    // Show the token
                    BlackToken.SetActive(true);
                }
            }
            else if (isInsideWhitePile)
                {
                    // Grabbing a token with fingers
                    if (IsGrabbed)
                    {
                        IsGrabbed = false;

                        // Undo flexed fingers
                        indexFingerJoin1.Rotate(-55, 0, 0);
                        thumbFingerJoint1.Rotate(-30, 0, 0);
                        // Hide the token
                        WhiteToken.SetActive(false);
                    }
                    else
                    {
                        IsGameStartedText.SetText("IsGameStarted: True");
                        IsGrabbed = true;
                        // Flex fingers in to grab token
                        indexFingerJoin1.Rotate(55, 0, 0);
                        thumbFingerJoint1.Rotate(30, 0, 0);
                        // Show the token
                        WhiteToken.SetActive(true);
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
        else if (other.gameObject.tag == "WhiteTokenPile")
        {
            isInsideWhitePile = true;
            other.gameObject.GetComponent<MeshRenderer>().material = HighlightMat;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BlackTokenPile")
        {
            isInsideBlackPile = false;
            GrabbedBlackToken = true;
            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.clear; ;
        }
        else if (other.gameObject.tag == "WhiteTokenPile")
        {
            isInsideWhitePile = false;
            GrabbedWhiteToken = true;
            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.clear; ;
        }
    }
}
