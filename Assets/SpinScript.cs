using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    // Update is called once per frame
    public float spinSpeed = 30;
    void Update()
    {
        transform.RotateAround(transform.position,Vector3.forward,spinSpeed * Time.deltaTime);
    }
}
