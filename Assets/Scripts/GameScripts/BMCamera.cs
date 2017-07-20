using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BMCamera : MonoBehaviour {

	public GameObject player;
	public BMPlayer BMPlayer;
	public EnemyList EnemyList;

	//List and Positioning Variables
	public List<GameObject> CamPoints = new List<GameObject>();
	public Vector3 currentCamPoint;
	Vector3 offset;
	public bool followPlayer;

	public List<GameObject> proceedPoints = new List<GameObject>();





	// Use this for initialization
	void Start () {
		player = GameObject.Find ("BloodMage");
		BMPlayer = player.GetComponent<BMPlayer> ();
		EnemyList = player.GetComponent<EnemyList> ();

		CamPoints.AddRange (GameObject.FindGameObjectsWithTag ("CamPoint").OrderBy(overview => overview.name).ToList<GameObject>());
		currentCamPoint = transform.position;
		offset = new Vector3 (player.transform.position.x, 4.25f, -3.0f);
		followPlayer = false;

		proceedPoints.AddRange(GameObject.FindGameObjectsWithTag("ProceedPoint").OrderBy(point => point.name).ToList<GameObject>());
		proceedPoints [0].GetComponent<ParticleSystem> ().Stop ();
	}

	void Update(){
		if (proceedPoints.Count != GameObject.FindGameObjectsWithTag ("ProceedPoint").Length) {
			proceedPoints.Clear ();
			proceedPoints.AddRange (GameObject.FindGameObjectsWithTag ("ProceedPoint").OrderBy (point => point.name).ToList<GameObject> ());
		}
		if(proceedPoints.Count >= 1){
			if (EnemyList.Enemies.Count == 0 && followPlayer == false) {
				//Enable "GO" Prompts
				proceedPoints [0].GetComponent<TextMesh> ().text = "GO";
				proceedPoints [0].GetComponent<ParticleSystem> ().Play ();
			} else {
					proceedPoints [0].GetComponent<TextMesh> ().text = "";
					proceedPoints [0].GetComponent<ParticleSystem> ().Stop ();
				}
		}
	}
	

	void LateUpdate () {
		offset = new Vector3 (player.transform.position.x, 5.0f, -3.0f);


		if (followPlayer == true) {
			transform.position = Vector3.MoveTowards(transform.position, offset, 0.05f);
		}
			
		if (BMPlayer.proceeding){
			followPlayer = true;
			CamPoints.RemoveAt (0);
			currentCamPoint = CamPoints[0].transform.position;
			BMPlayer.proceeding = false;
		}

		if (BMPlayer.arriving) {
			followPlayer = false;
			transform.position = Vector3.MoveTowards (transform.position, currentCamPoint, 0.05f);
			if (transform.position == currentCamPoint) {
				BMPlayer.arriving = false;
			}
		}
	}
}
