using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyList : MonoBehaviour {


	public List<GameObject> Enemies = new List<GameObject>();
	float minDist;

	//public GameObject player;
	public GameObject currentTarget;


	void Start () {
		//player = GameObject.FindGameObjectWithTag ("Player");
		Enemies.InsertRange (0, GameObject.FindGameObjectsWithTag ("Enemy"));

		minDist = Mathf.Infinity;
		StartCoroutine ("GetTarget");
	}
	

	void Update () {
		//When an enemy spawns, add it to the list
		if (GameObject.FindGameObjectsWithTag ("Enemy").Length > Enemies.Count) {
			Enemies.Clear ();
			Enemies.AddRange (GameObject.FindGameObjectsWithTag ("Enemy"));
		}
		//When an enemy dies, remove it from the list
		if (GameObject.FindGameObjectsWithTag ("Enemy").Length < Enemies.Count) {
			Enemies.Clear ();
			Enemies.AddRange (GameObject.FindGameObjectsWithTag ("Enemy"));
		}
			
//Get this so that it only runs when currentTarget is null
//or another enemy gets closer to the player than currentTarget is
		if (Enemies.Count > 0) {
			//currentTarget = null;
			StartCoroutine ("GetTarget");
		}
	}

	IEnumerator GetTarget(){
		currentTarget = null;
		minDist = Mathf.Infinity;
		foreach (GameObject target in Enemies) {
			float dist = Vector3.Distance (transform.position, target.transform.position);
			if (dist < minDist) {
				currentTarget = target;
				minDist = dist;
			}
		}
		yield return currentTarget;
	}
}
