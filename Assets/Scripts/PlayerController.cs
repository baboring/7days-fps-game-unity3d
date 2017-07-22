using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SB {

	[RequireComponent(typeof(CharacterController),typeof(UnitProperty))]
	[RequireComponent(typeof(Animator))]
	public class PlayerController : PooledObject {

		public Bullet bullet;
		public GameObject bulletSpawn;
		// Use this for initialization
		public Transform eyesTransform;
		public float jumpSpeed = 5.0f;
		public float gravity = 10;

		private UnitProperty unitInfo;

		Animator animator;
		CharacterController controller;
		Vector3 moveDirection;


		void Start () {
			unitInfo = GetComponent<UnitProperty>();
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
		void InputMovement() {
			// look cam view
			if(eyesTransform) {
				moveDirection = eyesTransform.forward * Input.GetAxis("Vertical");
	//			moveDirection +=  Vector3.Cross(eyesTransform.up, eyesTransform.forward).normalized * Input.GetAxis("Horizontal");
				moveDirection +=  eyesTransform.right * Input.GetAxis("Horizontal");
				animator.SetFloat("horz",Input.GetAxis("Horizontal") * unitInfo.walkSpeed);
				animator.SetFloat("vert",Input.GetAxis("Vertical") * unitInfo.walkSpeed);
			}
			else {
				// look back view
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				moveDirection = transform.TransformDirection(moveDirection);
				animator.SetFloat("horz",moveDirection.x * unitInfo.walkSpeed);
				animator.SetFloat("vert",moveDirection.z * unitInfo.walkSpeed);
			}

			if (controller.isGrounded && Input.GetButton("Jump")) {
				isJump = true;
				Debug.Log("Jump");
			}
		}

		void Shoot(float force)
		{
			Bullet clone = bullet.Instanciate<Bullet>();
			clone.transform.position = bulletSpawn.transform.position;
			clone.transform.rotation = bulletSpawn.transform.rotation;

			int maskLayer = Facade_NavMesh.GetLayerMask(
				LayerId.NonPlayer,
				LayerId.Geometry,
				LayerId.Obstacle); 
			// current look direction default
			Vector3 lookDir = transform.TransformDirection(Vector3.forward);

			// Camera direction default
			if(eyesTransform) {
				Debug.Assert(null != eyesTransform,"look object is null");
				lookDir = eyesTransform.forward;
			}

			//	Adjust aim more precisly 
			// RaycastHit hit;
			// if(Physics.Raycast(transform.position, lookDir, out hit, 1000 ,maskLayer)) {
			// 	Vector3 target = transform.position + (lookDir * hit.distance);
			// 	lookDir = (target - bulletSpawn.transform.position).normalized;
			// }
			
			clone.OnFire(unitInfo,lookDir,force);
		}		
		void Update() {
			if(Input.GetButtonDown("Fire1"))
				Shoot(100);
		}
		void FixedUpdate () {

			InputMovement();

			// calculate the surface shape
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hitInfo,
								controller.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
			Vector3 move = Vector3.ProjectOnPlane(moveDirection, hitInfo.normal).normalized;
			moveDirection.x = move.x * unitInfo.walkSpeed;
			moveDirection.z = move.z * unitInfo.walkSpeed;
			
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