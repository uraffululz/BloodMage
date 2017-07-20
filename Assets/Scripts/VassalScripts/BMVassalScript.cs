using UnityEngine;
using System.Collections;

public class BMVassalScript : MonoBehaviour {

	//Get Components from self
	Rigidbody rb;
	UnityEngine.AI.NavMeshAgent VNav;

	//Get Other GameObjects
	public GameObject player;
	public GameObject vassalSword;
	public GameObject ashPile;

	//Get Components from other GameObjects
	//From Player
	public BMPlayer BMPlayer;
	public BMPlayerMovement BMPlayerMovement;
	public EnemyList EnemyList;
	public GameObject currentTarget;
	//From Enemies
	public BMEnemyScript BMEnemyScript;
	//From Camera
	public BMCamera BMCamera;

	//Stat Variables
	public int vHealth;
	public bool alive = true;

	//Targeting & Attack Variables
	public int alertDist;
	public int attackDist;
	public bool allowAttack;
	public bool staggered = false;
	public float vStaggerTimer;
	//Following Variables
	public bool allowFollow;
	public float distToPlayer;


	void Start () {
		rb = GetComponent<Rigidbody> ();
		VNav = GetComponent<UnityEngine.AI.NavMeshAgent> ();

		player = GameObject.FindGameObjectWithTag ("Player");

		BMPlayer = player.GetComponent<BMPlayer> ();
		BMPlayerMovement = player.GetComponent<BMPlayerMovement> ();
		EnemyList = GetComponent<EnemyList> ();
		BMCamera = Camera.main.GetComponent<BMCamera> ();

		alive = true;
		allowAttack = true;
		allowFollow = true;
	}
	

	void FixedUpdate (){
		if (alive == true && vHealth <= 0) {
			alive = false;
			VassalDeath ();
		} else if (alive == true && vHealth > 0) {
			if (staggered != true) {
				FollowPlayer ();
			}
		} else if (alive == false) {
			allowFollow = false;
			allowAttack = false;
			VNav.enabled = false;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("EnemyAttack")){
			if (alive == true && staggered == false) {
				Staggering ();
				vStaggerTimer = 0.5f;
			}
		}
	}


	void FollowPlayer(){
		distToPlayer = Vector3.Distance (transform.position, player.transform.position);

		if (VNav.enabled == true && VNav.isOnNavMesh == true) {
			
			if (/*distToPlayer >= 3.0f && */allowFollow == true && VNav.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial
			    && BMPlayerMovement.allowJump == true) {
				WarpToPlayer ();
			} else if (/*distToPlayer < 3.0f && */EnemyList.Enemies.Count > 0 && EnemyList.currentTarget.GetComponent<BMEnemyScript> ().alive == true) {
				currentTarget = EnemyList.currentTarget;

				if (Vector3.Distance (transform.position, currentTarget.transform.position) <= alertDist) {
					VNav.SetDestination (currentTarget.transform.position);		

					if (allowAttack == true && Vector3.Distance (transform.position, currentTarget.transform.position) <= attackDist) {
						StartCoroutine ("VassalAttack");
					}
				}
			} else {
				if (allowFollow == true) {
					VNav.SetDestination (player.transform.position);
				}
			}
		}

		else if (VNav.enabled == false) {
				if (transform.position.y <= -2.0f) {
					StartCoroutine ("WarpLanding");
				}
			}
		}


	void WarpToPlayer(){
		VNav.enabled = false;
		GetComponent<CapsuleCollider> ().enabled = false;
		rb.velocity = new Vector3(0.0f, -1.0f, 0.0f);
	}


	IEnumerator WarpLanding(){
		Vector3 warpDrop = new Vector3 (Random.Range (-1.0f, 1.0f), 0.0f, (Random.Range (-1.0f, 1.0f)));
		rb.MovePosition (player.transform.position + warpDrop);
		rb.velocity = new Vector3 (0.0f, 3.0f, 0.0f);
		GetComponent<CapsuleCollider> ().enabled = true;
		yield return new WaitForSeconds (0.5f);
		VNav.enabled = true;
	}
		

	IEnumerator VassalAttack(){
		transform.LookAt (VNav.destination);
		VNav.stoppingDistance = 1.0f;
		yield return new WaitForSeconds (0.5f);
		if (staggered == false) {
			vassalSword.SetActive (true);
			allowAttack = false;
			allowFollow = false;
			yield return new WaitForSeconds (0.5f);
			vassalSword.SetActive (false);
			yield return new WaitForSeconds (0.5f);
			allowAttack = true;
			allowFollow = true;
			if (alive == true && VNav.enabled == true) {
				VNav.ResetPath ();
			}
		}
	}

	void Staggering(){
		staggered = true;
		if (staggered == true && vStaggerTimer > 0.0f) {
			vassalSword.SetActive (false);
			transform.position = transform.position;
			allowAttack = false;
			allowFollow = false;
			VNav.enabled = false;
			vStaggerTimer = vStaggerTimer - 1.0f * Time.deltaTime;
		} else {
			VNav.enabled = true;
			staggered = false;
			allowAttack = true;
			allowFollow = true;
		}
	}

	void VassalDeath(){
		vassalSword.SetActive (false);
		Instantiate (ashPile, transform.position, Quaternion.LookRotation(Vector3.up));
		gameObject.SetActive (false);
	}
}
