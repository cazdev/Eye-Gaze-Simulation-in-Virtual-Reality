using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RemoveColliderOnClient : MonoBehaviour
{
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
