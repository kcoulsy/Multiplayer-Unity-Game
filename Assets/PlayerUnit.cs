using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour {

	public float speed;
	public float jumpForce;
	private float moveInput;
	private Rigidbody2D rb;
	private bool facingRight = true;

	private bool isGrounded;
	public Transform groundCheck;
	public float checkRadius;
	public LayerMask whatIsGround;

	public int extraJumpsValue;
	private int extraJumps = 0;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasAuthority) {
			return;
		}
		Debug.Log(isGrounded);
		if (isGrounded) {
			extraJumps = extraJumpsValue;
		}

		if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0) {
			rb.velocity = Vector2.up * jumpForce;
			extraJumps--;
		} else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0) {
			rb.velocity = Vector2.up * jumpForce;
		}

		
		// if (Input.GetKeyDown(KeyCode.D)) {
		// 	Destroy(gameObject);
		// }
		
	}

	void FixedUpdate() {
		if (!hasAuthority) {
			return;
		}

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
		moveInput = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

		if (!facingRight && moveInput > 0) {
			Flip();
		} else if (facingRight && moveInput < 0){
			Flip();
		}
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 Scaler = transform.localScale;
		Scaler.x *= -1;
		transform.localScale = Scaler;
	}
}
