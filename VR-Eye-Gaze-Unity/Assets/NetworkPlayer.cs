using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class NetworkPlayer : MonoBehaviour
{
    public Transform LeftEyeTransform;
    public Transform RightEyeTransform;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            LeftEyeTransform.gameObject.SetActive(false);
            RightEyeTransform.gameObject.SetActive(false);

            MapPosition(LeftEyeTransform, XRNode.LeftEye);
            MapPosition(RightEyeTransform, XRNode.RightEye);
        }
    }

    void MapPosition(Transform target, XRNode node)
    {
       // InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        //target.position = position;
        target.rotation.Set(rotation.x, rotation.y, rotation.z, rotation.w);
    }
}
