using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class SelectSquare : MonoBehaviour
{
    public Material SolidHighlightMat;
    public InputActionReference action;
    public Transform indexFingerJoin1;
    public Transform thumbFingerJoint1;
    public GameObject TokenCloneOnBoard;

    private GameObject TokenClone;
    private bool isInsideGameSquare = false;
    private Vector3 gameSquarePos = new Vector3();
    PhotonView view;

    // Start is called before the first frame update
    private void Start()
    {
        view = GetComponent<PhotonView>();

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
                // Place on to gameboard
                //PlaceOnGameboard();
                view.RPC("PlaceOnGameboard", RpcTarget.All);
            }
        };
    }

    [PunRPC]
    void PlaceOnGameboard()
    {
        // Make sure token is not grabbed anymore
        GrabToken.isGrabbed = false;
        isInsideGameSquare = false;

        // Clone token and place on game board square
        TokenClone = Instantiate(TokenCloneOnBoard);
        // Move to palcement location
        TokenClone.transform.position = gameSquarePos;

        // Undo flexed fingers
        indexFingerJoin1.Rotate(-55, 0, 0);
        thumbFingerJoint1.Rotate(-30, 0, 0);

        // Disable/Hide this token in hand
        this.gameObject.SetActive(false);
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
