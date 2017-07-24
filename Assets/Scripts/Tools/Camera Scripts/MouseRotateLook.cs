/* *************************************************
*  Created:  7/24/2017, 12:11:26 AM
*  File:     MouseRotateLook.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotateLook : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		// Make the rigid body not change rotation
		// if (rigidbody)
		// 	rigidbody.freezeRotation = true;
	}
		
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;

	public float sensitivityX = 15f;
	public float sensitivityY = 15f;
	public float minimumX = -360F;
	public float maximumX = 360f;
	public float minimumY = -60f;
	public float maximumY = 60f;
	float rotationX = 0f;
	float rotationY = 0f;

	void FixedUpdate ()
	{
		if(turnOn) {
			switch(axes) {
				case RotationAxes.MouseXAndY:
					rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
					rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
					rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
					transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
					break;

				case RotationAxes.MouseX:
					transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
					break;

				case RotationAxes.MouseY:
					rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
					rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
					transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
					break;
			}
		}

	}


	public bool lockCursor = false;
	public bool turnOn = true;
	public void SetCursorLock(bool value)
	{
		turnOn = value;
		lockCursor = value;
		if (lockCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if (!lockCursor) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}		
	}	


	

}
