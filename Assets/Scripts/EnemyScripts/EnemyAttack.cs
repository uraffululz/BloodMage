using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour {

/*This script is basically used to determine what happens when an enemy attack strikes,
 * depending on who/what it hits
*/

	public GameObject player;
	public BMPlayer BMPlayer;

	public GameObject soldier;
	public VassalList VassalList;
	public List<GameObject> Vassals;

	public int damagePlayer;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		BMPlayer = player.GetComponent<BMPlayer> ();
		//soldier = GameObject.transform.parent;
		VassalList = soldier.GetComponent<VassalList> ();
		Vassals = VassalList.Vassals;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){
//Create a script which has all the different damage amounts/formulas/variables in one place,
//based on enemy type, level/strength, and the attack used by that enemy
		if (other.CompareTag ("Player")) {
			if (gameObject.name == "Arrow(Clone)") {
				damagePlayer = 5;
			} else if (gameObject.name == "EnemySword") {
				damagePlayer = 10;
			}
			BMPlayer.health = BMPlayer.health - damagePlayer;
		}
		if (other.CompareTag ("Vassal")) {
			if (gameObject.name == "Arrow(Clone)") {
				damagePlayer = 5;
			} else if (gameObject.name == "EnemySword") {
				damagePlayer = 10;
			}
			other.GetComponent<BMVassalScript> ().vHealth = other.GetComponent<BMVassalScript> ().vHealth - damagePlayer;
		}
		if (other.CompareTag ("PlayerShield")) {
			gameObject.SetActive (false);
		}
		if (gameObject.name == "Arrow(Clone)") {
			//gameObject.SetActive (false);
			Destroy (gameObject);
		}
	}
}
