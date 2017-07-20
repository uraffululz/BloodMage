using UnityEngine;
using System.Collections;

public class BMPlayer : MonoBehaviour {

	//GameObject references
	GameObject player;
	public GameObject enemy;
	public GameObject vassal;
	public GameObject ashPile;
	public bool EnemyAlive;
	public BMMeleeScript BMMelee;
	public BMPlayerMovement BMMove;
	public EnemyList EnemyList;

	//Component Variables
	//Rigidbody rb;

	//Stat Variables
	public int health = 50;
	public int maxHealth = 100;
	public int bloodMP = 50;
	public int maxBloodMP = 100;

	//Level-Progression Variables
	public bool proceeding = false;
	public bool arriving = false;

	//Distance Variables
	public float enemyDistance;
	public int siphonRange;

	/*
	//BloodSpell Variables
	public float spellRange;
	*/


	void Awake(){
		//spellRange = 1.0f;
	}

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		BMMove = GetComponent<BMPlayerMovement> ();
		BMMelee = GameObject.Find ("MeleeEmpty").GetComponent<BMMeleeScript> ();
		EnemyList = GetComponent<EnemyList>();
		//rb = GetComponent<Rigidbody>();

		InvokeRepeating ("DrainBloodMP", 1.0f, 1.0f);

	}


	void Update() {
		enemy = EnemyList.currentTarget;
		//If the player is not staggered, he is allowed to use his Siphon and/or other abilities
		if (BMMove.staggered == false) {
			SiphonEnemy ();
			//BloodSpell ();
		}
		if (bloodMP <= 0) {
			//When bloodMP runs out:
			//The Player will basically become a feral vampire, filled with "bloodlust"
				//He will take damage while being exposed to sunlight
				//He will only be able to attack with melee, and they will do greater damage
				//He will be unable to use spells/skills
			
		}
	}

	public void OnTriggerEnter(Collider other){
		//Proceeding to the next area of the map
		if (other.CompareTag("ProceedPoint")){
			//If there are no more enemies, the player is allowed to proceed to the next area of the map
			if (EnemyList.Enemies.Count == 0) {
				proceeding = true;
				arriving = false;
				other.gameObject.SetActive (false);
			}
		}

		//When arriving at the next area on the map
		if (other.CompareTag ("ArrivePoint")) {
			proceeding = false;
			arriving = true;
			other.gameObject.SetActive (false);
		}
	}


	void DrainBloodMP(){
		bloodMP -= 1;
	}
		

	void SiphonEnemy(){
		if (enemy != null) {
//The "alive" bool of all enemies should be on the same shared script for all enemies, to simplify this
//It is a quality that all enemies will possess
			if (enemy.name == "EnemySoldier") {
				EnemyAlive = enemy.GetComponent<BMEnemyScript> ().alive;
			} else if (enemy.name == "EnemyArcher") {
				EnemyAlive = enemy.GetComponent<BMEnemyArcher> ().alive;
			}
			enemyDistance = Vector3.Distance (transform.position, enemy.transform.position);

			if (enemy.activeInHierarchy && EnemyAlive == false) {
				if(enemyDistance < siphonRange){
					if (Input.GetKeyDown (KeyCode.F)) {
						//Siphon Health from Enemy
						Instantiate (ashPile, enemy.transform.position, Quaternion.LookRotation(Vector3.up));
						enemy.SetActive (false);
						health = health + 100;
						if (health > maxHealth) {
							health = maxHealth;
						}
						enemy = EnemyList.currentTarget;
					}

					else if (Input.GetKeyDown (KeyCode.G)){
						//Siphon MP from Enemy
						Instantiate (ashPile, enemy.transform.position, Quaternion.LookRotation(Vector3.up));
						enemy.SetActive (false);
						bloodMP = bloodMP + 100;
						if (bloodMP > maxBloodMP) {
							bloodMP = maxBloodMP;
						}
						enemy = EnemyList.currentTarget;
					}

					else if (Input.GetKeyDown(KeyCode.H)){
						//Enemy becomes Vassal
						Instantiate (vassal, enemy.transform.position, player.transform.rotation);
						enemy.SetActive(false);
					}
				}
			}
		}
	}

	/*
	void BloodSpell(){
		if (Input.GetKeyDown (KeyCode.B) && bloodMP >= 20) {
			GetComponent<SphereCollider> ().enabled = true;
			GetComponent<ParticleSystem> ().Play ();
		}
		if (Input.GetKey (KeyCode.B) && bloodMP >= 20 && spellRange < 3.0f) {
			spellRange += 0.5f * Time.deltaTime;
			GetComponent<SphereCollider> ().radius = spellRange;
			//ParticleSystem.ShapeModule PSSM = GetComponent<ParticleSystem.ShapeModule> ();
			//PSSM.radius = spellRange;
		}
		if (Input.GetKeyUp (KeyCode.B)) {
			if (bloodMP >= 20) {
				foreach (GameObject targetEnemy in EnemyList.Enemies) {
					if (Vector3.Distance (transform.position, targetEnemy.transform.position) <= GetComponent<SphereCollider> ().radius) {
						targetEnemy.GetComponent<BMEnemyScript> ().eHealth = targetEnemy.GetComponent<BMEnemyScript> ().eHealth - 10;
					}
				}
				bloodMP -= 20;
				spellRange = 1.0f;
			}
			GetComponent<SphereCollider> ().enabled = false;
			GetComponent<ParticleSystem> ().Stop ();
		}
	}
*/

}
