using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedCameraIsMine : MonoBehaviour
{
    private PhotonView photonView;
    public GameObject CameraGameObject;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            CameraGameObject.SetActive(true);
        } else
        {
            CameraGameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
