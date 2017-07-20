using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookSmooth : MonoBehaviour {

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

	void Update ()
	{
		if(m_cursorIsLocked) {
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
		UpdateCursorLock();
		
	}


	public bool lockCursor = true;
	bool m_cursorIsLocked = false;
	public void SetCursorLock(bool value)
	{
		lockCursor = value;
		if(!lockCursor) {//we force unlock the cursor if the user disable the cursor locking helper
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}	

	public void UpdateCursorLock()
	{
		//if the user set "lockCursor" we check & properly lock the cursos
		if (lockCursor)
			InternalLockUpdate();
	}

	private void InternalLockUpdate() {
		if(Input.GetKeyUp(KeyCode.Escape)) {
			m_cursorIsLocked = false;
		}
		else if(Input.GetMouseButtonUp(0)) {
			m_cursorIsLocked = true;
		}

		if (m_cursorIsLocked) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if (!m_cursorIsLocked) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
	

}
