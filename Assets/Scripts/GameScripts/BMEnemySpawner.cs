using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BMEnemySpawner : MonoBehaviour {

	public GameObject player;
	public BMPlayer BMPlayer;

	public GameObject EnemySoldier;
	public GameObject eSpawner;
	public bool allowSpawn = true;

	public List<GameObject> enemySpawners = new List<GameObject>();


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("BloodMage");
		BMPlayer = player.GetComponent<BMPlayer> ();
	
		enemySpawners.AddRange (GameObject.FindGameObjectsWithTag("EnemySpawner").OrderBy(spawner => spawner.name).ToList<GameObject>());
		allowSpawn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (enemySpawners.Count > 0){
			eSpawner = enemySpawners [0];
			if (BMPlayer.arriving && allowSpawn == true) {
				StartCoroutine ("SpawnSoldiers");
			}
		}
	}

	IEnumerator SpawnSoldiers(){
		for (int i = 0; i < Random.Range (1, 3); i++) {
			Instantiate (EnemySoldier, eSpawner.transform.position, eSpawner.transform.rotation);
		}
		enemySpawners.RemoveAt (0);
		allowSpawn = false;
		yield return new WaitForSeconds (7.0f);
		allowSpawn = true;
	}
}
