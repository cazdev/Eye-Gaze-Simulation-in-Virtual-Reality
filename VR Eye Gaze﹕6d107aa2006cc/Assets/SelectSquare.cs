using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectSquare : MonoBehaviour
{
    public Material SolidHighlightMat;
    public InputActionReference action;
    public Transform indexFingerJoin1;
    public Transform thumbFingerJoint1;

    private GameObject TokenClone;
    private bool isInsideGameSquare = false;
    private Vector3 gameSquarePos = new Vector3();

    // Start is called before the first frame update
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
            if (isInsideGameSquare && GrabToken.isGrabbed)
            {
                // Make sure token is not grabbed anymore
                GrabToken.isGrabbed = false;
                isInsideGameSquare = false;

                // Clone token and place on game board square
                TokenClone = Instantiate(this.gameObject);
                TokenClone.GetComponent<SelectSquare>().enabled = false;
                TokenClone.transform.position = gameSquarePos;

                // Undo flexed fingers
                indexFingerJoin1.Rotate(-55, 0, 0);
                thumbFingerJoint1.Rotate(-30, 0, 0);

                // Disable/Hide this token in hand
                this.gameObject.SetActive(false);
            }
        };
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GameBoardSquare")
        {
            isInsideGameSquare = true;
            other.gameObject.GetComponent<MeshRenderer>().material = SolidHighlightMat;
            gameSquarePos = other.gameObject.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GameBoardSquare")
        {
            isInsideGameSquare = false;
            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.clear; ;
        }
    }
}
