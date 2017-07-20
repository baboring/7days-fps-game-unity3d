using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

	// Use this for initialization
	public Transform lookTransform;
	public float speed = 1f;
	public float jumpSpeed = 5.0f;
	public float gravity = 10;

	Animator animator;
	CharacterController controller;
	Vector3 moveDirection;


	void Start () {
		animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
		if(lookTransform) {
			Camera playerCam = lookTransform.GetComponent<Camera>();
			if(playerCam) {
				Debug.Log("switch camera");
				Camera.main.enabled = false;
				playerCam.enabled = true;
			}
		}
	}
	
	void InputMovementTest() {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
	}
	// Update is called once per frame

	bool isJump = false;
	void InputMovement() {
		// look cam view
		if(lookTransform) {
			moveDirection = lookTransform.forward * Input.GetAxis("Vertical");
//			moveDirection +=  Vector3.Cross(lookTransform.up, lookTransform.forward).normalized * Input.GetAxis("Horizontal");
			moveDirection +=  lookTransform.right * Input.GetAxis("Horizontal");
			animator.SetFloat("horz",Input.GetAxis("Horizontal") * speed);
			animator.SetFloat("vert",Input.GetAxis("Vertical") * speed);
		}
		else {
			// look back view
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			animator.SetFloat("horz",moveDirection.x * speed);
			animator.SetFloat("vert",moveDirection.z * speed);
		}

		if (controller.isGrounded && Input.GetButton("Jump")) {
			isJump = true;
			Debug.Log("Jump");
		}
	}
	void Update() {

	}
	void FixedUpdate () {

		InputMovement();

		// calculate the surface shape
		RaycastHit hitInfo;
		Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hitInfo,
							controller.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
		Vector3 move = Vector3.ProjectOnPlane(moveDirection, hitInfo.normal).normalized;
		moveDirection.x = move.x*speed;
		moveDirection.z = move.z*speed;
		
        if (controller.isGrounded) {

			if(isJump) {
				isJump = false;
				moveDirection.y = jumpSpeed;
			}
        }
		else {
			moveDirection += new Vector3(0,-gravity,0) * Time.fixedDeltaTime;
		}

 
        controller.Move(moveDirection * Time.fixedDeltaTime);		
    }

}
