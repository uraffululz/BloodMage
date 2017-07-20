using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VassalList : MonoBehaviour {

	public List<GameObject> Vassals = new List<GameObject> ();
	public GameObject currentVassal;

	// Use this for initialization
	void Start () {
		Vassals.AddRange (GameObject.FindGameObjectsWithTag ("Vassal"));
	}
	
	// Update is called once per frame
	void Update () {
		//When an enemy spawns, add it to the list
		if (GameObject.FindGameObjectsWithTag ("Vassal").Length > Vassals.Count) {
			Vassals.Clear ();
			Vassals.AddRange (GameObject.FindGameObjectsWithTag ("Vassal"));
		}
		//When an enemy dies, remove it from the list
		if (GameObject.FindGameObjectsWithTag ("Vassal").Length < Vassals.Count) {
			Vassals.Clear ();
			Vassals.AddRange (GameObject.FindGameObjectsWithTag ("Vassal"));
		}

		if (Vassals.Count > 0) {
			currentVassal = null;
			StartCoroutine ("GetVassal");
		}
	}

	IEnumerator GetVassal(){
		currentVassal = null;
		float minDist = Mathf.Infinity;
		foreach (GameObject vassalTarget in Vassals) {
			float dist = Vector3.Distance (transform.position, vassalTarget.transform.position);
			if (dist < minDist) {
				currentVassal = vassalTarget;
				minDist = dist;
			}
		}
		yield return currentVassal;
	}
}
