using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectLocation : MonoBehaviour
{

    public Transform headTransform;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        this.transform.position = headTransform.position;
        this.transform.rotation = headTransform.rotation;

        //Wait for 1 seconds
        yield return new WaitForSeconds(1);

        this.transform.position = headTransform.position;
        this.transform.rotation = headTransform.rotation;

        //Wait for 2 seconds
        yield return new WaitForSeconds(2);

        this.transform.position = headTransform.position;
        this.transform.rotation = headTransform.rotation;
    }
}
