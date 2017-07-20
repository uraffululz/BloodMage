using UnityEngine;
using System.Collections;

public class BMMeleeScript : MonoBehaviour {

	//GameObject Variables
	public GameObject player;
	public GameObject meleeWeapon;
	public GameObject meleeShield;
	public GameObject aerialRadius;

	//Component Variables
	Animator anim;
	BMPlayerMovement BMMove;

	//Melee Variables
	public int playerDamage;
	public bool allowAttack = true;
	public bool allowShield = true;
	//Attack Combo Variables
	public bool allowCombo = false;
	public bool allowCombo2 = false;
	public float comboTimer;
	public float comboTimer2;
	public float comboTimer3;
	//Aerial Attack Variables
	public float aerialCharge;
	public bool allowAerial = false;
	public bool aerialCompleted = false;
	public bool activateAerialRadius;


	void Awake (){
		player = GameObject.Find ("BloodMage");
		//meleeWeapon = GameObject.Find ("Melee");
		meleeWeapon.SetActive (true);
		//meleeShield = GameObject.Find ("Shield");
		aerialRadius.SetActive (true);
	}

	void Start () {
		BMMove = GameObject.Find ("BloodMage").GetComponent<BMPlayerMovement> ();
		anim = GetComponentInChildren<Animator> ();

		playerDamage = 10;
		aerialCharge = 1.0f;
	}
	

	void Update () {
		if (BMMove.allowJump == true) {
			playerDamage = 10;
			aerialCharge = 1.0f;
			aerialCompleted = false;
			aerialRadius.SetActive (false);
			meleeWeapon.GetComponent<MeshRenderer> ().material.color = Color.white;
			Melee ();
		} else if (BMMove.allowJump == false) {
			allowShield = false;
			AerialAttack ();
		}
		if (allowShield == true) {
			RaiseShield ();
		}
	}


	void Melee(){
		if (allowAttack == true && Input.GetKeyDown (KeyCode.E)) {
			meleeWeapon.SetActive (true);
			comboTimer = 0.7f;
		}
		if (comboTimer > 0.0f) {
			allowAttack = false;
			allowCombo = true;
			if (comboTimer <= 0.3f) {
				if (Input.GetKeyDown (KeyCode.E)) {
					Debug.Log ("Combo +1");
					anim.Play ("MeleeCombo2");
					comboTimer2 = 0.7f;
				}
			}
			comboTimer = comboTimer - 1.0f * Time.deltaTime;
			allowShield = false;
		} else if (comboTimer2 > 0.0f) {
			allowAttack = false;
			allowCombo = false;
			allowCombo2 = true;
			if (comboTimer2 <= 0.3f) {
				if (Input.GetKeyDown (KeyCode.E)) {
					Debug.Log ("Combo +2");
					anim.Play ("MeleeCombo3");
					comboTimer3 = 0.5f;
				}
			}
			comboTimer2 = comboTimer2 - 1.0f * Time.deltaTime;
			allowShield = false;
		} else if (comboTimer3 > 0.0f) {
			comboTimer3 = comboTimer3 - 1.0f * Time.deltaTime;
			allowShield = false;
		} else if (comboTimer <= 0.0 && comboTimer2 <= 0.0f && comboTimer3 <= 0.0f) {
			meleeWeapon.SetActive (false);
			allowAttack = true;
			allowShield = true;
			allowCombo = false;
			allowCombo2 = false;
		}
	}

	void AerialAttack(){
		if (aerialCompleted == false) {
			allowAerial = true;
		} else {
			AerialRadius ();
		}
		if (allowAerial == true && transform.position.y >= BMMove.jumpStartHeight.y + 1.0f && Input.GetKey (KeyCode.E)) {
			BMMove.allowMove = false;
			meleeWeapon.SetActive (true);
			meleeWeapon.GetComponent<BoxCollider> ().enabled = false;
			anim.Play ("ReadyAerial");			
			player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			player.GetComponent<Rigidbody> ().useGravity = false;
			if (aerialCharge < 3.0f) {
				aerialCharge = aerialCharge + 0.75f * Time.deltaTime;
			}
			if (aerialCharge >= 3.0f) {
				meleeWeapon.GetComponent<MeshRenderer> ().material.color = Color.red;
			} else if (aerialCharge >= 2.0f) {
				meleeWeapon.GetComponent<MeshRenderer> ().material.color = Color.magenta;
			}
		} else if (allowAerial == true && Input.GetKeyUp (KeyCode.E)) {
			BMMove.allowMove = true;
			meleeWeapon.GetComponent<BoxCollider> ().enabled = true;
			playerDamage = 10 * (int)aerialCharge;
			player.GetComponent<Rigidbody> ().useGravity = true;
			//player.GetComponent<Rigidbody> ().velocity = Vector3.down;
			anim.Play ("AerialAttack");
			allowAerial = false;
			aerialCompleted = true;
		}
	}

	void AerialRadius(){
		RaycastHit aerialHit = new RaycastHit ();
		activateAerialRadius = Physics.Raycast (meleeWeapon.transform.position, Vector3.down, out aerialHit, 1.0f);
		Debug.DrawRay (meleeWeapon.transform.position, Vector3.down, Color.red);
	
		if (activateAerialRadius) {
			aerialRadius.SetActive (true);
		}
	}

	void RaiseShield(){
		if (allowShield == true && Input.GetKey (KeyCode.R)) {
			meleeShield.SetActive (true);
			allowAttack = false;
			BMMove.allowMove = false;
		} else {
			meleeShield.SetActive (false);
			allowAttack = true;
			BMMove.allowMove = true;
		}
	}

}
