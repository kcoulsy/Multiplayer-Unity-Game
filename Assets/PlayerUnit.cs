using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour {

	private Transform  playerCamera, centerPoint, player;

	private float mouseX, mouseY;
	public float mouseSensitivity = 3f;
	public float mouseYPosition = 1f;

	private float moveFB, moveLR;
	public float moveSpeed = 2f;

	private float zoom;
	public float zoomSpeed = 2;
	public float zoomMin = -2f;
	public float zoomMax = -10f;

	public float rotationSpeed = 2f;

	// Use this for initialization
	void Start () {
		playerCamera = GameObject.Find("Camera").transform;
		centerPoint = GameObject.Find("CameraTarget").transform;
		player = gameObject.transform;
		zoom = -5;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasAuthority) {
			return;
		}
		ZoomCamera();
		RotateCamera();
		Move();

	}

	void ZoomCamera() {
		zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		if ( zoom > zoomMin ) {
			zoom = zoomMin;
		}
		if ( zoom < zoomMax ) {
			zoom = zoomMax;
		}
		playerCamera.transform.localPosition = new Vector3(0, 0, zoom);
	}

	void RotateCamera() {
		// Right mouse click
		if (Input.GetMouseButton(1)) {
			mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
			mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		}
		mouseY = Mathf.Clamp(mouseY, -30f, 85f);

		playerCamera.LookAt(centerPoint);
		centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
	}

	void Move() {
		//Handle wasd movement
		moveFB = Input.GetAxis("Vertical") * moveSpeed;
		moveLR = Input.GetAxis("Horizontal") * moveSpeed;

		Vector3 movement = new Vector3(moveLR, 0, moveFB);
		movement = player.rotation * movement;

		player.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
		centerPoint.position = new Vector3(player.position.x, player.position.y, player.position.z);

		if (Input.GetAxis("Vertical") != 0) {
			Quaternion turnAngle = Quaternion.Euler(0, centerPoint.eulerAngles.y, 0);
			player.rotation = Quaternion.Slerp(player.rotation, turnAngle, Time.deltaTime * rotationSpeed);
		}
	}
}
