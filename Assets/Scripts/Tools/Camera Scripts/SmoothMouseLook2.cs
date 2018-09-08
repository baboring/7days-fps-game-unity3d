using UnityEngine;
using System.Collections;
 
// Very simple smooth mouselook modifier for the MainCamera in Unity by FatiguedArtist
//(Francis R. Griffiths-Keam) - www.fatiguedartist.com
 
//[AddComponentMenu("Camera-Controller/Smooth Mouse Look ")]
public class SmoothMouseLook2 : MonoBehaviour {
 
    public bool lockCursor = false;
 
    public float sensitivity = 30;
    public int smoothing = 10;
 
    float ymove;
    float xmove;
 
    int iteration = 0;
 
    float xaggregate = 0;
    float yaggregate = 0;
 
    //int Ylimit = 0;
    public int Xlimit = 20;
 
    void Start()
    {
        // make the cursor invisible and locked?
         if (lockCursor)
            Screen.lockCursor = true;
    }
 
 
    void FixedUpdate () {
 
        // ensure mouseclicks do not effect the screenlock
 
        if (lockCursor)
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Screen.lockCursor = true;
        }
 
        float[] x = new float[smoothing];
        float[] y = new float[smoothing];
 
        // reset the aggregate move values
 
        xaggregate = 0;
        yaggregate = 0;
 
        // receive the mouse inputs
 
        ymove = Input.GetAxis("Mouse Y");
        xmove = Input.GetAxis("Mouse X");
 
        // cycle through the float arrays and lop off the oldest value, replacing with the latest
 
        y[iteration % smoothing] = ymove;
        x[iteration % smoothing] = xmove;
 
        iteration++;
 
        // determine the aggregates and implement sensitivity
 
        foreach (float xmov in x)
            xaggregate += xmov;
 
        xaggregate = xaggregate / smoothing * sensitivity;
 
        foreach (float ymov in y)
            yaggregate += ymov;
 
        yaggregate = yaggregate / smoothing * sensitivity;
 
        // turn the x start orientation to non-zero for clamp
 
        Vector3 newOrientation = transform.eulerAngles + new Vector3(-yaggregate, xaggregate, 0);
        //xclamp = Mathf.Clamp(newOrientation.x, Xlimit, 360-Xlimit)%360;
 
        // rotate the object based on axis input (note the negative y axis)
        transform.eulerAngles = newOrientation;
       
    }
}