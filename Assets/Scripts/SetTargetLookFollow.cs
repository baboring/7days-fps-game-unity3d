using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetLookFollow : MonoBehaviour {

	public bool isSmooth = true;
	public Transform target;
	public float slerpTime = 100f;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if(null == target)
			return;
        // calculate direction moved since last frame:
        var dir = target.forward - this.transform.forward;
        dir.y = 0; // only the horizontal direction matters
        if (dir.magnitude > 0.05) { // if moved at least 5 cm (2 inches)...
			Vector3 look = this.transform.forward;
			look.y = 0;
    		if(isSmooth)
				target.transform.forward = Vector3.Lerp(target.transform.forward, look, slerpTime * Time.deltaTime);
			else
            	target.transform.forward = look; // and set its direction
	    }
    }
}
