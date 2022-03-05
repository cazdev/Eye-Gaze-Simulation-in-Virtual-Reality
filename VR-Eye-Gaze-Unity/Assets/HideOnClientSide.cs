using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HideOnClientSide : MonoBehaviour
{
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Player");
        } else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
