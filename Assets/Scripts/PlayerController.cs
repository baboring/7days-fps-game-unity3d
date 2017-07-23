﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SB {

	[RequireComponent(typeof(CharacterController),typeof(ObjectProperty))]
	[RequireComponent(typeof(Animator))]
	public class PlayerController : PooledObject {

		public Bullet bullet;
		public Weapon Weapon;
		// Use this for initialization
		public Transform eyesTransform;
		public float jumpSpeed = 5.0f;
		public float gravity = 10;

		private ObjectProperty unitInfo;

		Animator animator;
		CharacterController controller;
		Vector3 moveDirection;


		void Start () {
			unitInfo = GetComponent<ObjectProperty>();
			animator = GetComponent<Animator>();
			controller = GetComponent<CharacterController>();
		}

		public void ConnectToEyes(Transform cam) {
			if(cam) {
				eyesTransform = cam;
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
		void InputMovement(bool IsSprnit) {

			// look cam view
			if(eyesTransform) {
				moveDirection = eyesTransform.forward * Input.GetAxis("Vertical");
	//			moveDirection +=  Vector3.Cross(eyesTransform.up, eyesTransform.forward).normalized * Input.GetAxis("Horizontal");
				moveDirection +=  eyesTransform.right * Input.GetAxis("Horizontal");
				animator.SetFloat("horz",Input.GetAxis("Horizontal") * ((IsSprnit)? unitInfo.tb.runSpeed : unitInfo.tb.walkSpeed));
				animator.SetFloat("vert",Input.GetAxis("Vertical") * ((IsSprnit)? unitInfo.tb.runSpeed : unitInfo.tb.walkSpeed));
			}
			else {
				// look back view
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				moveDirection = transform.TransformDirection(moveDirection);
				animator.SetFloat("horz",moveDirection.x * unitInfo.tb.walkSpeed);
				animator.SetFloat("vert",moveDirection.z * ((IsSprnit)? unitInfo.tb.runSpeed : unitInfo.tb.walkSpeed));
			}

			if (controller.isGrounded && Input.GetButton("Jump")) {
				isJump = true;
				Debug.Log("Jump");
			}
		}

		void Shoot()
		{
			Debug.Assert(null != Weapon,"weapon is null!!");
			if(Weapon.Shoot(bullet, unitInfo, eyesTransform)) {
				//Debug.Log("Shoot!!");
			}
		}		
		void Update() {
			if(Input.GetButtonDown("Fire1"))
				Shoot();
		}
		void FixedUpdate () {

			bool IsSprnit = Input.GetKey(KeyCode.LeftShift);

			InputMovement(IsSprnit);

			// calculate the surface shape
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hitInfo,
								controller.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
			Vector3 move = Vector3.ProjectOnPlane(moveDirection, hitInfo.normal).normalized;
			moveDirection.x = move.x *  ((IsSprnit)? unitInfo.tb.runSpeed : unitInfo.tb.walkSpeed);
			moveDirection.z = move.z *  ((IsSprnit)? unitInfo.tb.runSpeed : unitInfo.tb.walkSpeed);
			
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
}