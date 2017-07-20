using UnityEngine;
using System.Collections;

public class BMEnemyScript : MonoBehaviour {

	//GameObject Reference Variables
	public GameObject player;
	public GameObject enemySword;
	Rigidbody rb;
	UnityEngine.AI.NavMeshAgent eNav;
	VassalList VassalList;
	BMMeleeScript BMMelee;

	//Stat Variables
	public int eHealth;
	public bool alive = true;

	//Movement and Pursuit Variables
	//public int moveSpeed;
	public bool allowFollow;
	public float playerDist;
	public float vassalDist;
	public int alertDist;

	//Attack Variables
	public bool allowAttack;
	public float attackDist;
	public bool staggered = false;
	public float eStaggerTimer;



	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		allowFollow = true;
		allowAttack = true;
		rb = GetComponent<Rigidbody> ();
		eNav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		VassalList = GetComponent<VassalList> ();
		BMMelee = GameObject.Find("MeleeEmpty").GetComponent<BMMeleeScript> ();
	}
	

	void Update () {
		if (alive == true && eHealth <= 0) {
			alive = false;
			EnemyDeath ();
		} else if (alive == true && eHealth > 0) {
			if (staggered != true) {
				AggroPlayer ();
			}
		} else if (alive == false) {
			allowFollow = false;
			allowAttack = false;
			eNav.enabled = false;
		}
	}


	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag ("Insta-Death")) {
//Maybe just destroy it instead
			gameObject.SetActive (false);
		}

		if (other.gameObject.CompareTag ("PlayerAttack")) {
			eHealth = eHealth - BMMelee.playerDamage;
			if (alive == true && staggered == false){
				StartCoroutine ("Staggering");
				eStaggerTimer = 1.0f;
			}
		}
		if (other.gameObject.CompareTag ("VassalAttack")) {
			eHealth = eHealth - 2;
			if (alive == true && staggered == false){
				Staggering ();
				eStaggerTimer = 0.5f;
			}
		}
	}


	void AggroPlayer(){
		playerDist = Vector3.Distance (transform.position, player.transform.position);
		eNav.updateRotation = true;

		//Player Spotted
		if (allowFollow == true && playerDist <= alertDist) {
			eNav.stoppingDistance = 1.5f;
			if (player.GetComponent<BMPlayerMovement> ().allowJump == false) {
				eNav.destination = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
			} else {
				eNav.speed = 3;
				eNav.SetDestination (player.transform.position);
				transform.LookAt (eNav.destination);
			}
			//Follow/Attack Player
			if (allowAttack == true && playerDist <= attackDist) {
				if (player.GetComponent<BMPlayerMovement> ().allowJump == true) {
					StartCoroutine ("AttackPlayer");
				}
			}
		}
		//Attack Vassal if it gets too close
		else if (allowFollow == true && playerDist >= attackDist) {
			if (VassalList.currentVassal != null && VassalList.Vassals.Count > 0) {
				vassalDist = Vector3.Distance (transform.position, VassalList.currentVassal.transform.position);
				if (vassalDist <= alertDist && VassalList.currentVassal.GetComponent<BMVassalScript>().alive == true) {
					eNav.SetDestination (VassalList.currentVassal.transform.position);
					transform.LookAt (eNav.destination);
					if (vassalDist <= attackDist) {
						StartCoroutine ("AttackPlayer");
					}
				}
			}
		} else {
			transform.position = transform.position;
		}
	}
		

	IEnumerator AttackPlayer(){
		transform.LookAt (eNav.destination);
		eNav.stoppingDistance = 0.5f;
		yield return new WaitForSeconds (0.5f);
		if (staggered == false) {
			enemySword.SetActive (true);
			allowAttack = false;
			allowFollow = false;
			yield return new WaitForSeconds (0.4f);
			enemySword.SetActive (false);
			yield return new WaitForSeconds (0.5f);
			allowAttack = true;
			allowFollow = true;
			if (alive == true && eNav.enabled == true) {
				eNav.ResetPath ();
			}
		}
	}

	void Staggering(){
		staggered = true;
		if (staggered == true && eStaggerTimer > 0.0f) {
			enemySword.SetActive (false);
			allowAttack = false;
			allowFollow = false;
			eNav.enabled = false;
			eStaggerTimer = eStaggerTimer - 1.0f * Time.deltaTime;
		} else {
			eNav.enabled = true;
			staggered = false;
			allowAttack = true;
			allowFollow = true;
		}
	}


	void EnemyDeath(){
		enemySword.SetActive (false);
		eNav.enabled = !eNav.enabled;
		gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
		rb.constraints = RigidbodyConstraints.FreezeRotationY;
	}
}
