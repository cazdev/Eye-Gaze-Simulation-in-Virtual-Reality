//========= Copyright 2018, HTC Corporation. All rights reserved. ===========
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Photon.Pun;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class SRanipal_GazeRaySample_v2 : MonoBehaviour
            {
                public int LengthOfRay = 25;
                [SerializeField] private LineRenderer GazeRayRenderer;
                private static EyeData_v2 eyeData = new EyeData_v2();
                private bool eye_callback_registered = false;
                public LogFile log;
                public LogFile logCollisions;
                private TextMeshProUGUI EyeGazeVectorText;

                // Start is called before the first frame update
                void Start()
                {

                    EyeGazeVectorText = GameObject.Find("EyeGazeVectorText (TMP)").GetComponent<TextMeshProUGUI>();

                    if (!SRanipal_Eye_Framework.Instance.EnableEye)
                    {
                        enabled = false;
                        return;
                    }
                    Assert.IsNotNull(GazeRayRenderer);
                }

                private void FixedUpdate()
                {
                    if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

                    if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
                    {
                        SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = true;
                    }
                    else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
                    {
                        SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }

                    Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;

                    if (eye_callback_registered)
                    {
                        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else return;
                    }
                    else
                    {
                        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else return;
                    }

                    Vector3 GazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionCombinedLocal);
                    GazeRayRenderer.SetPosition(0, Camera.main.transform.position - Camera.main.transform.up * 0.05f);
                    GazeRayRenderer.SetPosition(1, Camera.main.transform.position + GazeDirectionCombined * LengthOfRay);

                    // Calculate direction from these 2 points
                    Vector3 fromPosition = Camera.main.transform.position;
                    Vector3 toPosition = Camera.main.transform.position + GazeDirectionCombined * LengthOfRay;
                    Vector3 direction = toPosition - fromPosition;

                    if (log != null)
                    {
                        log.WriteLine(Time.time, direction.x, direction.y, direction.z);
                    }
                    EyeGazeVectorText.SetText("EyeGazeVector: " + GazeDirectionCombined.ToString());

                    RaycastHit hitPoint;
                    Ray ray = new Ray(fromPosition, direction);
                    if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity))
                    {
                        // Convert from world space to local
                        var localHitPoint = hitPoint.transform.InverseTransformPoint(hitPoint.point);
                        var worldHitPoint = hitPoint.point;
                        // Get collisions
                        if (logCollisions != null)
                        {
                            Debug.Log(hitPoint.collider.tag);
<<<<<<< Updated upstream
                            if (hitPoint.collider.tag != "Untagged" && hitPoint.collider.tag != "GameBoardSquare" && hitPoint.collider.tag != "Player"
                                && hitPoint.collider.tag != "WhiteTokenOnBoard" && hitPoint.collider.tag != "BlackTokenOnBoard" && hitPoint.collider.tag != "GameBoard"
                                && hitPoint.collider.tag != "Gameboard")
                            {
                                logCollisions.WriteLine(Time.time, "event_looked_at_" + hitPoint.collider.tag, localHitPoint.x, localHitPoint.y, localHitPoint.z, worldHitPoint.x, worldHitPoint.y, worldHitPoint.z);
                            }
                        }
                    }

                    RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

                    // For each object that the raycast hits.
                    foreach (RaycastHit hit in hits)
                    {
                        // Convert from world space to local
                        var localHitPoint = hit.transform.InverseTransformPoint(hit.point);
                        var worldHitPoint = hit.point;

                        // separate logging for gameboard
                        // Get collisions
                        if (logCollisions != null)
                        {
                            Debug.Log(hit.collider.tag);
                            if (hit.collider.tag == "GameBoard" && hit.collider.tag == "Gameboard")
=======
                            // separate logging for gameboard
                            if (hitPoint.collider.tag == "Gameboard")
                            {
                                logCollisions.WriteLine(Time.time, "event_looked_at_" + hitPoint.collider.tag, localHitPoint.x, localHitPoint.y, localHitPoint.z, worldHitPoint.x, worldHitPoint.y, worldHitPoint.z);
                            }
                            if (hitPoint.collider.tag != "GameBoardSquare" && hitPoint.collider.tag != "Player"
                                && hitPoint.collider.tag != "WhiteTokenOnBoard" && hitPoint.collider.tag != "BlackTokenOnBoard" && hitPoint.collider.tag != "Gameboard")
>>>>>>> Stashed changes
                            {
                                logCollisions.WriteLine(Time.time, "event_looked_at_" + hit.collider.tag, localHitPoint.x, localHitPoint.y, localHitPoint.z, worldHitPoint.x, worldHitPoint.y, worldHitPoint.z);
                            }
                        }
                    }
                }

                private Vector3 GazeDirectionCombined = new Vector3();

                void FixedUpdate()
                {
                    // Calculate direction from these 2 points
                    Vector3 fromPosition = Camera.main.transform.position - Camera.main.transform.up * 0.05f;
                    Vector3 toPosition = Camera.main.transform.position + GazeDirectionCombined * LengthOfRay;
                    Vector3 direction = toPosition - fromPosition;

                    if (log != null)
                    {
                        log.WriteLine(Time.time, direction.x, direction.y, direction.z);
                    }
                    EyeGazeVectorText.SetText("EyeGazeVector: " + GazeDirectionCombined.ToString());

                    RaycastHit hitPoint;
                    Ray ray = new Ray(fromPosition, direction);
                    if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity))
                    {
                        // Convert from world space to local
                        var localHitPoint = hitPoint.transform.InverseTransformPoint(hitPoint.point);
                        var worldHitPoint = hitPoint.point;

                        // Get collisions
                        if (logCollisions != null && hitPoint.collider.tag != "Untagged")
                        {
                            if (hitPoint.collider.tag != "GameBoardSquare" && hitPoint.collider.tag != "Player"
                                && hitPoint.collider.tag != "WhiteTokenOnBoard" && hitPoint.collider.tag != "BlackTokenOnBoard" && hitPoint.collider.tag != "Gameboard"
                                && hitPoint.collider.tag != "GameBoard")
                            {
                                logCollisions.WriteLine(Time.time, "event_looked_at_" + hitPoint.collider.tag, localHitPoint.x, localHitPoint.y, localHitPoint.z, worldHitPoint.x, worldHitPoint.y, worldHitPoint.z);
                            }
                        }
                    }

                    // Separate logging for gamboard
                    RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

                    // For each object that the raycast hits.
                    foreach (RaycastHit hit in hits)
                    {
                        var localHitPoint = hit.transform.InverseTransformPoint(hit.point);
                        var worldHitPoint = hit.point;

                        Debug.Log(hit.collider.tag);
                        // separate logging for gameboard
                        if (hit.collider.tag == "Gameboard")
                        {
                            logCollisions.WriteLine(Time.time, "event_looked_at_" + hit.collider.tag, localHitPoint.x, localHitPoint.y, localHitPoint.z, worldHitPoint.x, worldHitPoint.y, worldHitPoint.z);
                        }
                    }
                }

                private void Release()
                {
                    if (eye_callback_registered == true)
                    {
                        SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }
                }
                private static void EyeCallback(ref EyeData_v2 eye_data)
                {
                    eyeData = eye_data;
                }
            }
        }
    }
}
