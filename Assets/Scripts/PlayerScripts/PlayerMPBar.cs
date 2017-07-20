using UnityEngine;
using System.Collections;

public class PlayerMPBar : MonoBehaviour {

	public GameObject playerMPBar;
	public BMPlayer BMPlayer;
	public float pMaxMP;
	public float pBloodMP;


	void Start () {
		BMPlayer = GetComponent<BMPlayer> ();
	}


	void Update () {
		pBloodMP = BMPlayer.bloodMP;
		pMaxMP = BMPlayer.maxBloodMP;
		if (pBloodMP <= 0){
			pBloodMP = 0;
		}
		float calcPMP = pBloodMP / pMaxMP;
		SetPlayerHealth (calcPMP);
	}
		
	public void SetPlayerHealth(float playerMP){
		playerMPBar.transform.localScale = new Vector3
			(playerMP, playerMPBar.transform.localScale.y, playerMPBar.transform.localScale.z);
	}
}
