using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    // Update is called once per frame
    public float spinSpeed = 30;
    private bool playing = true;
    void Update()
    {
        if(!playing){return;}
        transform.RotateAround(transform.position,Vector3.forward,spinSpeed * Time.deltaTime);
    }
    public void Play()
    {
        playing = true;
    }
    public void Pause()
    {
        playing = false;    
    }
}
