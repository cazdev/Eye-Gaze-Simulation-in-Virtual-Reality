using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;

public class SelectSquare : MonoBehaviour
{
    public Material SolidHighlightMat;
    public InputActionReference action;
    public Transform indexFingerJoin1;
    public Transform thumbFingerJoint1;

    private bool isInsideGameSquare = false;
    private Vector3 gameSquarePos = new Vector3();
    PhotonView view;
    public LogFile log;
    private TextMeshProUGUI PlayerTurnText;
    private TileCoordinates gameSquarePlacement;

    // Start is called before the first frame update
    private void Start()
    {
        PlayerTurnText = GameObject.Find("PlayerTurnText (TMP)").GetComponent<TextMeshProUGUI>();
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
            if (isInsideGameSquare && GrabToken.IsGrabbed)
            {
                // Place on to gameboard
                PlaceOnGameboard();
                //view.RPC("PlaceOnGameboard", RpcTarget.All);
            }
        };
    }

    //[PunRPC]
    void PlaceOnGameboard()
    {
        // Make sure token is not grabbed anymore
        GrabToken.IsGrabbed = false;
        isInsideGameSquare = false;

        if (GrabToken.GrabbedBlackToken)
        {
            // Clone token and place on game board square
            PhotonNetwork.Instantiate("TokenClone Black", gameSquarePos, Quaternion.identity, 0);
            GrabToken.GrabbedBlackToken = false;
            PlayerTurnText.SetText("PlayerTurn: White");
            if (log != null)
            {
                // write the time and the placement x and y positions to the file
                log.WriteLine(Time.time, "event_token_placed_black", gameSquarePlacement.Row, gameSquarePlacement.Column);
            }
        } 
        else if (GrabToken.GrabbedWhiteToken) {
            // Clone token and place on game board square
            PhotonNetwork.Instantiate("TokenClone White", gameSquarePos, Quaternion.identity, 0);
            GrabToken.GrabbedWhiteToken = false;
            PlayerTurnText.SetText("PlayerTurn: Black");
            if (log != null)
            {
                // write the time and the placement x and y positions to the file
                log.WriteLine(Time.time, "event_token_placed_white", gameSquarePlacement.Row, gameSquarePlacement.Column);
            }
        }

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
            gameSquarePlacement = other.gameObject.GetComponent<TileCoordinates>();
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
