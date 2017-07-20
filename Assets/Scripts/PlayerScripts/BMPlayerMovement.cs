using UnityEngine;
using System.Collections;

public class BMPlayerMovement : MonoBehaviour {

	//Component Variables
	Rigidbody rb;
	public BMMeleeScript BMMelee;

	//Movement Variables
	Vector3 movement;
	public int moveSpeed;
	public int rotSpeed;
	public bool allowMove = true;
	public int jumpHeight;
	public Vector3 jumpStartHeight;
	public bool allowJump;

	//Dashing Variables
	public KeyCode prevKeyDown = KeyCode.None;
	public int buttonCount = 0;
	public float dashTimer = 0.0f;

	//Status Variables
	public bool staggered;
	public float staggerTimer;


	void Start () {
		rb = GetComponent<Rigidbody>();
		BMMelee = GameObject.Find("MeleeEmpty").GetComponent<BMMeleeScript> ();
	}


	void Update () {
		if (staggered == true) {
			Staggering ();
		}
		if (allowMove == true) {
			PlayerJump ();
			PlayerDash ();
		}
	}


	void FixedUpdate(){
		if (allowMove == true && staggered == false) {
			MovePlayer ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag("EnemyAttack")){
			staggered = true;
			staggerTimer = 0.5f;
		}
	}


	void MovePlayer () {
		//Move UP
		if (Input.GetKey (KeyCode.W)) {
			rb.MovePosition (transform.position + Vector3.forward * moveSpeed * Time.deltaTime);
			//Vector3 rotUp = Vector3.RotateTowards (transform.position, Vector3.forward, rotSpeed, 0.0f);
			//transform.rotation = Quaternion.LookRotation (rotUp);
		}

		//Move DOWN
		else if (Input.GetKey (KeyCode.S)) {
			rb.MovePosition(transform.position + Vector3.back * moveSpeed * Time.deltaTime);
			//Vector3 rotDown = Vector3.RotateTowards (transform.position, Vector3.back, rotSpeed, 0.0f);
			//transform.rotation = Quaternion.LookRotation (rotDown);
		}

		//Move RIGHT
		if (Input.GetKey (KeyCode.D)) {
			rb.MovePosition(transform.position + Vector3.right * moveSpeed * Time.deltaTime);
			Vector3 rotRight = Vector3.RotateTowards (transform.position, Vector3.right, rotSpeed, 0.0f);
			transform.rotation = Quaternion.LookRotation (rotRight);
		}

		//Move LEFT
		else if (Input.GetKey (KeyCode.A)) {
			rb.MovePosition(transform.position + Vector3.left * moveSpeed * Time.deltaTime);
			Vector3 rotLeft = Vector3.RotateTowards (transform.position, Vector3.left, rotSpeed, 0.0f);
			transform.rotation = Quaternion.LookRotation (rotLeft);
		}
	}

	void PlayerJump(){
		RaycastHit jumpCastHit = new RaycastHit ();
		allowJump = Physics.SphereCast (transform.position, 0.4f, -Vector3.up, out jumpCastHit, 0.2f);

		if (Input.GetButtonDown ("Jump")) {
			if (allowJump) {
				jumpStartHeight = transform.position;
				rb.velocity = new Vector3 (0.0f, jumpHeight, 0.0f);
			}
		}
	}


	void PlayerDash(){
		//Dash UP
		if (Input.GetKeyDown (KeyCode.W)) {
			if (prevKeyDown != KeyCode.W) {
				buttonCount = 0;
			}
			if (prevKeyDown == KeyCode.W && dashTimer > 0 & buttonCount == 1) {
				//rb.MovePosition(transform.position + Vector3.forward * 5.0f);
				//Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, 2.0f);
				transform.Translate(Vector3.forward * 2.0f, Space.World);
				Debug.Log ("Dashing up");
			}
			if (buttonCount == 0) {
				dashTimer = 0.5f;
			}
			buttonCount += 1;
			prevKeyDown = KeyCode.W;
		}

		//Dash DOWN
		if (Input.GetKeyDown (KeyCode.S)) {
			if (prevKeyDown != KeyCode.S) {
				buttonCount = 0;
			}
			if (prevKeyDown == KeyCode.S && dashTimer > 0 & buttonCount == 1) {
				//rb.MovePosition(transform.position + Vector3.back * 2.0f);
				transform.Translate(Vector3.back * 2.0f, Space.World);
				Debug.Log ("Dashing down");
			}
			if (buttonCount == 0) {
				dashTimer = 0.5f;
			}
			buttonCount += 1;
			prevKeyDown = KeyCode.S;
		}

		//Dash RIGHT
		if (Input.GetKeyDown (KeyCode.D)) {
			if (prevKeyDown != KeyCode.D) {
				buttonCount = 0;
			}
			if (prevKeyDown == KeyCode.D && dashTimer > 0 & buttonCount == 1) {
				//rb.MovePosition(transform.position + Vector3.right * 2.0f);
				transform.Translate(Vector3.right * 2.0f, Space.World);
				Debug.Log ("Dashing right");
			}
			if (buttonCount == 0) {
				dashTimer = 0.5f;
			}
			buttonCount += 1;
			prevKeyDown = KeyCode.D;
		}

		//Dash LEFT
		if (Input.GetKeyDown (KeyCode.A)) {
			if (prevKeyDown != KeyCode.A) {
				buttonCount = 0;
			}
			if (prevKeyDown == KeyCode.A && dashTimer > 0 & buttonCount == 1) {
				//rb.MovePosition(transform.position + Vector3.left * 2.0f);
				transform.Translate(Vector3.left * 2.0f, Space.World);
				Debug.Log ("Dashing left");
			}
			if (buttonCount == 0) {
				dashTimer = 0.5f;
			}
			buttonCount += 1;
			prevKeyDown = KeyCode.A;
		}

		if (dashTimer > 0.0f) {
			dashTimer -= 1.0f * Time.deltaTime;
		} else {
			dashTimer = 0.0f;
			buttonCount = 0;
		}

		Vector3 playerPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

		//Keep Player from dashing OUT OF BOUNDS
		if (transform.position.z >= 2.5f) {
			playerPos.z = playerPos.z - 2.0f;
			rb.MovePosition (playerPos);
		} else if (transform.position.z <= -2.5f) {
			playerPos.z = playerPos.z + 2.0f;
			rb.MovePosition (playerPos);
		} else {
			playerPos = transform.position;
		}
	}

	void Staggering(){
		if (staggerTimer > 0.0f){
			allowMove = false;
			BMMelee.allowAttack = false;
			staggerTimer = staggerTimer - 1.0f * Time.deltaTime;
		} else {
			staggered = false;
			allowMove = true;
			BMMelee.allowAttack = true;
		}
	}
}
