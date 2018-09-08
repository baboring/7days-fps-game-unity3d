using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDirection : MonoBehaviour {

	public bool isSmooth = true;
	private Vector3 lastPos;
	public float speed = 0.1f;
	public float slerpTime = 100f;
	// Use this for initialization
	void Start () {
	   lastPos = transform.position; // initialize lastPos
	}
	
	// Update is called once per frame
	void Update () {
		float horz = Input.GetAxis("Horizontal") * speed;
		float vert = Input.GetAxis("Vertical") * speed;

		transform.position += new Vector3(horz,0,vert);

        // calculate direction moved since last frame:
        var dir = transform.position - lastPos;
        dir.y = 0; // only the horizontal direction matters
        if (dir.magnitude > 0.05) { // if moved at least 5 cm (2 inches)...
            lastPos = transform.position; // update last position...
    		if(isSmooth)
				transform.forward = Vector3.Lerp(transform.forward, dir.normalized, slerpTime * Time.deltaTime);
			else
            	transform.forward = dir.normalized; // and set its direction
	    }
    }

 
}
