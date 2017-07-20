using UnityEngine;
using System.Collections;

public class PlayerHPBar : MonoBehaviour {

	public GameObject playerHPBar;
	public BMPlayer BMPlayer;
	public float pMaxHP;
	public float pHealth;


	void Start () {
		BMPlayer = GetComponent<BMPlayer> ();
	}
	

	void Update () {
		pHealth = BMPlayer.health;
		pMaxHP = BMPlayer.maxHealth;
		if (pHealth <= 0){
			pHealth = 0;
		}
		float calcPHealth = pHealth / pMaxHP;
		SetPlayerHealth (calcPHealth);
	}

	public void SetPlayerHealth(float playerHealth){
		playerHPBar.transform.localScale = new Vector3
			(playerHealth, playerHPBar.transform.localScale.y, playerHPBar.transform.localScale.z);
	}
}
